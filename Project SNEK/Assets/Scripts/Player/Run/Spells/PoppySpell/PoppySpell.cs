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
        public GameObject bomb, kettleRenderer;
        public float animCooldown, throwSpeed, offset;
        public override IEnumerator SpellCast(PlayerDirection spellDirection)
        {
            if (!PlayerManager.Instance.currentController.playerRunSpirits.ConsumeSpirits(3))
            {
                yield break;
            }
            PlayerManager.Instance.currentController.spellMoveSpeedModifier = 0.01f;
            GameObject cauldron= Instantiate(bomb, transform.position, Quaternion.identity); ;
            PlayerManager.Instance.currentController.animator.Play("Anim_PlayerRun_Throw");
            switch (spellDirection)
            {
                case PlayerDirection.Up:
                    cauldron.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case PlayerDirection.Down:
                    cauldron.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    break;
                case PlayerDirection.Left:
                    cauldron.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                    break;
                case PlayerDirection.Right:
                    cauldron.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    break;
            }
            yield return new WaitForSeconds(0.245f); // Must be equal to 0.245f
            switch (spellDirection)
            {
                case PlayerDirection.Up:
                    cauldron.transform.position += new Vector3(0, 0, offset);
                    cauldron.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, throwSpeed);
                    break;
                case PlayerDirection.Down:
                    cauldron.transform.position += new Vector3(0, 0, -offset);
                    cauldron.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -throwSpeed);
                    break;
                case PlayerDirection.Left:
                    cauldron.transform.position += new Vector3(-offset, 0, 0);
                    cauldron.GetComponent<Rigidbody>().velocity = new Vector3(-throwSpeed, 0, 0);

                    break;
                case PlayerDirection.Right:
                    cauldron.transform.position += new Vector3(offset, 0, 0);
                    cauldron.GetComponent<Rigidbody>().velocity = new Vector3(throwSpeed, 0, 0);
                    break;
            }
            yield return new WaitForSeconds(animCooldown);
            PlayerManager.Instance.currentController.spellMoveSpeedModifier = 1f;
        }
    }
}

