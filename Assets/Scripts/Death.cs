using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Death : MonoBehaviour
{
    public RuntimeAnimatorController death;
    public GameObject knife;
    public bool hasDied = false;

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
        if (!hasDied)
        {
            if (this.gameObject.GetComponent<Animation>() != null)
            {
                this.gameObject.GetComponent<Animation>().Play("death");
            }
            else
            {
                this.gameObject.GetComponent<Animator>().runtimeAnimatorController = death;
            }
            hasDied = true;
        }      
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
