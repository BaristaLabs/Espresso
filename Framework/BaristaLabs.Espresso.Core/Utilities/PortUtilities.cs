namespace BaristaLabs.Espresso.Core.Utilities
{
    using System.Net;
    using System.Net.Sockets;

    public static class PortUtilities
    {
        /// <summary>
        /// Returns a free port on the interface for the specified IPAddress.
        /// </summary>
        /// <remarks>
        /// Hint: It's in the Bahamas.
        /// </remarks>
        /// <param name="address"></param>
        /// <returns></returns>
        public static int FindFreePort(IPAddress address = null)
        {
            if (address == null)
                address = IPAddress.Any;

            int port;
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                var pEndPoint = new IPEndPoint(address, 0);
                socket.Bind(pEndPoint);
                pEndPoint = (IPEndPoint)socket.LocalEndPoint;
                port = pEndPoint.Port;
            }
            finally
            {
                socket.Close();
            }
            return port;
        }
    }
}
