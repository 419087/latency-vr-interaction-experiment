using UnityEngine;
using System;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using ContactGloveSDK.NamedPipe;
using Cysharp.Threading.Tasks;

namespace ContactGloveSDK
{
    public class NamedPipeReceiver<T>
    {
        public delegate void OnDataReceived(T data);

        private readonly string _pipeName;
        private readonly OnDataReceived _onDataReceived;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly byte[] _buffer = new byte[8192]; // Reuse the buffer to avoid frequent allocations
        private NamedPipeServerStream _pipe;

        public NamedPipeReceiver(string pipeName, OnDataReceived onDataReceived)
        {
            _pipeName = pipeName;
            _onDataReceived = onDataReceived;
        }

        public void Run()
        {
            Loop().Forget();
        }

        private async UniTaskVoid Loop()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                if (_pipe == null || !_pipe.IsConnected)
                {
                    _pipe?.Dispose();
                    _pipe = new NamedPipeServerStream(_pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                    Debug.Log("Waiting for connection");
                    await _pipe.WaitForConnectionAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
                }

                var message = await ReceiveAsync();
                if (message == null) continue;
                _onDataReceived(message);
            }
            Debug.Log("Exiting loop");
        }

        private async UniTask<T> ReceiveAsync()
        {
            try
            {
                var len = await _pipe.ReadAsync(_buffer, 0, _buffer.Length, _cancellationTokenSource.Token).ConfigureAwait(false);
                if (len == 0) return default; // Connection has been closed or terminated

                var json = Encoding.UTF8.GetString(_buffer, 0, len);
#if DEBUG
                // Debug.Log($"Received message: {json}");
#endif
                return JsonHelper.Deserialize<T>(json); // Ensure JsonHelper.Deserialize is efficient or consider an alternative
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.LogError($"Error receiving message: {ex.Message}");
#endif
                _pipe?.Dispose();
                _pipe = null; // Ensure the pipe is recreated on the next loop iteration if an error occurs
                return default;
            }
        }

        public void Close()
        {
            _cancellationTokenSource.Cancel();
            _pipe?.Dispose();
            _pipe = null; // Ensure resources are released
        }
    }
}
