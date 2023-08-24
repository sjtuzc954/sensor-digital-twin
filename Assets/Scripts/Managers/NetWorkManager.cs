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
    Socket serverSocket; //��������socket
    Socket clientSocket; //�ͻ���socket
    IPAddress ip; //����ip
    IPEndPoint ipEnd;
    public static string recvStr; //���յ��ַ���
    byte[] sendData;
    byte[] recvData = new byte[10240]; //���յ����ݣ�����Ϊ�ֽ�
    int recvLen; //���յ����ݳ���
    Thread connectThread; //�����߳�
    bool isMqttEstablished;

    //��ʼ��
    void Start()
    {
        //�����������IP�Ͷ˿ڣ��˿����������Ӧ
        ipEnd = new IPEndPoint(IPAddress.Any, 9999); //�������˿ں�
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
                //����һ���߳����ӣ�����ģ��������߳̿���
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

    async void EstablishMqtt()
    {
        Debug.Log("����mqtt broker...");
        var mqttFactory = new MqttFactory();
        using (var mqttClient = mqttFactory.CreateMqttClient())
        {
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("60.204.201.196", 1883)
                .Build();

            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                var value = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                var key = e.ApplicationMessage.Topic.Split('/')[1];
                if (value.StartsWith('!'))
                {
                    key += "_Alert";
                    value = value[1..];
                }
                if (key.Equals("Temperature"))
                {
                    var v = double.Parse(value) * 0.1;
                    value = v.ToString() + "���϶�";
                }
                else if (key.Equals("Humidity"))
                {
                    var v = double.Parse(value) * 0.1;
                    value = v.ToString() + "%";
                }
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
            Debug.Log("Mqtt�ͻ������ӳɹ�");
        }

    }

    void SocketConnect()
    {
        if (clientSocket != null)
            clientSocket.Close();
        Debug.Log("�ȴ��ͻ�������...");
        //�����׽�������,���������߳��ж���
        clientSocket = serverSocket.Accept();
        //��ȡ�ͻ��˵�IP�Ͷ˿�
        IPEndPoint ipEndClient = (IPEndPoint)clientSocket.RemoteEndPoint;
        //����ͻ��˵�IP�Ͷ˿�
        print("Connect with " + ipEndClient.Address.ToString() + ":" + ipEndClient.Port.ToString());
    }

    void SocketSend(string msg)
    {
        //��շ��ͻ���
        sendData = new byte[1024];
        //��������ת��
        sendData = Encoding.UTF8.GetBytes(msg);

        //����
        clientSocket.Send(sendData, sendData.Length, SocketFlags.None);
    }

    void SocketReceive()
    {
        SocketConnect();

        if (clientSocket == null || !clientSocket.Connected)
        {
            Debug.Log("����ʧ��");
            return;
        }
        GameManager.isNetWorkConnecting = true;
        
        //���Ͻ��շ���������������
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
                //Tcp ���ܷ��������ݣ���Ҫ�Լ���װ
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

    // �Ͽ�socket���ӣ�������̡߳�����ͣ�������Լ��༭���˳�ʱ����
    void SocketQuit()
    {
        //�ȹرտͻ���
        if (clientSocket != null)
            clientSocket.Close();
        //�ٹر��߳�
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //���رշ�����
        serverSocket.Close();
    }

    void OnApplicationQuit()
    {
        SocketQuit();
    }
}
