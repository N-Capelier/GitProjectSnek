using Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player.Spells
{
    /// <summary>
    /// Coco
    /// </summary>
    public class PoppySpell : PlayerSpell
    {
        public GameObject bomb;
        public float animCooldown, throwSpeed;
        public override IEnumerator SpellCast(PlayerDirection spellDirection)
        {
            if (!PlayerManager.Instance.currentController.playerRunSpirits.ConsumeSpirits(3))
            {
                yield break;
            }
            PlayerManager.Instance.currentController.isCastingSpell = true;
            yield return new WaitForSeconds(animCooldown);
            GameObject cauldron = Instantiate(bomb, transform.position, Quaternion.identity);
            switch (spellDirection)
            {
                case PlayerDirection.Up:
                    cauldron.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, throwSpeed);
                    break;
                case PlayerDirection.Down:
                    cauldron.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -throwSpeed);
                    break;
                case PlayerDirection.Left:
                    cauldron.GetComponent<Rigidbody>().velocity = new Vector3(-throwSpeed, 0, 0);
                    break;
                case PlayerDirection.Right:
                    cauldron.GetComponent<Rigidbody>().velocity = new Vector3(throwSpeed, 0, 0);
                    break;
            }
            PlayerManager.Instance.currentController.isCastingSpell = false;

        }
    }
}

