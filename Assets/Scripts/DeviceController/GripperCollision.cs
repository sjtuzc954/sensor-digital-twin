using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripperCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Object" || collision.gameObject.tag == "Vehicle")
        {
            if (this.name == "LeftGripper")
            {
                this.GetComponentInParent<RobotArmController>().SetIsReallyGrabingLeft();
                this.GetComponentInParent<RobotArmController>().SetGrabedObject(collision.gameObject);
            }
            if (this.name == "RightGripper")
            {
                this.GetComponentInParent<RobotArmController>().SetIsReallyGrabingRight();
                this.GetComponentInParent<RobotArmController>().SetGrabedObject(collision.gameObject);
            }
        }
    }
}
