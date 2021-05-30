using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saving;

public class CanvasPainting : MonoBehaviour
{
    /// <summary>
    /// Coco
    /// </summary>
    // Start is called before the first frame update

    [SerializeField] SpriteRenderer renderer;
    [SerializeField] Sprite[] rendererArray;
    void Start()
    {
        switch (SaveManager.Instance.state.canvasCurrentState)
        {
            case 0:
                renderer.sprite = null;
                break;
            case 1:
                renderer.sprite = rendererArray[0];
                break;
            case 2:
                renderer.sprite = rendererArray[1];
                break;
            case 3:
                renderer.sprite = rendererArray[2];
                break;
        }
    }
}
