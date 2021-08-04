using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashFeedback : MonoBehaviour
{

    [Header("Flash Feedback")]
    [SerializeField] CanvasGroup flashCanvasGroup;
    [SerializeField] CanvasGroup vignetteCanvasGroup;
    [SerializeField] CanvasGroup noiseCanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        LeanTween.alphaCanvas(flashCanvasGroup, 0, 0.3f);
        LeanTween.alphaCanvas(noiseCanvasGroup, 0, 8.5f);
        LeanTween.alphaCanvas(vignetteCanvasGroup, 0, 8.5f).setDestroyOnComplete(true);
    }
}
