using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace SelectServe
{
    class SelectServe
    {
        //监听sockt 
        static Socket listenfd;
        //客户端Socket 以及状态信息
        static Dictionary<Socket, ClientState> clinets = new Dictionary<Socket, ClientState>();
        static void SeclectServe()
        {
            Console.WriteLine("[服务器]SeclectServe");
            //scoket
            listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //bind
            IPAddress ipadr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEp = new IPEndPoint(ipadr, 8080);
            listenfd.Bind(ipEp);//绑定端口
            //listen
            listenfd.Listen(0);
            Console.WriteLine("[服务器]启动成功");
            //通过select处理客户端请求
            //checkRead
            List<Socket> checkRead = new List<Socket>();
            while(true)
            {
                //填充checkRead列表
                checkRead.Clear();
                checkRead.Add(listenfd);
                foreach(ClientState s in clinets.Values)
                {
                    checkRead.Add(s.socket);
                }
                //select
                Socket.Select(checkRead, null, null, 1000);
                //检查可读对象
                foreach(Socket s in checkRead)
                {
                    if(s == listenfd)
                    {
                        ReadLstenfd(s);
                    }
                    else
                    {
                        ReadClinetfd(s);
                    }
                }
            }
        }
        //读取Listenfd
        public static void ReadLstenfd(Socket listenfd)
        {
            Console.WriteLine("accept");
            Socket clientfd = listenfd.Accept();
            ClientState state = new ClientState();
            state.socket = clientfd;
            clinets.Add(clientfd, state);
        }
        //读取Clinentfd
        public static bool ReadClinetfd(Socket clientfd)
        {
            ClientState state = clinets[clientfd];
            //接收
            int count = 0;
            try
            {
                count = clientfd.Receive(state.readBuff);
            }
            catch (SocketException ex)
            {
                clientfd.Close();
                clinets.Remove(clientfd);
                Console.WriteLine("receive SocketException" + ex.ToString());
                return false;
            }
            //客户端关闭
            if (count == 0)
            {
                clientfd.Close();
                clinets.Remove(clientfd);
                Console.WriteLine("Socket close");
                return false;
            }
            //广播
            string recvStr = System.Text.Encoding.Default.GetString(state.readBuff, 0, count);
            Console.WriteLine("receive" + recvStr);
            string sendStr = clientfd.RemoteEndPoint.ToString() + ":" + recvStr;
            byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
            foreach (ClientState s in clinets.Values)
            {
                s.socket.Send(sendBytes);
            }
            return true;
        }
    }
    class ClientState
    {
        public Socket socket;//保存客户端的socket;
        public byte[] readBuff = new byte[1024];
    }
}
