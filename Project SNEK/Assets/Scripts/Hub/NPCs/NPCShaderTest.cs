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
        gameObject.GetComponent<NPCFaceManager>().RandomizeMouth();
        canAdd = false;
        yield return new WaitForSeconds(0.2f);
        canAdd = true;
    }
}
