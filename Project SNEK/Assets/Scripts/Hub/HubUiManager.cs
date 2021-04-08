using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameManagement;
using Hub.Interaction;

namespace Hub.UI
{
    /// <summary>
    /// Coco
    /// </summary>
    public class HubUiManager : Singleton<HubUiManager>
    {

        [SerializeField] GameObject SkillTreeBox, LevelAccessBox, LetterBox;
        [SerializeField] Image fadeBackground;
        void Start()
        {
            SkillTreeBox.transform.localScale = Vector3.zero;
            LevelAccessBox.transform.localScale = Vector3.zero;
            LetterBox.transform.localScale = Vector3.zero;
        }

        public void OpenBox(GameObject box)
        {
            box.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseBox(GameObject box)
        {
            box.LeanScale(Vector3.zero, 0.2f);
        }

        public void OpenSkillTree()
        {
            SkillTreeBox.transform.LeanScale(Vector3.one, 0.3f);
        }

        public void CloseSkillTree()
        {
            SkillTreeBox.transform.LeanScale(Vector3.zero, 0.3f);
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        public void OpenLevelAccess()
        {
            LevelAccessBox.transform.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseLevelAcces()
        {
            LevelAccessBox.transform.LeanScale(Vector3.zero, 0.2f);
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        public void OpenLetterBox()
        {
            LetterBox.transform.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseLetterBox()
        {
            LetterBox.transform.LeanScale(Vector3.zero, 0.2f);
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        public void ActivateFadeBackground()
        {
            fadeBackground.enabled = true;
        }

        public void DeactivateFadeBackground()
        {
            fadeBackground.enabled = false;
        }
    }
}

