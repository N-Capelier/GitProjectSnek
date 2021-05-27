using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShaderTest : MonoBehaviour
{
    /// <summary>
    /// Coco
    /// </summary>

    bool canAdd = false;
    void Start()
    {
        StartCoroutine(Add());
    }

    private void Update()
    {
        if(canAdd == true)
        {
            StartCoroutine(Add());
        }
    }

    IEnumerator Add()
    {
        //gameObject.GetComponent<NPCFaceManager>().RandomizeMouth();
        //gameObject.GetComponent<NPCFaceManager>().SetEyesExpression(2);
        canAdd = false;
        yield return new WaitForSeconds(0.1f);
        canAdd = true;
    }
}
