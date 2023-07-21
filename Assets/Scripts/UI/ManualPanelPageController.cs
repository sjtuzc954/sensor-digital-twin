using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RackPage
{
    public int initpage;
    public int maxpage;
}

public class ManualPanelPageController : MonoBehaviour
{
    public GameObject NextPage;
    public GameObject LastPage;
    public List<RackPage> RackPages;
    public int initListnum = 0;

    private int page;
    private int listnum;
    private GameObject currentPage;
    public Camera currentCamera;

    // Start is called before the first frame update
    void Start()
    {
        page = RackPages[initListnum].initpage;
        listnum = initListnum;
        currentPage = this.transform.Find("Rank" + (listnum + 1) + "_Pages/" + "Page" + page).gameObject;
        currentCamera = GameObject.Find("Rank" + (listnum + 1) + "_Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        LastPage.SetActive(true);
        NextPage.SetActive(true);
        if (page == RackPages[listnum].initpage) LastPage.SetActive(false);
        if (page == RackPages[listnum].maxpage) NextPage.SetActive(false);
    }

    public void NextPageOnClick()
    {
        page++;
        currentPage.SetActive(false);
        currentPage = this.transform.Find("Rank" + (listnum + 1) + "_Pages/" + "Page" + page).gameObject;
        currentPage.SetActive(true);
    }

    public void LastPageOnClick()
    {
        page--;
        currentPage.SetActive(false);
        currentPage = this.transform.Find("Rank" + (listnum + 1) + "_Pages/" + "Page" + page).gameObject;
        currentPage.SetActive(true);
    }

    public void ChangeRank()
    {
        currentPage.SetActive(false);
        this.transform.Find("Rank" + (listnum + 1) + "_Pages").gameObject.SetActive(false);
        listnum = (listnum + 1) % RackPages.Count;
        page = RackPages[listnum].initpage;
        this.transform.Find("Rank" + (listnum + 1) + "_Pages").gameObject.SetActive(true);
        currentPage = this.transform.Find("Rank" + (listnum + 1) + "_Pages/" + "Page" + page).gameObject;
        currentPage.SetActive(true);
        currentCamera.depth = -10;
        currentCamera = GameObject.Find("Rank" + (listnum + 1) + "_Camera").GetComponent<Camera>();
        GameManager.currentCamera = currentCamera;
        currentCamera.depth = -2;
    }
}
