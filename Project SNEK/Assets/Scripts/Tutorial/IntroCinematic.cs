using GameManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    /// <summary>
    /// Thomas
    /// </summary>
    public class IntroCinematic : MonoBehaviour
    {
        public AnimationClip intro; 
        void Start()
        {
            StartCoroutine(CinematicRoutine());
        }

        IEnumerator CinematicRoutine()
        {
            yield return new WaitForSeconds(intro.length);
            GameManager.Instance.gameState.Set(GameManager.Instance.gameState.ActiveState, "TutorialIntro");
        }
    }
}
