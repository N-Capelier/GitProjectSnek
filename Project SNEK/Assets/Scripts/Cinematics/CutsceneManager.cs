using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] List<TimelineAsset> cutscenes = new List<TimelineAsset>();
    [SerializeField] PlayableDirector mainDirector;
    
    void Start()
    {
        PlayCutscene(0);
    }

    void PlayCutscene(int index)
    {
        mainDirector.playableAsset = cutscenes[index];
        mainDirector.Play();
    }

    void EndCustscene(int index)
    {
        mainDirector.Stop();
    }
}
