using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static bool isStart;
    public static bool isInfo;
    public static bool isCameraFree;
    public static bool isCameraInfo;
    public static bool isNetWorkConnecting;
    public static GameObject rackManager;
    public static GameObject infoObject;
    public static Dictionary<string, string> MsgDic;
    public static Texture2D InfoTex;
    public static Camera currentCamera;

    public GameObject CameraBtn;
    public GameObject InfoPanel;
    public GameObject SocketPanel;
    public GameObject InfoImage;
    public GameObject ManualPanel;
    public GameObject ObjectPrefab;
    public GameObject CameraPanel;
    public GameObject AlertPanel;
    public GameObject SmartPlugPanel;

    private GameObject infoImage;

    // Start is called before the first frame update
    void Start()
    {
        isStart = false;
        isInfo = false;
        isCameraFree = true;
        isCameraInfo = false;
        isNetWorkConnecting = false;
        MsgDic = new Dictionary<string, string>();
        currentCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // ������������չʾ����
        ShowDataInfo();
        // չʾ�����ݲ㴫���������Ϣ
        ShowNetWorkInfo();
    }

    void ShowDataInfo()
    {
        if (isInfo)
        {
            SetInfo(rackManager, infoObject);
            if (isCameraInfo)
            {
                InfoAddPicture(InfoTex);
            }
        }
        // ���ڷ�����豸չʾʱ��Image�������
        if (!isCameraInfo)
        {
            InfoDestroyPicture();
        }
    }

    void ShowNetWorkInfo()
    {
        if (isNetWorkConnecting)
        {
            try
            {
                string res = "";
                //�Լ�ֵ�Խ��б���
                foreach (KeyValuePair<string, string> kv in MsgDic)
                {
                    res += kv.Key + ": " + kv.Value + "\n";
                }
                SocketPanel.GetComponentInChildren<Text>().text = res;
            }
            catch (Exception ex)
            {

            }
        }
        else
        {
            SocketPanel.GetComponentInChildren<Text>().text = "";
        }
    }

    public void PipelineStart()
    {
        isStart = true;
    }

    public void PipelineStop()
    {
        isStart = false;
    }

    public void PipelineRestart()
    {
        isStart = false;
        SceneManager.LoadSceneAsync("Pipeline");
    }

    public void Generate()
    {
         GameObject.Instantiate(ObjectPrefab);
    }

    public void Exit()
    {
         Application.Quit();
    }
    public void CameraBtnPress()
    {
        CameraBtn.GetComponentInChildren<Text>().text = (CameraBtn.GetComponentInChildren<Text>().text == "�̶����") ? "�������" : "�̶����";
        if (CameraBtn.GetComponentInChildren<Text>().text == "�������")
        {
            Camera.main.depth = -10;
            currentCamera = ManualPanel.GetComponent<ManualPanelPageController>().currentCamera;
            isCameraFree = false;
        }
        else
        {
            Camera.main.depth = -1;
            currentCamera = Camera.main;
            isCameraFree = true;
        }
    }

    public void CallInfoPanel()
    {
        InfoPanel.GetComponent<SidebarPanelController>().PanelMove();
    }

    public void SetInfo(GameObject rackManager, GameObject infoObject)
    {
        InfoPanel.GetComponentInChildren<Text>().text = rackManager.GetComponent<RackManager>().GetInfo(infoObject);
    }

    public void InfoAddPicture(Texture2D tex)
    {
        if (infoImage == null)
        {
            infoImage = Instantiate(InfoImage, InfoPanel.transform.Find("ViewPort"));
        }
        infoImage.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        // �����ǽ��һ��bug����һ��InfoObject��Text����С�ڸ�InfoObject�����ݳ���ʱ��ͼƬ��λ�ûḲ��һ����Text������ԭ���������д�����Խ����
        InfoPanel.GetComponentInChildren<Text>().text += " ";
    }

    public void InfoDestroyPicture()
    {
        if (infoImage != null)
        {
            Destroy(infoImage);
            infoImage = null;
        }
    }

    public void CallAlertPanel(GameObject obj)
    {
        AlertPanel.GetComponent<AlertPanelController>().alertSensor = obj;
        AlertPanel.GetComponent<AlertPanelController>().SetVisible(true);
    }

    public void CallCameraPanel()
    {
        CameraPanel.GetComponent<CameraPanelController>().SetVisible(true);
    }

    public void CallSmartPlugPanel()
    {
        SmartPlugPanel.GetComponent<SmartPlugController>().SetVisible(true);
    }
}
