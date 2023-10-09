using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertPanelController : MonoBehaviour
{
    public GameObject alertPanel;

    public GameObject alertSensor;

    // Start is called before the first frame update
    void Start()
    {
        SetVisible(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetVisible(bool visible)
    {
        alertPanel.GetComponent<CanvasGroup>().alpha = visible ? 1 : 0;
        alertPanel.GetComponent<CanvasGroup>().interactable = visible;
        alertPanel.GetComponent<CanvasGroup>().blocksRaycasts = visible;
    }

    public void ResetAlert()
    {
        if (alertSensor == null)
        {
            return;
        }
        alertSensor.GetComponent<SensorLogger>().ResetAlert();
        SetVisible(false);
    }

    public void Cancel()
    {
        alertSensor = null;
        SetVisible(false);
    }
}
