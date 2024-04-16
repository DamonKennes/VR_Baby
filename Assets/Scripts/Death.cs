using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Death : MonoBehaviour
{
    public RuntimeAnimatorController death;
    public GameObject knife;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void Die()
    {
        this.gameObject.GetComponent<Animator>().runtimeAnimatorController = death;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(knife)) {
            Die(); 
        }
        if (other.gameObject.CompareTag("bullet")) { 
            Die(); 
        }
    }
}
