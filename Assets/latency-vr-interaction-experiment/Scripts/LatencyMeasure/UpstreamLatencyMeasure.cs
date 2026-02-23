using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using VContainer;

public class UpstreamLatencyMeasurer : NetworkBehaviour
{
    private enum MeasurementState
    {
        NotStarted,
        BaselineMeasuring,
        WaitingForEmulator,
        LatencyMeasuring
    }

    [Header("計測設定")]
    [SerializeField] private int _baselinePingCount = 100; // ベースラインを測る回数
    [SerializeField] private float _pingInterval = 0.1f;   // Pingの送信間隔（秒）

    private MeasurementState _currentState = MeasurementState.NotStarted;
    private int _currentPingIndex = 0;
    private double _baselineRttSum = 0;
    private double _averageBaselineRtt = 0;
    private double _estimatedDownstreamLatency = 0;
    private float _nextPingTime = 0;

    private List<double> _latencyMeasurements = new List<double>(); // 本計測の遅延値を保存するリスト

    private ResultWriter _resultWriter;

    [Inject]
    public void Construct(ResultWriter resultWriter)
    {
        _resultWriter = resultWriter;
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("UpstreamLatencyMeasurerが起動しました");
        Debug.Log($"IsClient: {IsClient}, IsServer: {IsServer}");
        if (!IsClient) return;
        
        Debug.Log("【クライアント】ベースライン計測を開始します...");
        _currentState = MeasurementState.BaselineMeasuring;
        _nextPingTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        if (!IsClient) return;

        // ステートごとの処理
        switch (_currentState)
        {
            case MeasurementState.BaselineMeasuring:
            case MeasurementState.LatencyMeasuring:
                if (Time.realtimeSinceStartup >= _nextPingTime)
                {
                    _nextPingTime = Time.realtimeSinceStartup + _pingInterval;
                    SendPingToServer();
                }
                break;

            default:
                break;
        }
    }

    [ClientRpc]
    public void StartMeasureUpstreamLatencyClientRpc()
    {
        // 本計測を開始する
        _currentState = MeasurementState.LatencyMeasuring;
        _nextPingTime = Time.realtimeSinceStartup;
    }

    [ClientRpc]
    public void StopMeasureUpstreamLatencyClientRpc()
    {
        // 計測を停止する
        _currentState = MeasurementState.NotStarted;
    }

    private void SendPingToServer()
    {
        // 高精度な現在時刻（double型）を送信
        double sendTime = Time.realtimeSinceStartupAsDouble;
        PingServerRpc(sendTime);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void PingServerRpc(double clientSendTime, RpcParams rpcParams = default)
    {
        // サーバーは受け取った時刻をそのままクライアントに送り返す
        // 送信元のクライアントにだけClientRpcを飛ばす設定
        ulong senderClientId = rpcParams.Receive.SenderClientId;
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { senderClientId }
            }
        };

        PongClientRpc(clientSendTime, clientRpcParams);
    }

    [ClientRpc]
    private void PongClientRpc(double clientOriginalSendTime, ClientRpcParams rpcParams = default)
    {
        double receiveTime = Time.realtimeSinceStartupAsDouble;
        double rtt = receiveTime - clientOriginalSendTime; // 往復遅延(秒)
        double rttMs = rtt * 1000.0; // ミリ秒に変換

        if (_currentState == MeasurementState.BaselineMeasuring)
        {
            _baselineRttSum += rttMs;
            _currentPingIndex++;

            // ベースラインが計測完了した場合
            if (_currentPingIndex >= _baselinePingCount)
            {
                // ベースライン計測完了時の計算
                _averageBaselineRtt = _baselineRttSum / _baselinePingCount;
                // 下りの遅延はRTTの半分と仮定する
                _estimatedDownstreamLatency = _averageBaselineRtt / 2.0;

                Debug.Log($"【クライアント】ベースライン完了: 平均RTT = {_averageBaselineRtt:F2}ms, 推定下り遅延 = {_estimatedDownstreamLatency:F2}ms");

                // サーバーに完了を報告
                ReportBaselineCompleteServerRpc(_averageBaselineRtt, _estimatedDownstreamLatency);

                // エミュレータ起動待ち状態へ移行
                _currentState = MeasurementState.WaitingForEmulator;
                Debug.Log("【待機中】外部ネットワークエミュレータで遅延を設定し、タスクを開始してください。");
            }
        }
        else if (_currentState == MeasurementState.LatencyMeasuring)
        {
            // 本計測：今回計測したRTTから、固定された下り遅延を引いて上り遅延を算出
            double upstreamLatencyMs = rttMs - _estimatedDownstreamLatency;

           _latencyMeasurements.Add(upstreamLatencyMs);
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void ReportBaselineCompleteServerRpc(double avgRtt, double estDownstreamLatency, RpcParams rpcParams = default)
    {
        ulong senderClientId = rpcParams.Receive.SenderClientId;
        Debug.Log($"【サーバー】クライアント({senderClientId})がベースライン計測完了。平均RTT: {avgRtt:F2}ms, 推定下り遅延: {estDownstreamLatency:F2}ms");
    }

    [ClientRpc]
    public void SaveLatencyClientRpc(int participantId1, int participantId2, int latencyCondition)
    {
        // クライアント側で遅延データを保存する処理を呼び出す
        _resultWriter.WriteResultsToCSV(GetWritableUpstreamLatency(), "UpstreamLatency", participantId1, participantId2, latencyCondition);
        _resultWriter.WriteResultsToCSV(GetWritableBaselineLatency(), "BaselineLatency", participantId1, participantId2, latencyCondition);
    }

    private WritableData GetWritableUpstreamLatency()
    {
        var writable = new WritableData(new List<string> { "UpstreamLatency" }, new List<List<string>>());
        foreach (var latency in _latencyMeasurements)
        {
            writable.AddData(new List<string> { latency.ToString() });
        }
        Debug.Log(writable.Data[0][0]);

        return writable;
    }

    private WritableData GetWritableBaselineLatency()
    {
        var writable = new WritableData(new List<string> { "AverageRTT", "EstimatedDownstreamLatency" }, new List<List<string>>());
        writable.AddData(new List<string> { _averageBaselineRtt.ToString(), _estimatedDownstreamLatency.ToString() });
        Debug.Log(writable.Data[0][0]);
        return writable;
    }
}