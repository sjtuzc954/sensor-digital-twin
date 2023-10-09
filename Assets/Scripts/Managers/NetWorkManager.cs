using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;
using UnityEngine.UI;
using MQTTnet;
using System.Threading.Tasks;
using MQTTnet.Client;

public class NetWorkManager : MonoBehaviour
{
    Socket serverSocket; //服务器端socket
    Socket clientSocket; //客户端socket
    IPAddress ip; //主机ip
    IPEndPoint ipEnd;
    public static string recvStr; //接收的字符串
    byte[] sendData;
    byte[] recvData = new byte[10240]; //接收的数据，必须为字节
    int recvLen; //接收的数据长度
    Thread connectThread; //连接线程
    bool isMqttEstablished;
    IMqttClient mqttClient;

    //初始化
    void Start()
    {
        //定义服务器的IP和端口，端口与服务器对应
	    // ipEnd = new IPEndPoint(IPAddress.Parse("192.168.3.20"), 15000);
        ipEnd = new IPEndPoint(IPAddress.Any, 9999); //服务器端口号
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    void Update()
    {
        if (GameManager.isStart)
        {
            if (connectThread == null)
            {
                serverSocket.Bind(ipEnd);
                serverSocket.Listen(1);
                //开启一个线程连接，必须的，否则主线程卡死
                connectThread = new Thread(new ThreadStart(SocketReceive));
                connectThread.Start();
            }
            if (!isMqttEstablished)
            {
                isMqttEstablished = true;
                EstablishMqtt();
            }
        }
        else
        {
            if (connectThread != null)
            {
                SocketQuit();
            }
        }
    }

    void SocketConnect()
    {
        if (clientSocket != null)
            clientSocket.Close();
        Debug.Log("等待客户端连接...");
        //定义套接字类型,必须在子线程中定义
        clientSocket = serverSocket.Accept();
        //获取客户端的IP和端口
        IPEndPoint ipEndClient = (IPEndPoint)clientSocket.RemoteEndPoint;
        //输出客户端的IP和端口
        print("Connect with " + ipEndClient.Address.ToString() + ":" + ipEndClient.Port.ToString());
    }

    async void EstablishMqtt()
    {
        Debug.Log("连接mqtt broker...");
        var mqttFactory = new MqttFactory();
        mqttClient = mqttFactory.CreateMqttClient();
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer("60.204.201.196", 1883)
            .Build();

        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            var value = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.ToArray());
            var key = e.ApplicationMessage.Topic.Split('/')[1];
            if (value.StartsWith('!'))
            {
                key += "_Alert";
                value = value[1..];
            }
            if (key.Equals("Temperature"))
            {
                var v = double.Parse(value) * 0.1;
                value = v.ToString() + "摄氏度";
            }
            else if (key.Equals("Humidity"))
            {
                var v = double.Parse(value) * 0.1;
                value = v.ToString() + "%";
            }
            Debug.Log("Mqtt Client Received: " + key + ":" + value);
            if (GameManager.MsgDic.ContainsKey(key))
            {
                GameManager.MsgDic[key] = value;
            }
            else
            {
                GameManager.MsgDic.Add(key, value);
            }
            return Task.CompletedTask;
        };

        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(
                f =>
                {
                    f.WithTopic("Sensor/#").WithExactlyOnceQoS();
                })
            .Build();

        await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
        Debug.Log("Mqtt客户端连接成功");
    }

    void SocketSend(string msg)
    {
        //清空发送缓存
        sendData = new byte[1024];
        //数据类型转换
        sendData = Encoding.UTF8.GetBytes(msg);

        //发送
        clientSocket.Send(sendData, sendData.Length, SocketFlags.None);
    }

    void SocketReceive()
    {
        SocketConnect();

        if (clientSocket == null || !clientSocket.Connected)
        {
            Debug.Log("连接失败");
            return;
        }
        GameManager.isNetWorkConnecting = true;
        
        //不断接收服务器发来的数据
        while (true)
        {
            try
            {
                Array.Clear(recvData, 0, recvData.Length);
                recvLen = clientSocket.Receive(recvData);
                if (recvLen <= 0 || !clientSocket.Connected)
                {
                    SocketConnect();
                    continue;
                }
                //Tcp 接受服务器数据，需要自己组装
                recvStr = Encoding.UTF8.GetString(recvData);
                ResolveData(recvStr);
            }
            catch (Exception ex)
            {
                continue;
            }
        }
    }

    void ResolveData(string data)
    {
        string[] strList = data.Split(';');
        string key, value;
        for(int i = 0; i < strList.Length; ++i)
        {
            key = strList[i].Split(':')[0];
            value = strList[i].Split(':')[1];
            if (GameManager.MsgDic.ContainsKey(key))
            {
                GameManager.MsgDic[key] = value;
            }
            else
            {
                GameManager.MsgDic.Add(key, value);
            }
        }
    }

    // 断开socket连接，并清除线程。在暂停、重置以及编辑器退出时调用
    void SocketQuit()
    {
        //先关闭客户端
        if (clientSocket != null)
            clientSocket.Close();
        //再关闭线程
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //最后关闭服务器
        serverSocket.Close();
        mqttClient.DisconnectAsync();
    }

    void OnApplicationQuit()
    {
        SocketQuit();
    }
}
