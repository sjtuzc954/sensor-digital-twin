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
        // 控制数字孪生展示界面
        ShowDataInfo();
        // 展示由数据层传输过来的信息
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
        // 用于非相机设备展示时的Image组件销毁
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
                //对键值对进行遍历
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
        CameraBtn.GetComponentInChildren<Text>().text = (CameraBtn.GetComponentInChildren<Text>().text == "固定相机") ? "自由相机" : "固定相机";
        if (CameraBtn.GetComponentInChildren<Text>().text == "自由相机")
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
        // 这行是解决一个bug：上一个InfoObject里Text内容小于该InfoObject里内容长度时，图片的位置会覆盖一部分Text。具体原因不明，这行代码可以解决。
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
}
