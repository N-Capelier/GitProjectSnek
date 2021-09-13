using Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Spells
{
    public class ThistleSpell : PlayerSpell
    {
        public GameObject shieldPrefab;
        private GameObject shield;
        [SerializeField] private float shieldLifetime;
        public override IEnumerator SpellCast(PlayerDirection spellDirection)
        {
            PlayerManager.Instance.currentController.animator.Play(Animator.StringToHash("Anim_PlayerRun_Shield"));
            PlayerManager.Instance.currentController.spellMoveSpeedModifier = 0.01f;
            yield return new WaitForSeconds(0.833f); //Cooldown Anim Shield
            PlayerManager.Instance.currentController.spellMoveSpeedModifier = 1f;
            if(shield == null)
            {
                shield = Instantiate(shieldPrefab, this.transform);
            }
            shield.SetActive(true);
            shield.GetComponent<ParticleSystem>().Play();
            yield return new WaitForSeconds(shieldLifetime);
            shield.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (shield != null)
                shield.transform.position = transform.position + Vector3.up * 0.5f;
        }

        public void Abort()
        {
            print(PlayerManager.Instance.currentController.playerRunSpell);
            if (PlayerManager.Instance.currentController.playerRunSpell.currentSpellCast != null)
            {
                if(shield!= null)
                {
                    shield.GetComponent<ParticleSystem>().Stop();
                    shield.SetActive(false);
                }
                StopCoroutine(PlayerManager.Instance.currentController.playerRunSpell.currentSpellCast);
                PlayerManager.Instance.currentController.spellMoveSpeedModifier = 1f;
                PlayerManager.Instance.currentController.playerRunSpell.spellCooldownTimer.Stop();

            }
        }

    }

}
