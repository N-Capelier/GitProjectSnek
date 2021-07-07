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

        PlayerController player;

        float currentProgression;

        void Start()
        {
            GameManager.Instance.uiHandler.levelProgressUI = this;
            
            fillMask.fillAmount = 0;
            player = PlayerManager.Instance.currentController;
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

                if(currentProgression > fillMask.fillAmount)
                {
                    fillMask.fillAmount = currentProgression;
                    headSlider.value = currentProgression;
                }
                else
                {
                    fillMask.fillAmount = Mathf.Lerp(fillMask.fillAmount, currentProgression, Time.deltaTime);
                }
            }
        }
    }
}
