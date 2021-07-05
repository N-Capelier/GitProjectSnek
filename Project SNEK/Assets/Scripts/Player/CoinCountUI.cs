using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Saving;
using GameManagement;
using Player;
using AudioManagement;

namespace CoinUI
{
    public class CoinCountUI : MonoBehaviour
    {
        [SerializeField] GameObject UiCoin;
        [SerializeField] TextMeshProUGUI currentCoinCount;

        [SerializeField] GameObject playerRenderer;

        void Start()
        {
            UiCoin.transform.localScale = Vector3.zero;
            currentCoinCount.text = "x" + SaveManager.Instance.state.heartCoinAmount;
        }

        private void FixedUpdate()
        {
            if(GameManager.Instance.gameState.ActiveState == GameState.Hub && playerRenderer != null)
            {
                transform.position = playerRenderer.transform.localPosition;
            }
        }

        public void UpdateCoinCount()
        {
            StartCoroutine(UpdateCoinCountAnim());
        }


        IEnumerator UpdateCoinCountAnim()
        {
            int oldCount = SaveManager.Instance.state.heartCoinAmount - 1;
            AudioManager.Instance.PlaySoundEffect("ObjectSecretItemCollect");
            UiCoin.LeanScale(Vector3.one, 0.2f);
            currentCoinCount.text = "x" + oldCount;
            yield return new WaitForSeconds(1f);
            currentCoinCount.text = "x" + SaveManager.Instance.state.heartCoinAmount;
            yield return new WaitForSeconds(1f);
            UiCoin.LeanScale(Vector3.zero, 0.2f);
            yield return null;
        }
    }
}

