using MQTTnet;
using MQTTnet.Client;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SmartPlugController : MonoBehaviour
{
    public GameObject smartPlugPanel;

    public GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        SetVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        var s = "";
        if (!GameManager.MsgDic.ContainsKey("SmartPlug"))
        {
            s = "δ���ӵ����ܲ���";
        }
        else
        {
            var status = GameManager.MsgDic["SmartPlug"];
            if (status.Equals("0"))
            {
                s = "���ܲ�����ǰ״̬���ر�";
            }
            if (status.Equals("1"))
            {
                s = "���ܲ�����ǰ״̬������";
            }
        }
        text.GetComponent<Text>().text = s;
    }

    /*
    public async void Switch()
    {
        if (!GameManager.MsgDic.ContainsKey("SmartPlug"))
        {
            return;
        }
        var status = GameManager.MsgDic["SmartPlug"];
        var msg = status.Equals("0") ? "1" : "0";
        await SendSwitchMsg(msg);
    }
    */

    public async void SendSwitchMsg(string msg)
    {
        Debug.Log("1");
        var mqttFactory = new MqttFactory();

        using (var mqttClient = mqttFactory.CreateMqttClient())
        {
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("60.204.201.196", 1883)
                .Build();
            
            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic("Sensor/SmartPlugControl")
                .WithPayload(msg)
                .Build();

            Debug.Log("Publishing: " + msg);
            var result = await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            Debug.Log("IsSuccess: " + result.IsSuccess);

            await mqttClient.DisconnectAsync();
        }
    }

    public void SetVisible(bool visible)
    {
        smartPlugPanel.GetComponent<CanvasGroup>().alpha = visible ? 1 : 0;
        smartPlugPanel.GetComponent<CanvasGroup>().interactable = visible;
        smartPlugPanel.GetComponent<CanvasGroup>().blocksRaycasts = visible;
    }
}
