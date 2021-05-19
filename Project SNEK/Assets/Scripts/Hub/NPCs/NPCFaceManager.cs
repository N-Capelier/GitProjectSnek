using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCFaceManager : MonoBehaviour
{
    /// <summary>
    /// Coco
    /// </summary>

    [SerializeField] Material faceMat;
    [SerializeField] Vector2[] eyesList;
    [SerializeField] Vector2[][] mouthList;
    [SerializeField] int mouthColumn;


    public void RandomizeMouth(int face)
    {
        faceMat.SetVector("Mouth", mouthList[face][Random.Range(0, mouthList.Length + 1)]);
    }

    /// Le int Face représente l'expression qu'adopte le personnage,
    /// Ensuite on randomise la bouche de cette expression pour garder une cohérence.
}
