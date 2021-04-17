using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Hub.Interaction;
using GameManagement;
using Player;
using Saving;

namespace Cinematic
{
    public class CutsceneManager : Singleton<CutsceneManager>
    {
        //[SerializeField] List<TimelineAsset> cutscenes = new List<TimelineAsset>();
        public PlayableDirector mainDirector;
        [SerializeField] GameObject playableCamera;
        [SerializeField] GameObject[] playableActors;
        //Vector3[] actorsPositions;

        private void Awake()
        {
            CreateSingleton();
        }

        public void PlayCutscene(TimelineAsset _cutscene)
        {
            print("Playing cutscene: " + _cutscene.name);
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.camTarget.actions++;
                InteractionManager.Instance.playerController.actions++;
            }
            if(GameManager.Instance.gameState.ActiveState != GameState.Cinematic)
            {
                PlayerManager.Instance.currentController.objectRenderer.SetActive(false);
            }
            mainDirector.playableAsset = _cutscene;
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
            if (GameManager.Instance.gameState.ActiveState != GameState.Cinematic)
            {
                InteractionManager.Instance.camTarget.actions--;
                InteractionManager.Instance.playerController.actions--;

                PlayerManager.Instance.currentController.objectRenderer.SetActive(true);
            }
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

        public void SetBergamotState(int newState)
        {
            SaveManager.Instance.state.bergamotState = newState;
            NPCManager.Instance.RefreshNPCs();
        }
    }

}
