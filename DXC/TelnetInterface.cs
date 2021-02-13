// minimalistic telnet implementation
// conceived by Tom Janssens on 2007/06/06  for codeproject
//
// http://www.corebvba.be



using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace DXC
{


    enum Options
    {
        Sga = 3
    }

    class TelnetConnection
    {
        TcpClient _tcpSocket;

        int _timeOutMs = 100;

        public TelnetConnection(string hostname, int port)
        {
            _tcpSocket = new TcpClient(hostname, port);
       
        }

        public void Close()
        {
        	if(IsConnected)
        	_tcpSocket.Close();
        }
   
        public string Login(string username,string password,int loginTimeOutMs)
        {
            int oldTimeOutMs = _timeOutMs;
            _timeOutMs = loginTimeOutMs;
            string s = Read();
           // if (!s.TrimEnd().EndsWith(":"))
             //  throw new Exception("Failed to connect : no login prompt");
            //WriteLine(Username);

            //s += Read();
            //if (!s.TrimEnd().EndsWith(":"))
              //  throw new Exception("Failed to connect : no password prompt");
            //WriteLine(Password);

            //s += Read();
            _timeOutMs = oldTimeOutMs;
            return s;
        }

        public void WriteLine(string cmd)
        {
            Write(cmd + "\r\n");
        }

        public void Write(string cmd)
        {
            if (!_tcpSocket.Connected) return;
            byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(cmd.Replace("\0xFF","\0xFF\0xFF"));
            _tcpSocket.GetStream().Write(buf, 0, buf.Length);
        }

        public string Read()
        {
            if (!_tcpSocket.Connected) return null;
            StringBuilder sb=new StringBuilder();
            do
            {
                ParseTelnet(sb);
                System.Threading.Thread.Sleep(_timeOutMs);
            } while (_tcpSocket.Available > 0);
            return sb.ToString();
        }

        public bool IsConnected
        {
            get { return _tcpSocket.Connected; }
        }

        void ParseTelnet(StringBuilder sb)
        {
            while (_tcpSocket.Available > 0)
            {
                int input = _tcpSocket.GetStream().ReadByte();
                switch (input)
                {
                    case -1 :
                        break;
                    case (int)Verbs.Iac:
                        // interpret as command
                        int inputverb = _tcpSocket.GetStream().ReadByte();
                        if (inputverb == -1) break;
                        switch (inputverb)
                        {
                            case (int)Verbs.Iac: 
                                //literal IAC = 255 escaped, so append char 255 to string
                               // sb.Append(inputverb);
                                break;
                            case (int)Verbs.Do: 
                            case (int)Verbs.Dont:
                            case (int)Verbs.Will:
                            case (int)Verbs.Wont:
                                // reply to all commands with "WONT", unless it is SGA (suppres go ahead)
                                //int inputoption = tcpSocket.GetStream().ReadByte();
                                //if (inputoption == -1) break;
                                //tcpSocket.GetStream().WriteByte((byte)Verbs.IAC);
                                //if (inputoption == (int)Options.SGA )
                                //    tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WILL:(byte)Verbs.DO); 
                                //else
                                //    tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WONT : (byte)Verbs.DONT); 
                                //tcpSocket.GetStream().WriteByte((byte)inputoption);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        sb.Append( (char)input );
                        break;
                }
            }
        }
    }
}
