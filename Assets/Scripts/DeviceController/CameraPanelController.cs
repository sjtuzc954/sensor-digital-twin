using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CameraPanelController : MonoBehaviour
{
    public Button closeBtn;
    public GameObject camaraPanel;
    public Image image;

    private readonly string url = "http://localhost:8080/images/1.jpg";

    // Start is called before the first frame update
    void Start()
    {
        SetVisible(false);
        closeBtn.onClick.AddListener(() => SetVisible(false));
        StartCoroutine(DownSprite());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVisible(bool visible)
    {
        camaraPanel.GetComponent<CanvasGroup>().alpha = visible ? 1 : 0;
        camaraPanel.GetComponent<CanvasGroup>().interactable = visible;
        camaraPanel.GetComponent<CanvasGroup>().blocksRaycasts = visible;
    }

    IEnumerator DownSprite()
    {
        while (true)
        {
            UnityWebRequest request = new UnityWebRequest(url);
            DownloadHandlerTexture handler = new DownloadHandlerTexture(true);
            request.downloadHandler = handler;
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.ConnectionError)
            {
                Texture2D texture = handler.texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                image.sprite = sprite;
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
