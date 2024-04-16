using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Knife : MonoBehaviour
{
    public GameObject controllerVisual;
    private bool isGrabbed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGrab()
    {
        controllerVisual.SetActive(false);
        isGrabbed = true;
    }

    public void OnGrabRelease()
    {
        controllerVisual.SetActive(true);
        isGrabbed = false;
    }
}
