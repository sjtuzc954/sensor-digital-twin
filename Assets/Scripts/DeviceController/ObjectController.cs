using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
public Vector3[] positions;
    public bool isBlock = false;
private bool OK = false;
private bool destroy = false;
private float timer = 0;
private float delay = 5;
static int cnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!OK && destroy) {
		timer += Time.deltaTime;
		if (timer >= delay) {
// Debug.Log("1");
			// Destroy(this.gameObject);
if (cnt == 5) {
OK = true; return;}
this.gameObject.transform.position = positions[cnt];
cnt++;

this.gameObject.layer = LayerMask.NameToLayer("ignore");
destroy = false;
OK = true;
		}
	}
    }

private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Board")
        {
            destroy =  true;
        }
    }
}
