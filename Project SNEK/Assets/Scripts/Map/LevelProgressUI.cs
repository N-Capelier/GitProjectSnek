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
        [SerializeField] int levelLength;
        [HideInInspector] public int cinematicLength;

        PlayerController player;

        float currentProgression;
        void Start()
        {
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
                currentProgression = player.transform.position.z - cinematicLength / levelLength;

                if(currentProgression > fillMask.fillAmount)
                {
                    fillMask.fillAmount = currentProgression;
                }
                else
                {
                    fillMask.fillAmount = Mathf.Lerp(fillMask.fillAmount, currentProgression, Time.deltaTime);
                }
            }
        }
    }
}
