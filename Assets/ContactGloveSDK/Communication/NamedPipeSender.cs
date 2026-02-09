using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ContactGloveSDK
{
    public class NamedPipeSender<T>
    {
        private readonly string _pipeName;
        private readonly string _serverName;
        private NamedPipeClientStream _pipeClient;
        private bool _isConnecting;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public NamedPipeSender(string serverName, string pipeName)
        {
            _serverName = serverName;
            _pipeName = pipeName;
            _pipeClient = new NamedPipeClientStream(serverName, pipeName, PipeDirection.Out, PipeOptions.Asynchronous);
            _isConnecting = false;
        }

        private async UniTask ConnectIfNeeded()
        {
            if (_pipeClient != null && _pipeClient.IsConnected)
                return;

            if (_isConnecting)
                return;

            _isConnecting = true;
            _pipeClient?.Dispose();
            _pipeClient = new NamedPipeClientStream(_serverName, _pipeName, PipeDirection.Out, PipeOptions.Asynchronous);

            try
            {
                await _pipeClient.ConnectAsync(500, _cancellationTokenSource.Token);
                Debug.Log("Pipe connection established.");
            }
            catch (TimeoutException ex)
            {
                Debug.LogError($"Failed to connect to pipe: {ex.Message}");
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Connection attempt was canceled.");
            }
            finally
            {
                _isConnecting = false;
            }
        }

        public async UniTask Send(T message)
        {
            try
            {
                await ConnectIfNeeded();

                if (!_pipeClient.IsConnected)
                {
                    Debug.LogWarning("PipeClient is not connected.");
                    return;
                }

                var json = JsonHelper.Serialize(message);
                var buffer = Encoding.UTF8.GetBytes(json);

                await _pipeClient.WriteAsync(buffer, 0, buffer.Length, _cancellationTokenSource.Token);
                await _pipeClient.FlushAsync(_cancellationTokenSource.Token);
            }
            catch (TimeoutException ex)
            {
                Debug.LogError($"Timeout error sending message: {ex.Message}");
            }
            catch (IOException ex)
            {
                Debug.LogError($"IO error (possible broken pipe) sending message: {ex.Message}");
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Send operation was canceled.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error sending message: {ex.Message}");
            }
        }

        public void Close()
        {
            _cancellationTokenSource.Cancel();
            _pipeClient?.Close();
            _pipeClient?.Dispose();
            _pipeClient = null;
            _isConnecting = false;
            _cancellationTokenSource.Dispose();
        }
    }
}
