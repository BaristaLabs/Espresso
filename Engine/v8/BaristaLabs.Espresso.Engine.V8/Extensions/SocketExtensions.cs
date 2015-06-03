namespace BaristaLabs.Espresso.Engine.V8.Extensions
{
    using System;
    using System.Net.Sockets;

    internal static class SocketExtensions
    {
        public static void SendBytesAsync(this Socket socket, byte[] bytes, Action<bool> callback)
        {
            socket.SendBytesAsync(bytes, 0, bytes.Length, callback);
        }

        public static void SendBytesAsync(this Socket socket, byte[] bytes, int offset, int count, Action<bool> callback)
        {
            if (!MiscHelpers.Try(() => socket.BeginSend(bytes, offset, count, SocketFlags.None, result => socket.OnBytesSent(result, bytes, offset, count, callback), null)))
            {
                callback(false);
            }
        }

        public static void OnBytesSent(this Socket socket, IAsyncResult result, byte[] bytes, int offset, int count, Action<bool> callback)
        {
            int sentCount;
            if (MiscHelpers.Try(out sentCount, () => socket.EndReceive(result)) && (sentCount > 0))
            {
                if (sentCount >= count)
                {
                    callback(true);
                }
                else
                {
                    socket.SendBytesAsync(bytes, offset + sentCount, count - sentCount, callback);
                }
            }
            else
            {
                callback(false);
            }
        }

        public static void ReceiveBytesAsync(this Socket socket, int count, Action<byte[]> callback)
        {
            socket.ReceiveBytesAsync(new byte[count], 0, count, callback);
        }

        public static void ReceiveBytesAsync(this Socket socket, byte[] bytes, int offset, int count, Action<byte[]> callback)
        {
            if (!MiscHelpers.Try(() => socket.BeginReceive(bytes, offset, count, SocketFlags.None, result => socket.OnBytesReceived(result, bytes, offset, count, callback), null)))
            {
                callback(null);
            }
        }

        public static void OnBytesReceived(this Socket socket, IAsyncResult result, byte[] bytes, int offset, int count, Action<byte[]> callback)
        {
            int receivedCount;
            if (MiscHelpers.Try(out receivedCount, () => socket.EndReceive(result)) && (receivedCount > 0))
            {
                if (receivedCount >= count)
                {
                    callback(bytes);
                }
                else
                {
                    socket.ReceiveBytesAsync(bytes, offset + receivedCount, count - receivedCount, callback);
                }
            }
            else
            {
                callback(null);
            }
        }
    }
}
