using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace EcnoServe
{
    class Program
    {
        //监听sockt 
        static Socket listenfd;
        //客户端Socket 以及状态信息
        static Dictionary<Socket, ClientState> clinets = new Dictionary<Socket, ClientState>();
        /*//accept回调 */
        public static void AcceptCallBack(IAsyncResult ar)
        {
            try
            {
                Console.WriteLine("[服务器]Accept");
                Socket listenfd = (Socket)ar.AsyncState;
                Socket clienfd = listenfd.EndAccept(ar);
                //clinet 列表
                ClientState state = new ClientState();
                state.socket = clienfd;
                clinets.Add(clienfd, state);
                //接收数据beginnReceive
                clienfd.BeginReceive(state.readBuff, 0, 1024, 0, ReceiveCallBack,state);
                //继续accept
                listenfd.BeginAccept(AcceptCallBack, listenfd);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("socket accept fail " + ex.ToString());

            }
        }
        //receve回调
        public static void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                ClientState state = (ClientState)ar.AsyncState;
                Socket clientfd = state.socket;
                int count = clientfd.EndReceive(ar);
                if(count == 0)//客户端关闭
                {
                    clientfd.Close();
                    clinets.Remove(clientfd);
                    Console.WriteLine("Socket close");
                    return;
                }
                string recvStr = System.Text.Encoding.Default.GetString(state.readBuff, 0, count);
                byte[] sendtype = System.Text.Encoding.Default.GetBytes("echo:"+recvStr);
                clientfd.Send(sendtype);//减少代码量，不用异步
                clientfd.BeginReceive(state.readBuff, 0, 1024, 0, ReceiveCallBack, state);

            }
            catch (SocketException ex)
            {
                Console.WriteLine("socket receive fail " + ex.ToString());

            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Helle");
            //scoket
            listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //bind
            IPAddress ipadr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEp = new IPEndPoint(ipadr, 8080);
            listenfd.Bind(ipEp);//绑定端口
            //listen
            listenfd.Listen(0);
            Console.WriteLine("[服务器]启动成功");
            //异步处理客户端端请求
            //accept
            listenfd.BeginAccept(AcceptCallBack, listenfd);
            //等待
            Console.ReadLine();
            /*/
            while (true)
            {
                //accep
                Socket connfd = listenfd.Accept();
                Console.WriteLine("[服务器]Accept");
                //recv
                Byte[] readbuf = new byte[1024];
                int count = connfd.Receive(readbuf);
                string readStr = System.Text.Encoding.Default.GetString(readbuf);
                Console.WriteLine("[服务器接收]" + readStr);
                //send
                byte[] sendStr = System.Text.Encoding.Default.GetBytes(readStr);
                connfd.Send(sendStr);
            }*/
        }
    }
    class ClientState
    {
        public Socket socket;//保存客户端的socket;
        public byte[] readBuff = new byte[1024];
    }
}
