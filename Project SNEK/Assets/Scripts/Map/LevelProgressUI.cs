using GameManagement;
using Player;
using Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    /// <summary>
    /// Thomas
    /// </summary>
    public class LevelProgressUI : MonoBehaviour
    {
        [SerializeField] Image fillMask;
        [SerializeField] Slider headSlider;
        [SerializeField] int levelLength;
        [SerializeField] CanvasGroup canvasGroup;

        PlayerController player;

        float currentProgression;

        [Header("Checkpoints")]
        [SerializeField] List<int> checkpointPositions;
        [SerializeField] List<CheckpointUI> checkpoints;

        [Header("Cinematic")]
        [SerializeField] Image cinematicImage;
        [SerializeField] int minBound;
        [SerializeField] int maxBound;


        void Start()
        {
            GameManager.Instance.uiHandler.levelProgressUI = this;
            
            fillMask.fillAmount = 0;
            player = PlayerManager.Instance.currentController;

            Initcheckpoints();
            FadeOut();
        }

        void Update()
        {
            if(player == null)
            {
                player = PlayerManager.Instance.currentController;
            }
            else
            {
                currentProgression = player.transform.position.z / levelLength;

                if(currentProgression != fillMask.fillAmount)
                {
                    fillMask.fillAmount = currentProgression;
                    headSlider.value = currentProgression;
                }
                CheckpointFeedback();
            }

        }

        void Initcheckpoints()
        {
            for (int i = 0; i < checkpointPositions.Count; i++)
            {
                float value = (float)checkpointPositions[i] / (float)levelLength;
                //print(value);

                checkpoints[i].rectTransform.anchorMin = new Vector2(0, value);
                checkpoints[i].rectTransform.anchorMax = new Vector2(1,value); 
            }

            cinematicImage.rectTransform.anchorMin = new Vector2(0, (float)minBound / (float)levelLength);
            cinematicImage.rectTransform.anchorMax = new Vector2(1, (float)maxBound / (float)levelLength);
        }


        void CheckpointFeedback()
        {
            for (int i = 0; i < checkpointPositions.Count; i++)
            {
                if(!checkpoints[i].check)
                {
                    if(player.transform.position.z < checkpointPositions[i])
                    {
                        break;
                    }
                    else
                    {
                        checkpoints[i].check = true;
                        checkpoints[i].gameObject.SetActive(false);
                        FadeOut();
                    }
                }
            }
        }


        public void FadeOut()
        {
            if(canvasGroup.alpha !=0)
            {
                if(canvasGroup.alpha <1)
                {
                    canvasGroup.LeanAlpha(1, 0.5f);
                    canvasGroup.LeanAlpha(0.5f, 0.5f).setDelay(3f);
                }
                else
                {
                    canvasGroup.LeanAlpha(0.5f, 0.5f).setDelay(3f);
                }
            }
        }
    }
}
