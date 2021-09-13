﻿using Player.Controller;
using System.Collections;
using UnityEngine;
using AudioManagement;

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
            /// Moved to PlayerSpell.cs
            //if (PlayerManager.Instance.currentController.playerRunSpirits.GetActiveSpirits() < 3)
            //{
            //    yield break;
            //}
            PlayerManager.Instance.currentController.spellMoveSpeedModifier = 0.01f;
            GameObject cauldron= Instantiate(bomb, transform.position, Quaternion.identity); ;
            PlayerManager.Instance.currentController.animator.Play(Animator.StringToHash("Anim_PlayerRun_Throw"));
            AudioManager.Instance.PlaySoundEffect("MarmiteLancer");
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

        public void Abort()
        {
            if(PlayerManager.Instance.currentController.playerRunSpell.currentSpellCast != null)
            {
                StopCoroutine(PlayerManager.Instance.currentController.playerRunSpell.currentSpellCast);
                PlayerManager.Instance.currentController.spellMoveSpeedModifier = 1f;
                PlayerManager.Instance.currentController.playerRunSpell.spellCooldownTimer.Stop();
            }
        }
    }
}

