using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPressed;
    public UnityEvent onRelease;
    GameObject presser;
    bool isPressed;



    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed){
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            presser = other.gameObject;
            onPressed.Invoke();
            isPressed = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            onRelease.Invoke();
            isPressed = false;


        }
    }

}
