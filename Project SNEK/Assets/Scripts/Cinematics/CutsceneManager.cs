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
        Vector3[] actorsPositions;

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

        public IEnumerator PauseCutscene()
        {
            //mainDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);

            //actorsPositions = new Vector3[playableActors.Length];

            //for (int i = 0; i < actorsPositions.Length; i++)
            //{
            //    actorsPositions[i] = playableActors[i].transform.position;
            //}

            //yield return new WaitForEndOfFrame();

            mainDirector.Pause();
            //mainDirector.playableGraph.Stop();

            //yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.5f);

            //for (int i = 0; i < actorsPositions.Length; i++)
            //{
            //    playableActors[i].transform.position = actorsPositions[i];
            //}
        }

        public void ResumeCutscene()
        {
            //mainDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
            mainDirector.Resume();
            //mainDirector.playableGraph.Play();
        }
    }

}
