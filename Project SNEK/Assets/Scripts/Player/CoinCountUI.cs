using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Saving;

namespace CoinUI
{
    public class CoinCountUI : Singleton<CoinCountUI>
    {
        [SerializeField] GameObject UiCoin;
        [SerializeField] TextMeshProUGUI currentCoinCount;
        private void Awake()
        {
            CreateSingleton();
        }
        void Start()
        {
            UiCoin.transform.localScale = Vector3.zero;
            currentCoinCount.text = "x" + SaveManager.Instance.state.heartCoinAmount;
        }

        public void UpdateCoinCount()
        {
            StartCoroutine(UpdateCoinCountAnim());
        }

        IEnumerator UpdateCoinCountAnim()
        {
            UiCoin.LeanScale(Vector3.one, 0.2f);
            yield return new WaitForSeconds(1f);
            currentCoinCount.text = "x" + SaveManager.Instance.state.heartCoinAmount;
            yield return new WaitForSeconds(1f);
            UiCoin.LeanScale(Vector3.zero, 0.2f);
            yield return null;
        }
    }
}

