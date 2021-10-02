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
using AudioManagement;

namespace Cinematic
{
    public class CutsceneManager : Singleton<CutsceneManager>
    {
        //[SerializeField] List<TimelineAsset> cutscenes = new List<TimelineAsset>();
        public PlayableDirector mainDirector;
        [SerializeField] GameObject playableCamera;
        [SerializeField] GameObject[] playableActors;
        [SerializeField] Material newNPCMaterial; // Pour l'instant c'est juste le materiel de Dark Poppy
        //Vector3[] actorsPositions;
        [HideInInspector] public bool isBossFinished = false;

        Source musicAudio;

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

                //Hide UI elements
                GameManager.Instance.uiHandler.HideUIHub();
            }
            else if(GameManager.Instance.gameState.ActiveState == GameState.Run)
            {
                spiritCount = PlayerManager.Instance.currentController.playerRunSpirits.GetActiveSpirits();
                for (int i = 0; i < PlayerManager.Instance.currentController.playerRunSpirits.spiritChain.Count; i++)
                {
                    PlayerManager.Instance.currentController.playerRunSpirits.spiritChain[i].objectRenderer.SetActive(false);
                }
                PlayerManager.Instance.currentController.isInCutscene = true;

                //Hide UI elements
                GameManager.Instance.uiHandler.HideUIRun();
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

                if(!isBossFinished)
                {
                    GameManager.Instance.StartCoroutine(PlayerManager.Instance.currentController.RespawnAfterCutscene(spiritCount));
                }
                else
                {
                    isBossFinished = false;
                }

                //Show UI elements
                GameManager.Instance.uiHandler.ShowUIRun();
            }
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                //Show UI elements
                GameManager.Instance.uiHandler.ShowUIHub();
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

        public void BackToHubFromLvl1_1()
        {
            //Add to save state
            SaveManager.Instance.state.bergamotState = 16;
            SaveManager.Instance.state.thistleState = 6;
            SaveManager.Instance.state.poppyState = 13;
            SaveManager.Instance.state.unlockedLetters = 2;
            SaveManager.Instance.state.canvasCurrentState = 1;
            EndCustscene();
            GameManager.Instance.gameState.Set(GameState.Hub, "Hub");
        }

        public void BackToHubFromLvl1_2()
        {
            //Add to save state
            SaveManager.Instance.state.bergamotState = 23;
            SaveManager.Instance.state.thistleState = 9;
            SaveManager.Instance.state.poppyState = 26;
            SaveManager.Instance.state.unlockedLetters = 3;
            SaveManager.Instance.state.canvasCurrentState = 2;
            EndCustscene();
            GameManager.Instance.gameState.Set(GameState.Hub, "Hub");
        }

        public void BackToHubFromLvl1_3()
        {
            //Add to save state
            SaveManager.Instance.state.bergamotState = 27;
            SaveManager.Instance.state.thistleState = 14;
            SaveManager.Instance.state.poppyState = 29;
            SaveManager.Instance.state.unlockedLetters = 4;
            SaveManager.Instance.state.canvasCurrentState = 3;
            isBossFinished = true;
            EndCustscene();
            GameManager.Instance.gameState.Set(GameState.Hub, "Hub");
        }

        public void BackToHubFromLvl2_1()
        {
            //Add to save state
            SaveManager.Instance.state.bergamotState = 32;
            SaveManager.Instance.state.thistleState = 20;
            SaveManager.Instance.state.poppyState = 36;
            SaveManager.Instance.state.unlockedLetters = 5;
            SaveManager.Instance.state.canvasCurrentState = 4;
            EndCustscene();
            GameManager.Instance.gameState.Set(GameState.Hub, "Hub");
        }

        public void BackToHubFromLvl2_2()
        {
            //Add to save state
            SaveManager.Instance.state.bergamotState = 37;
            SaveManager.Instance.state.thistleState = 25;
            SaveManager.Instance.state.poppyState = 42;
            SaveManager.Instance.state.unlockedLetters = 6;
            SaveManager.Instance.state.canvasCurrentState = 5;
            EndCustscene();
            GameManager.Instance.gameState.Set(GameState.Hub, "Hub");
        }

        public void BackToHubFromLvl2_3()
        {
            //Add to save state
            SaveManager.Instance.state.bergamotState = 39;
            SaveManager.Instance.state.thistleState = 30;
            SaveManager.Instance.state.poppyState = 48;
            SaveManager.Instance.state.unlockedLetters = 7;
            SaveManager.Instance.state.canvasCurrentState = 6;
            EndCustscene();
            GameManager.Instance.gameState.Set(GameState.Hub, "Hub");
        }

        public void BackToHubFromLvl3_1()
        {
            //Add to save state
            SaveManager.Instance.state.bergamotState = 44;
            SaveManager.Instance.state.thistleState = 36;
            SaveManager.Instance.state.poppyState = 52;
            SaveManager.Instance.state.unlockedLetters = 8;
            SaveManager.Instance.state.canvasCurrentState = 7;
            EndCustscene();
            GameManager.Instance.gameState.Set(GameState.Hub, "Hub");
        }

        public void BackToHubFromLvl3_2()
        {
            //Add to save state
            SaveManager.Instance.state.bergamotState = 50;
            SaveManager.Instance.state.thistleState = 41;
            SaveManager.Instance.state.poppyState = 58;
            SaveManager.Instance.state.unlockedLetters = 9;
            SaveManager.Instance.state.canvasCurrentState = 8;
            EndCustscene();
            GameManager.Instance.gameState.Set(GameState.Hub, "Hub");
        }

        public void BackToHubFromLvl3_3()
        {
            //Add to save state
            SaveManager.Instance.state.bergamotState = 57;
            SaveManager.Instance.state.thistleState = 46;
            SaveManager.Instance.state.poppyState = 64;
            SaveManager.Instance.state.unlockedLetters = 10;
            SaveManager.Instance.state.canvasCurrentState = 9;
            EndCustscene();
            GameManager.Instance.gameState.Set(GameState.Hub, "Hub");
        }

        public void PlayMusic(string name)
        {
            MusicManager.Instance.Music(name);
        }

        public void StopMusic()
        {
            MusicManager.Instance.Music(null);
        }

        public void ChangeNPCMaterial(SkinnedMeshRenderer renderer)
        {
            renderer.material = newNPCMaterial;
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
