using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorLogger : MonoBehaviour
{
    // Start is called before the first frame update
    public string infostr;

    public string alertstr = "";

    public bool isAlert = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateInfo(string msg)
    {
        // print(msg);
        infostr = msg;
    }
    public void UpdateAlert(string msg)
    {
        // print(msg);
        isAlert = true;
        alertstr += "\n" + System.DateTime.Now.ToString("G") + " " + msg;
    }
    public void ResetAlert()
    {
        isAlert = false;
        alertstr = "";
    }
}
