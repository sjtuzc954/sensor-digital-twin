using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SidebarPanelController : MonoBehaviour
{
    public GameObject LeftArrow;
    public GameObject RightArrow;

    private bool active = false;
    private float movespeed = 400f;
    private float origin;

    // Start is called before the first frame update
    void Start()
    {
        origin = this.GetComponent<RectTransform>().anchoredPosition.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active)
        {
            // 左侧边栏，向右运动
            if (LeftArrow.activeSelf)
            {
                if (this.GetComponent<RectTransform>().anchoredPosition.x < origin + this.GetComponent<RectTransform>().rect.width)
                {
                    this.GetComponent<RectTransform>().anchoredPosition += Vector2.right * movespeed * Time.deltaTime;
                }
            }
            // 右侧边栏，向左运动
            else if(RightArrow.activeSelf)
            {
                if (this.GetComponent<RectTransform>().anchoredPosition.x > origin - this.GetComponent<RectTransform>().rect.width)
                {
                 
                    this.GetComponent<RectTransform>().anchoredPosition += Vector2.left * movespeed * Time.deltaTime;
                }
            }
        }
        else
        {
            // 右侧边栏，向右运动
            if (LeftArrow.activeSelf)
            {
                if (this.GetComponent<RectTransform>().anchoredPosition.x < origin)
                {
                    this.GetComponent<RectTransform>().anchoredPosition += Vector2.right * movespeed * Time.deltaTime;
                }
            }
            // 左侧边栏，向左运动
            else if (RightArrow.activeSelf)
            {
                if (this.GetComponent<RectTransform>().anchoredPosition.x > origin)
                {
                    this.GetComponent<RectTransform>().anchoredPosition += Vector2.left * movespeed * Time.deltaTime;
                }
            }
        }
    }

    public void PanelMove()
    {
        active = !active;
        if (LeftArrow.activeSelf)
        {
            LeftArrow.SetActive(false);
            RightArrow.SetActive(true);
        }
        else if (RightArrow.activeSelf)
        {
            LeftArrow.SetActive(true);
            RightArrow.SetActive(false);
        }
    }
}
