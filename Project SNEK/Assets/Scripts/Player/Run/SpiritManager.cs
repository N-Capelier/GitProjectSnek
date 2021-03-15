using System.Collections.Generic;
using UnityEngine;
using Player.Controller;

namespace Player.Spirits
{
    public class SpiritManager : MonoBehaviour
    {
        [SerializeField] SpiritBehaviour spiritPrefab;
        [HideInInspector] public PlayerDirection nextDir;

        [Space]
        public List<SpiritBehaviour> spiritChain = new List<SpiritBehaviour>();

        private void Start()
        {
            PlayerRunController.PlayerChangedDirection += OnPlayerChangingDirection;
        }

        public void ResetSpiritsPositions()
        {
            for (int i = 0; i < spiritChain.Count; i++)
            {
                spiritChain[i].SetDirection(PlayerDirection.Up);
                spiritChain[i].transform.position = new Vector3(PlayerManager.Instance.currentController.transform.position.x, spiritChain[i].transform.position.y, PlayerManager.Instance.currentController.transform.position.z - (i + 1));
            }
        }

        void OnPlayerChangingDirection()
        {
            nextDir = PlayerManager.Instance.currentController.currentDirection;
            spiritChain[0].SetDirection(nextDir);
        }

        public void AddSpirit()
        {
            for (int i = 0; i < spiritChain.Count; i++)
            {
                if(!spiritChain[i].objectRenderer.gameObject.activeSelf)
                {
                    spiritChain[i].objectRenderer.gameObject.SetActive(true);
                    break;
                }
            }
        }

        public void CutChain(SpiritBehaviour _spiritBehaviour)
        {
            int _index = 25;

            for (int i = 0; i < spiritChain.Count; i++)
            {
                if(spiritChain[i] == _spiritBehaviour)
                {
                    _index = i;
                    break;
                }
            }

            if(_index == 25)
            {
                throw new System.Exception("Spirit not found in spirit chain!");
            }

            int _range = spiritChain.Count;

            for (int i = _index; i < _range; i++)
            {
                spiritChain[i].Death();
                spiritChain.RemoveAt(i);
            }
        }

        private void OnDestroy()
        {
            PlayerRunController.PlayerChangedDirection -= OnPlayerChangingDirection;
        }
    }
}