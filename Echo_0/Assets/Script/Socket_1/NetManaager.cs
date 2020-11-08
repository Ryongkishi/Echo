using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using UnityEngine.UI;
public class NetManaager
{
    //定义套接字
     static Socket socket;
    //接受缓冲区
    static byte[] readBuff = new byte[1024];
    //委托类型
    public delegate void MsgListener(string str);
    //监听列表 采用字典进行存储
    private static Dictionary<string ,MsgListener > listeners = new Dictionary<string, MsgListener>();
    //消息列表
    public static List<string> msgList = new List<string>();
    //添加监听
    public static void AddListener(string msgName,MsgListener listener){
        listeners[msgName] = listener;
    }
    //获取描述 本机ip+端口号
    public static string GetDesc(){
        if(socket == null)return "";
        if(!socket.Connected) return "";
        Debug.Log("本机ip:"+socket.LocalEndPoint.ToString());
        return socket.LocalEndPoint.ToString();

    }
    //连接
    public static void Connect(string ip,int port){
        //Socket
        socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        //connect 同步方式简化代码
        socket.Connect(ip,port);
        //beginReceive
        socket.BeginReceive(readBuff,0,1024,0,receveCallBack,socket);
    }
    //receive回调
    public static void receveCallBack(IAsyncResult ar){
        try{
            Socket socket = (Socket)ar.AsyncState;
            int count  = socket.EndReceive(ar);
            string recvStr = System.Text.Encoding.Default.GetString(readBuff,0,count);
            msgList.Add(recvStr);
            Debug.Log("[客户端信息]recvStr:"+recvStr);
            socket.BeginReceive(readBuff,0,1024,0,receveCallBack,socket);
        }
        catch(SocketException ex){
            Debug.Log("Socket Receive fail"+ex.ToString());
        }
    }
    //发送
    public static void Send(string str){
        if(socket == null) return;
        if(!socket.Connected) return;
        byte[] sendBytes = System.Text.Encoding.Default.GetBytes(str);
        socket.Send(sendBytes);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public static void Update()
    {
        if(msgList.Count<=0){
            return;
        }
        string msgStr = msgList[0];
        msgList.RemoveAt(0);
        string[] split = msgStr.Split('|');
        string msgName = split[0];
        string msgArgs = split[1];
        Debug.Log("[客户端]监听列表："+msgName+"  "+msgArgs);
        //监听回调
        if(listeners.ContainsKey(msgName)){
            Debug.Log("[客户端]监听列表(列表查询)："+msgName);
            listeners[msgName](msgArgs);
        }
    }
}
