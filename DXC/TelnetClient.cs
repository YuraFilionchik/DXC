using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace DXC
{
    public class TelnetClient : IDisposable, IServiceProvider
    {
        protected readonly Socket TheSocket;
        private readonly byte[] _mBuffer;
        private SocketError _mError;
        private readonly AsyncCallback _receiveDataCallback;
        private readonly AsyncCallback _initalizeCallback;
        private readonly BinaryReader _mReader;

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        protected void FireDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (DataReceived != null)
                DataReceived(sender, e);
        }

        protected void FireDataReceived(int count)
        {
            FireDataReceived(this, new DataReceivedEventArgs(count, _mBuffer));
        }

        public TelnetClient()
        {
            TheSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _mBuffer = new byte[32767];
            _receiveDataCallback = new AsyncCallback(ReceiveData);
            _initalizeCallback = new AsyncCallback(Initialize);
            _mReader = new BinaryReader(new MemoryStream(_mBuffer), Encoding.ASCII);
        }

        private void Initialize(IAsyncResult ar)
        {
            TheSocket.BeginReceive(_mBuffer, 0, _mBuffer.Length, SocketFlags.None, out _mError,
                _receiveDataCallback, null);
        }

        private void ReceiveData(IAsyncResult ar)
        {
            FireDataReceived(TheSocket.EndReceive(ar));
        }

        public void Send(byte[] buffer)
        {
            TheSocket.EndSend(TheSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, out _mError, _initalizeCallback, null));
        }

        public void Send(string s)
        {
            Send(Encoding.ASCII.GetBytes(s));
        }

        public void Send(char c)
        {
            Send(c.ToString());
        }

        

        public void Connect(EndPoint remoteEp)
        {
            TheSocket.Connect(remoteEp);
            Initialize(null);
        }

        public SocketError LastError
        {
            get { return _mError; }
        }

        public BinaryReader InputBuffer
        {
            get { return _mReader; }
        }

        public void Connect(string host, int port)
        {
            TheSocket.Connect(host, port);
            Initialize(null);
        }

        public void Connect(IPAddress ip, int port)
        {
            TheSocket.Connect(ip,port);
            Initialize(null);
        }

        public static string ParseData(byte[] bs)
        {
            byte[] bytes = new byte[bs.Length];
            int n = 0;
            foreach (byte b in bs)
            {
                switch (b)
                {
                    case (byte)Verbs.Do:
                    case (byte)Verbs.Dont:
                    case (byte)Verbs.Iac:
                    case (byte)Verbs.Will:
                    case (byte)Verbs.Wont:
                    case 0:
                    case 1:
                    case 3:
                        break;
                    default:
                        bytes[n] = b;
                        n++;
                        break;
                }
            }

            return Encoding.Default.GetString(bytes);
        }
        public void Connect(byte[] ip, int port)
        {
            Connect(ConvertToIPv4(ip), port);
        }

        public void Connect(Uri url)
        {
            Connect(url.AbsoluteUri, url.Port);
            Initialize(null);
        }

        public bool IsConnected()
        {
            return TheSocket.Connected;
        }
        public void Disconnect()
        {
            TheSocket.Disconnect(false);
        }

        

        #region IDisposable Members

        public void Dispose()
        {
            _mReader.Close();
            TheSocket.Close();
        }

        #endregion

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            if (serviceType.FullName == typeof(Socket).FullName)
                return TheSocket;
            return null;
        }

        #endregion

        public static explicit operator Socket(TelnetClient client)
        {
            return client.TheSocket;
        }

        public static IPAddress ConvertToIPv4(byte[] address)
        {
            return new IPAddress(address);
        }

        ~TelnetClient()
        {
            Dispose();
        }
    }
}
