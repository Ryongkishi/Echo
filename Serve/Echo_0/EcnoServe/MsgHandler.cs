using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class MsgHandler
{
    public static void MsgEnter(ClientState c,string msgArgs)
    {
        Console.WriteLine("[服务端消息处理]MsgEnter回调"+msgArgs);
    }
    public static void MsgList(ClientState c, string msgArgs)
    {
        Console.WriteLine("[服务端消息处理]MsgList" + msgArgs);
    }
}