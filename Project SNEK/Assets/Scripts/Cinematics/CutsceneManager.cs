using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using Hub.Interaction;
using GameManagement;
using Player;

namespace Cinematic
{
    public class CutsceneManager : Singleton<CutsceneManager>
    {
        [SerializeField] List<TimelineAsset> cutscenes = new List<TimelineAsset>();
        public PlayableDirector mainDirector;
        [SerializeField] GameObject playableCamera;
        [SerializeField] GameObject[] playableActors;


        private void Awake()
        {
            CreateSingleton(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                PlayCutscene(0);
            }
            if (Input.GetKeyDown(KeyCode.P))
                Debug.Log(InteractionManager.Instance.camTarget.actions);
        }

        public void PlayCutscene(int index)
        {
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.camTarget.actions++;
                InteractionManager.Instance.playerController.actions++;
            }
            PlayerManager.Instance.currentController.objectRenderer.SetActive(false);
            mainDirector.playableAsset = cutscenes[index];
            mainDirector.Play();


        }

        public void EndCustscene()
        {
            mainDirector.Stop();
            mainDirector.playableAsset = null;
            playableCamera.SetActive(false);
            foreach(GameObject actor in playableActors)
            {
                actor.SetActive(false);
            }

            InteractionManager.Instance.camTarget.actions--;
            InteractionManager.Instance.playerController.actions--;

            PlayerManager.Instance.currentController.objectRenderer.SetActive(true);
        }

        public void PauseCutscene()
        {
            //mainDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
            mainDirector.Pause();
        }

        public void ResumeCutscene()
        {
            //mainDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
            mainDirector.Resume();
        }
    }

}
