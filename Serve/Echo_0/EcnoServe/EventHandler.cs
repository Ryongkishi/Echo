using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class EventHandler
{
    public static void Ondisconnect(ClientState c)
    {
        Console.WriteLine("[服务端事件处理]Ondisconnect回调");
    }
}
