using System.Collections.Generic;
using UnityEngine;
using Player.Controller;

namespace Player.Spirits
{
    /// <summary>
    /// Nico
    /// </summary>
    public class SpiritManager : MonoBehaviour
    {
        [SerializeField] SpiritBehaviour spiritPrefab;
        [SerializeField] ParticleSystem spiritGetParticle, spawnPoofParticle;
        [HideInInspector] public PlayerDirection nextDir;

        [Space]
        public List<SpiritBehaviour> spiritChain = new List<SpiritBehaviour>();

        private void Start()
        {
            PlayerRunController.PlayerChangedDirection += OnPlayerChangingDirection;
        }

        //public void UpdateSpiritsVelocity()
        //{
        //    foreach(SpiritBehaviour _spirit in spiritChain)
        //    {
        //        _spirit.UpdateSpeed();
        //    }
        //}

        public void ResetSpiritsPositions()
        {
            for (int i = 0; i < spiritChain.Count; i++)
            {
                spiritChain[i].SetDirection(PlayerDirection.Up);
                spiritChain[i].transform.position = new Vector3(PlayerManager.Instance.currentController.transform.position.x, spiritChain[i].transform.position.y, PlayerManager.Instance.currentController.transform.position.z - (i + 1));
            }
        }

        void OnPlayerChangingDirection(PlayerDirection _dir)
        {
            spiritChain[0].SetDirection(_dir);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                AddSpirit();
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                ConsumeSpirits(3);
            }
        }
#endif

        public void AddSpirit()
        {

            for (int i = 0; i < spiritChain.Count; i++)
            {
                if (!spiritChain[i].objectRenderer.gameObject.activeSelf)
                {
                    Instantiate(spiritGetParticle, PlayerManager.Instance.currentController.transform);
                    Instantiate(spawnPoofParticle, spiritChain[i].transform);
                    spiritChain[i].objectRenderer.gameObject.SetActive(true);
                    break;
                }
            }
        }

        public int GetActiveSpirits()
        {
            int _count = 0;

            for (int i = 0; i < spiritChain.Count; i++)
            {
                if (!spiritChain[i].objectRenderer.activeSelf)
                    break;
                _count++;
            }

            return _count;
        }

        public bool ConsumeSpirits(int _count)
        {
            if (GetActiveSpirits() < _count)
            {
                return false;
            }
            int _index = 0;

            for (int i = 0; i < spiritChain.Count; i++)
            {
                if (!spiritChain[i].objectRenderer.activeSelf)
                    break;
                _index++;
            }
            CutChain(spiritChain[_index - _count]);
            return true;
        }

        public void CutChain(SpiritBehaviour _spiritBehaviour)
        {
            int _index = 25;

            for (int i = 0; i < spiritChain.Count; i++)
            {
                if (spiritChain[i] == _spiritBehaviour)
                {
                    _index = i;
                    break;
                }
            }

            if (_index == 25)
            {
                throw new System.Exception("Spirit not found in spirit chain!");
            }

            for (int i = _index; i < spiritChain.Count; i++)
            {
                StartCoroutine(spiritChain[i].Death());
            }
        }

        private void OnDestroy()
        {
            PlayerRunController.PlayerChangedDirection -= OnPlayerChangingDirection;
        }
    }
}