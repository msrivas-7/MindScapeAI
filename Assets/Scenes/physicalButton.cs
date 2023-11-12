
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class physicalButton : MonoBehaviour
{
    /// <summary>
    /// Add this script as a child of the button UI
    /// 
    /// This script will call the onclick() function on the button UI.
    /// 
    /// README
    /// 
    /// Create a tag named "index finger" and "Button Bottom"
    /// 
    /// </summary>
    /// 
    bool indexIn = false;
    Transform indexFinger; // Index finger of the quest controller, set it's index finger collider to trigger

    public float MaxRange = 1; // this is the max range where the button will be on its idle position
    public float MinRange = .75f; // how far the button can be pressed down
    float range = 0;

    private void Start()
    {
        indexIn = false;
    }

    private void Update()
    {
        if (indexIn)
        {
            {
                transform.position = new Vector3(transform.position.x, indexFinger.position.y, transform.position.z);
                range = Mathf.Clamp(transform.localPosition.y, MinRange, MaxRange);
                transform.localPosition = new Vector3(transform.localPosition.x, range, transform.localPosition.z);
                Debug.Log("index in");
            }
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, MaxRange, transform.localPosition.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Button Bottom")
        {
            Debug.Log("Button Pressed");
             gameObject.transform.parent.GetComponentInParent<Button>().onClick.Invoke(); // get the button component from the parent of the parent
            // clicks the UI button parent
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "index finger")
        {
            indexIn = true;
            indexFinger = other.transform; // gets the transform of the index finger that makes contact with the button
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "index finger")
        {
            indexIn = false;
        }
    }
  
}
