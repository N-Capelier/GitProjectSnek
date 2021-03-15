using System.Collections.Generic;
using UnityEngine;

namespace Player.Spirits
{
    public class SpiritManager : MonoBehaviour
    {
        [SerializeField] SpiritBehaviour spiritPrefab;

        [Space]
        public List<SpiritBehaviour> spiritChain = new List<SpiritBehaviour>();

        public void AddSpirit()
        {
            SpiritBehaviour _spirit = Instantiate(spiritPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
            spiritChain.Add(_spirit);
        }

        public void CutChain(int _index)
        {
            int _range = spiritChain.Count;

            for (int i = _index; i < _range; i++)
            {
                spiritChain[_index].Death();
                spiritChain.RemoveAt(_index);
            }
        }

    }
}