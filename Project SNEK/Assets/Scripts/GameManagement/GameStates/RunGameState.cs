using UnityEngine;
using Player;
using Player.Controller;
using Map;
using Rendering.Run;
using AudioManagement;
using Saving;
using UnityEngine.SceneManagement;
using Boss;
using Cinematic;

namespace GameManagement.GameStates
{
    /// <summary>
    /// Nico
    /// </summary>
    public class RunGameState : StateMachineBehaviour
    {
        bool playedBossCinematic = false;
        public static Source runMusic;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameObject _newController = Instantiate(PlayerManager.Instance.runPlayer, PlayerManager.Instance.transform);
            PlayerManager.Instance.currentController = _newController.GetComponent<PlayerController>();
            PlayerManager.Instance.currentController.StartCoroutine(PlayerManager.Instance.currentController
                .Init(SaveManager.Instance.state.bonusHealth, SaveManager.Instance.state.bonusRange, SaveManager.Instance.state.bonusPower));
            PlayerManager.Instance.currentController.transform.position = MapGrid.Instance.GetWorldPos(5, 0);
            if(SceneManager.GetActiveScene().name == "Boss Anorexia")
            {
                RunCamController.Instance.Set(CamState.SemiScrolling);
                if (!playedBossCinematic)
                {
                    playedBossCinematic = true;
                    CutsceneManager.Instance.PlayCutscene(TestAnorexia.Instance.introCinematic);
                    GameManager.Instance.playedBossCinematic = true;
                }
            }
            else
            {
                RunCamController.Instance.Set(CamState.PlayerScrolling);
            }            
            if (runMusic == null)
            {
                if(SceneManager.GetActiveScene().name != "Boss Anorexia")
                {
                    runMusic = AudioManager.Instance.PlayThisSoundEffect("LevelMusic", true);
                }
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(PlayerManager.Instance.currentController.gameObject);
            Destroy(MapGrid.Instance.gameObject);
            if (runMusic != null)
            {
                runMusic.audioSource.Stop();
                runMusic = null;
            }
            GameManager.Instance.playedBossCinematic = false;
        }
    }
}