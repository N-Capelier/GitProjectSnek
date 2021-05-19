using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFaceManager : MonoBehaviour
{
    /// <summary>
    /// Coco
    /// </summary>

    [SerializeField] Material faceMat;
    [SerializeField] Vector2[] eyesList;
    [SerializeField] Vector2[] mouthList;

    public void RandomizeMouth()
    {
        faceMat.SetVector("Mouth", mouthList[Random.Range(0, mouthList.Length + 1)]);
    }
}
