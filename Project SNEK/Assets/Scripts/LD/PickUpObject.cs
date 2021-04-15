using System.Collections;
using UnityEngine;
using Saving;


public class PickUpObject : MonoBehaviour
{
    public int iD;
    bool alreadyPicked;

    //public GameObject fX;

    private void Start()
    {
        //Check if already picked
        if (alreadyPicked)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
        {
            //Add iD to save system
            Depop();
        }
    }

    void Depop()
    {
        //Instantiate Fx
        Destroy(gameObject);
    }
}
