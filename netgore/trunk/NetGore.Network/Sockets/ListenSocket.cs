using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using log4net;

namespace NetGore.Network
{
    /// <summary>
    /// A socket that asynchronously listens for connections, accepts them and returns them
    /// through the OnAccept event.
    /// </summary>
    public class ListenSocket : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SocketAsyncEventArgs for the accept event
        /// </summary>
        readonly SocketAsyncEventArgs _acceptEventArgs;

        /// <summary>
        /// Socket used for the connection
        /// </summary>
        readonly Socket _socket;

        /// <summary>
        /// If the ListenSocket is disposed
        /// </summary>
        bool _disposed;

        /// <summary>
        /// Notifies when a connection has been accepted
        /// </summary>
        public event ListenSocketAcceptHandler OnAccept;

        /// <summary>
        /// ListenSocket constructor
        /// </summary>
        /// <param name="port">Port to listen on</param>
        public ListenSocket(int port)
        {
            // Set up the accept event args
            _acceptEventArgs = new SocketAsyncEventArgs();
            _acceptEventArgs.Completed += EndAccept;

            try
            {
                // Use IPAddress.Any for public
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Loopback, port);
                _socket = new Socket(localEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                if (log.IsInfoEnabled)
                    log.InfoFormat("Socket created on address {0}", localEndPoint.ToString());

                _socket.Bind(localEndPoint);
                if (log.IsInfoEnabled)
                    log.Info("Socket binded");

                _socket.Listen(5);
                if (log.IsInfoEnabled)
                    log.Info("Socket listening");
            }
            catch (SocketException ex)
            {
                if (log.IsFatalEnabled)
                    log.Fatal("Failed creating ListenSocket.", ex);
                throw;
            }

            // Start accepting right away
            BeginAccept();
        }

        /// <summary>
        /// If the listen socket is correctly working and accepting connections
        /// </summary>
        public bool IsAlive
        {
            get { return _socket.IsBound; }
        }

        /// <summary>
        /// Begins async accepting - use this instead of socket.BeginAccept
        /// </summary>
        void BeginAccept()
        {
            // Clear the accept socket reference from the last accept
            _acceptEventArgs.AcceptSocket = null;

            // Try to accept again
            try
            {
                if (!_socket.AcceptAsync(_acceptEventArgs))
                    EndAccept(this, _acceptEventArgs);
            }
            catch (Exception ex)
            {
                if (log.IsFatalEnabled)
                    log.Fatal("Failed to begin async accept.", ex);
                throw;
            }
        }

        /// <summary>
        /// Disposes of the ListenSocket
        /// </summary>
        /// <param name="disposeManaged">If true, managed resources will be disposed</param>
        void Dispose(bool disposeManaged)
        {
            if (_disposed)
                return;

            _disposed = true;
            if (disposeManaged)
            {
                if (_socket != null)
                    _socket.Close();
            }
        }

        void EndAccept(object sender, SocketAsyncEventArgs e)
        {
            Socket sock = e.AcceptSocket;

            if (sock == null)
            {
                if (log.IsErrorEnabled)
                    log.Error("Socket acquired from AsyncAcceptCallback is null.");
                Debug.Fail("Socket acquired from AsyncAcceptCallback is null.");
                return;
            }

            // Check if the EndAccept was caused by the ListenSocket closing down
            if (e.SocketError == SocketError.OperationAborted)
            {
                Dispose(true);
                return;
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Connection request from {0}", sock.RemoteEndPoint);

            // Set up the new TCPSocket and send it to the accept event
            TCPSocket conn = new TCPSocket();
            conn.SetSocket(sock);
            if (OnAccept != null)
                OnAccept(conn);

            // Start accepting again on the ListenSocket
            BeginAccept();
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes the socket and shuts it down
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}