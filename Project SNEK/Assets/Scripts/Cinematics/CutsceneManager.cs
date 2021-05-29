using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Hub.Interaction;
using GameManagement;
using Player;
using Saving;
using UnityEngine.SceneManagement;

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


        int spiritCount = 0;

        public void PlayCutscene(TimelineAsset _cutscene)
        {
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.camTarget.actions++;
                InteractionManager.Instance.playerController.actions++;  //////////////////
            }
            else if(GameManager.Instance.gameState.ActiveState == GameState.Run)
            {
                spiritCount = PlayerManager.Instance.currentController.playerRunSpirits.GetActiveSpirits();
                for (int i = 0; i < PlayerManager.Instance.currentController.playerRunSpirits.spiritChain.Count; i++)
                {
                    PlayerManager.Instance.currentController.playerRunSpirits.spiritChain[i].objectRenderer.SetActive(false);
                }
                PlayerManager.Instance.currentController.isInCutscene = true;
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
            if (GameManager.Instance.gameState.ActiveState != GameState.Cinematic && GameManager.Instance.gameState.ActiveState != GameState.Run)
            {
                InteractionManager.Instance.camTarget.actions--;
                InteractionManager.Instance.playerController.actions--;

            }
            if (GameManager.Instance.gameState.ActiveState != GameState.Cinematic)
            {
                PlayerManager.Instance.currentController.objectRenderer.SetActive(true);
            }
            if(GameManager.Instance.gameState.ActiveState == GameState.Run)
            {
                //PlayerManager.Instance.currentController.transform.position = PlayerManager.Instance.currentController.checkPoint.position;
                //PlayerManager.Instance.currentController.SnapPosition();
                //PlayerManager.Instance.currentController.playerRunSpirits.ResetSpiritsPositions();
                //for (int i = 0; i < spiritCount; i++)
                //{
                //    PlayerManager.Instance.currentController.playerRunSpirits.AddSpirit();
                //}
                //PlayerManager.Instance.currentController.objectRenderer.SetActive(true);
                //PlayerManager.Instance.currentController.playerRunSpirits.ResetSpiritsPositions();

                //PlayerManager.Instance.currentController.Death(0);
                //for (int i = 0; i < spiritCount; i++)
                //{
                //    PlayerManager.Instance.currentController.playerRunSpirits.AddSpirit();
                //}

                //PlayerManager.Instance.currentController.isInCutscene = false;

                PlayerManager.Instance.currentController.RespawnAfterCutscene(spiritCount);
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


        public void BackToHubFromBoss()
        {
            //Add to save state
            EndCustscene();
            GameManager.Instance.gameState.Set(GameState.Hub, "Hub");            
        }

        public void SetBergamotState(float _state)
        {

        }

        public void SetPoppyState(float _state)
        {

        }

        public void SetThitleState(float _state)
        {

        }
    }

}
