using UnityEngine;
using System.Collections.Generic;

public class PlayerData
{
    public int LatencyCondition { get; private set; }

    // プレイヤーID: クライアントID
    private Dictionary<int, ulong> _playerIdToClientIdDict = new Dictionary<int, ulong>();
    // プレイヤーID: 参加者ID
    private Dictionary<int, int> _playerIdToParticipantIdDict = new Dictionary<int, int>();

    public ulong? GetClientId(int playerId) => _playerIdToClientIdDict.TryGetValue(playerId, out var clientId) ? clientId : null;
    public int? GetParticipantId(int playerId) => _playerIdToParticipantIdDict.TryGetValue(playerId, out var participantId) ? participantId : null;

    public void AddPlayer(int playerId, ulong clientId, int participantId)
    {
        _playerIdToClientIdDict[playerId] = clientId;
        _playerIdToParticipantIdDict[playerId] = participantId;
    }

    public void SetLatencyCondition(int latencyCondition)
    {
        LatencyCondition = latencyCondition;
    }
}
