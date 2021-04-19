using UnityEngine;
using UnityEngine.UI;
using GameManagement;
using Saving;
using Hub.Interaction;
using TMPro;
using AudioManagement;

namespace Hub.UI
{
    /// <summary>
    /// Coco
    /// </summary>
    public class HubUiManager : Singleton<HubUiManager>
    {
        enum BonusStat
        {
            Health,
            Range,
            Power
        }

        [SerializeField] GameObject skillTreeBox, levelAccessBox, letterBox,letterScreen,demoScreen;
        [SerializeField] Image fadeBackground;
        [SerializeField] TextMeshProUGUI hearthCoins;
        [SerializeField] TextMeshProUGUI equipedTechnicText; //temp
        [SerializeField] TextMeshProUGUI upgradeText;
        [SerializeField] GameObject confirmationBox;
        [SerializeField] TextMeshProUGUI fadeBack;

        int cost;
        BonusStat statToUpgrade;

        private void Awake()
        {
            CreateSingleton();
        }

        void Start()
        {
            skillTreeBox.transform.localScale = Vector3.zero;
            skillTreeBox.gameObject.SetActive(false);
            levelAccessBox.transform.localScale = Vector3.zero;
            levelAccessBox.gameObject.SetActive(false);
            letterBox.transform.localScale = Vector3.zero;
            letterBox.gameObject.SetActive(false);
            demoScreen.transform.localScale = Vector3.zero;
            demoScreen.gameObject.SetActive(false);
            EquipTechnic(SaveManager.Instance.state.equipedTechnic);
            DrawHeartCoins();

            if(SaveManager.Instance.state.isDemoFinished)
            {
                OpenBox(demoScreen);
            }
        }

        public void PowerUphealth()
        {
            if(SaveManager.Instance.state.heartCoinAmount >= SaveManager.Instance.state.bonusHealth + 1)
            {
                cost = SaveManager.Instance.state.bonusHealth + 1;
                statToUpgrade = BonusStat.Health;
                OpenBox(confirmationBox);
                //SaveManager.Instance.state.heartCoinAmount -= SaveManager.Instance.state.bonusHealth + 1;
                //SaveManager.Instance.state.bonusHealth += 1;
                //DrawHeartCoins();
            }
        }

        public void PowerUpRange()
        {
            if (SaveManager.Instance.state.heartCoinAmount >= SaveManager.Instance.state.bonusRange + 1)
            {
                cost = SaveManager.Instance.state.bonusRange + 1;
                statToUpgrade = BonusStat.Range;
                OpenBox(confirmationBox);
                //SaveManager.Instance.state.heartCoinAmount -= SaveManager.Instance.state.bonusRange + 1;
                //SaveManager.Instance.state.bonusRange += 1;
                //DrawHeartCoins();
            }
        }

        public void PowerUpPower()
        {
            if (SaveManager.Instance.state.heartCoinAmount >= SaveManager.Instance.state.bonusPower + 1)
            {
                cost = SaveManager.Instance.state.bonusPower + 1;
                statToUpgrade = BonusStat.Power;
                OpenBox(confirmationBox);
                //SaveManager.Instance.state.heartCoinAmount -= SaveManager.Instance.state.bonusPower + 1;
                //SaveManager.Instance.state.bonusPower += 1;
                //DrawHeartCoins();
            }
        }

        public void ApplyPowerUp()
        {
            SaveManager.Instance.state.heartCoinAmount -= cost;

            switch (statToUpgrade)
            {
                case BonusStat.Health:
                    SaveManager.Instance.state.bonusHealth += 1;
                    break;
                case BonusStat.Range:
                    SaveManager.Instance.state.bonusRange += 1;
                    break;
                case BonusStat.Power:
                    SaveManager.Instance.state.bonusPower += 1;
                    break;
            }
            DrawHeartCoins();
            CloseBox(confirmationBox);
        }

        public void ResetPowerUps()
        {
            SaveManager.Instance.state.heartCoinAmount += SaveManager.Instance.state.bonusHealth + SaveManager.Instance.state.bonusRange + SaveManager.Instance.state.bonusPower;

            if(SaveManager.Instance.state.bonusHealth > 0)
            {
                for (int i = 0; i < SaveManager.Instance.state.bonusHealth; i++)
                {
                    SaveManager.Instance.state.heartCoinAmount += i;
                }
            }
            if (SaveManager.Instance.state.bonusRange > 0)
            {
                for (int i = 0; i < SaveManager.Instance.state.bonusRange; i++)
                {
                    SaveManager.Instance.state.heartCoinAmount += i;
                }
            }
            if (SaveManager.Instance.state.bonusPower > 0)
            {
                for (int i = 0; i < SaveManager.Instance.state.bonusPower; i++)
                {
                    SaveManager.Instance.state.heartCoinAmount += i;
                }
            }

            SaveManager.Instance.state.bonusHealth = SaveManager.Instance.state.bonusRange = SaveManager.Instance.state.bonusPower = 0;
            DrawHeartCoins();
        }

        public void EquipTechnic(int _index)
        {
            SaveManager.Instance.state.equipedTechnic = _index;
            switch(_index)
            {
                case 0:
                    equipedTechnicText.text = "No technic equiped!";
                    break;
                case 1:
                    equipedTechnicText.text = "Equiped technic: Swift combo.";
                    break;
                case 2:
                    equipedTechnicText.text = "Equiped technic: Sword beam.";
                    break;
                case 3:
                    equipedTechnicText.text = "Equiped technic: Bubble shield.";
                    break;
            }
        }

        public void DrawHeartCoins()
        {
            hearthCoins.text = $"x{SaveManager.Instance.state.heartCoinAmount}";
        }

        public void OpenBox(GameObject box)
        {
            box.LeanScale(Vector3.one, 0.2f);
            upgradeText.text = $"Pay {cost} heart coins to upgrade this skill?";
        }

        public void CloseBox(GameObject box)
        {
            AudioManager.Instance.PlaySoundEffect("UINone");
            box.LeanScale(Vector3.zero, 0.2f);
        }

        public void OpenSkillTree()
        {
            skillTreeBox.gameObject.SetActive(true);
            skillTreeBox.transform.LeanScale(Vector3.one, 0.3f);
        }

        public void CloseSkillTree()
        {
            skillTreeBox.transform.LeanScale(Vector3.zero, 0.3f).setOnComplete(SetSkillTreeFalse);
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        public void OpenLevelAccess()
        {
            AudioManager.Instance.PlaySoundEffect("UIClick");
            levelAccessBox.gameObject.SetActive(true);
            levelAccessBox.transform.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseLevelAcces()
        {
            AudioManager.Instance.PlaySoundEffect("UINone");
            levelAccessBox.transform.LeanScale(Vector3.zero, 0.2f).setOnComplete(SetLevelAccessFalse); 
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        public void OpenLetterBox()
        {
            AudioManager.Instance.PlaySoundEffect("UIClick");
            letterBox.SetActive(true);
            letterBox.transform.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseLetterBox()
        {
            AudioManager.Instance.PlaySoundEffect("UINone");
            letterBox.transform.LeanScale(Vector3.zero, 0.2f).setOnComplete(SetLetterBoxFalse);
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

        public void OpenLetter()
        {
            fadeBack.gameObject.LeanAlpha(1, 0.5f);
        }

        public void SetLetterBoxFalse()
        {
            letterBox.SetActive(false);
        }        
        public void SetLevelAccessFalse()
        {
            levelAccessBox.SetActive(false);
        }
        public void SetSkillTreeFalse()
        {
            skillTreeBox.SetActive(false);
        }
    }
}

