using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHighlight : MonoBehaviour
{
    public GameObject gameManager;

    private GameObject highlightcheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int mouseButton = -1;
        if (Input.GetMouseButtonDown(0))
        {
            mouseButton = 0;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            mouseButton = 1;
        }
        if (mouseButton > -1)
        {
            var ray = GameManager.currentCamera.ScreenPointToRay(Input.mousePosition);//������Ļ����ת��Ϊһ������
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f, 1 << (LayerMask.NameToLayer("Highlightable"))))
            {
                if (!IsTouchedUI())
                {
                    var hitObj = hit.collider.gameObject;

                    while (hitObj.transform.parent != null)
                    {
                        hitObj = hitObj.transform.parent.gameObject;
                    }
                    SetObjectHighlight(hitObj, mouseButton);
                }
            }
        }
    }

    bool IsTouchedUI()
    {
        bool touchedUI = false;
        if (Application.isMobilePlatform)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                touchedUI = true;
            }
        }
        else if (EventSystem.current.IsPointerOverGameObject())
        {
            touchedUI = true;
        }
        return touchedUI;
    }

    public void SetObjectHighlight(GameObject obj, int mouseButton)
    {
        if (mouseButton == 0)
        {
            if (obj.name.Equals("USBCamera"))
            {
                gameManager.GetComponent<GameManager>().CallCameraPanel();
            }
            else if (obj.name.Equals("SmartPlug"))
            {
                gameManager.GetComponent<GameManager>().CallSmartPlugPanel();
            }
            else if (highlightcheck == null)
            {
                AddComponent(obj);
                gameManager.GetComponent<GameManager>().CallInfoPanel();
            }
            else if (highlightcheck == obj)
            {
                RemoveComponent(obj);
                gameManager.GetComponent<GameManager>().CallInfoPanel();
            }
            else
            {
                RemoveComponent(highlightcheck);
                AddComponent(obj);
            }
        }
        else if (mouseButton == 1)
        {
            gameManager.GetComponent<GameManager>().CallAlertPanel(obj);
        }
    }

    public void AddComponent(GameObject obj)
    {
        if (obj.GetComponent<HighlightableObject>() == null)
        {
            obj.AddComponent<HighlightableObject>();
        }
        obj.GetComponent<HighlightableObject>().ConstantOn(Color.red);
        GameManager.isInfo = true;
        GameManager.rackManager = obj.GetComponent<DataInfoObject>().RackManager;
        GameManager.infoObject = obj;
        highlightcheck = obj;
    }

    public void RemoveComponent(GameObject obj)
    {
        if (obj.GetComponent<HighlightableObject>() != null)
        {
            Destroy(obj.GetComponent<HighlightableObject>());
        }
        GameManager.isInfo = false;
        GameManager.rackManager = null;
        GameManager.infoObject = null;
        GameManager.isCameraInfo = false;
        obj.GetComponent<HighlightableObject>().ConstantOff();
        highlightcheck = null;
    }
}