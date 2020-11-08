using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;
using System;
public class Echo : MonoBehaviour
{
    //定义套接字
    Socket socket;
    public InputField inputFeld;
    public Text text;
    //接收缓冲区
    byte[] readbuf = new byte[1024];
    string recvStr = "";
    // Start is called before the first frame update
    /*
    点击连接按钮
    */
    public void Connecton()
    {
        //socket
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //阻塞connect
        //socket.Connect("127.0.0.1", 8080);
        //采用异步connect
        socket.BeginConnect("127.0.0.1", 8080, ConnectCallBack, socket);

    }
    //connect 回调
    public void ConnectCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            //
            socket.EndConnect(ar);
            Debug.Log("Socket success");
            socket.BeginReceive(readbuf, 0, 1024, 0, ReceiveCallback, socket);

        }
        catch (SocketException ex)
        {
            Debug.Log("Socket connect fail " + ex.ToString());
        }
    }
    //receve 回调
    public void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndReceive(ar);
            recvStr = System.Text.Encoding.Default.GetString(readbuf, 0, count);
            socket.BeginReceive(readbuf, 0, 1024, 0, ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket receive fail " + ex.ToString());
        }
    }
    /*
点击发送按钮
*/
    public void Send()
    {
        //send
        string sendstr = inputFeld.text;
        byte[] sendbyte = System.Text.Encoding.Default.GetBytes(sendstr);
        socket.BeginSend(sendbyte,0,sendbyte.Length,0,SendCallback,socket);

        //socket.Send(sendbyte);
        //Recv
        //byte[] readbuff = new byte[1024];
        //rece是一个阻塞方法
        // int count = socket.Receive(readbuff);
        // string recvStr = System.Text.Encoding.Default.GetString(readbuff, 0, count);
        // text.text = recvStr;
        // socket.Close();
    }
    //send 回调
    public void SendCallback(IAsyncResult ar){
        try{
            Socket socket = (Socket) ar.AsyncState;
            int count = socket.EndSend(ar);
            Debug.Log("Socket send success"+count);
        }
                catch (SocketException ex)
        {
            Debug.Log("Socket send fail " + ex.ToString());
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        text.text = recvStr;
    }
}