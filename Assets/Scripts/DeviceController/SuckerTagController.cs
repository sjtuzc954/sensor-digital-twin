using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckerTagController : MonoBehaviour
{
    public GameObject Sucker;
    public GameObject Tags;
    public GameObject TagPrefab;
    public Transform InitiatePos;
    private GameObject Tag;
    private int state;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Tag")
        {
            state = Sucker.GetComponent<XYZSuckerController>().CheckState();
            if (state == 1)
            {
                Tag = collider.gameObject;
                Tag.gameObject.transform.parent = this.gameObject.transform;
            }
        }
        if (collider.gameObject.tag == "Object")
        {
            state = Sucker.GetComponent<XYZSuckerController>().CheckState();
            if (state == 3)
            {
                if (Tag != null)
                {
                    Tag.gameObject.transform.parent = collider.gameObject.transform;
                    Tag = null;
                    Instantiate(TagPrefab, InitiatePos.transform.position, new Quaternion(0, 0, 0, 0));
                }
            }
        }
    }
}
