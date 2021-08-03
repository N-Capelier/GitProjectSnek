using UnityEngine;
using Player.Controller;

namespace Player.Spells
{
    public class SpellPickup : MonoBehaviour
    {
        [SerializeField] Spell spell;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                PlayerManager.Instance.currentController.runController.SetSpell(spell);
                Destroy(gameObject);
            }
        }
    }
}