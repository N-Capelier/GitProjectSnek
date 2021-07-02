using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

namespace Player.Technique
{
    /// <summary>
    /// Coco
    /// </summary>
    public class BubbleShieldTechnique : PlayerTechnique
    {
        public GameObject shieldPrefab; //Enfant de ce gameobject pour la detection des collisions
        GameObject shield;
        BubbleShieldBehaviour shieldBehaviour;
        [SerializeField] float shieldLifetime;

        public override IEnumerator TechniqueCast(Controller.PlayerDirection techniqueDirection)
        {
                PlayerManager.Instance.currentController.animator.Play(Animator.StringToHash("Anim_PlayerRun_Shield"));
                PlayerManager.Instance.currentController.spellMoveSpeedModifier = 0.01f;
                yield return new WaitForSeconds(0.833f); //Cooldown Anim Shield
                PlayerManager.Instance.currentController.spellMoveSpeedModifier = 1f;
                shield = Instantiate(shieldPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity, gameObject.transform);
                shieldBehaviour = shield.GetComponent<BubbleShieldBehaviour>();
                shieldBehaviour.shieldTechnique = this;
                shieldBehaviour.count = 0;
                yield return new WaitForSeconds(shieldLifetime);
                if (shield != null)
                {
                    StartCoroutine(shieldBehaviour.DestroyShield());
                }           
        }

        private void FixedUpdate()
        {
            if (shield != null)
                shield.transform.position = transform.position + Vector3.up * 0.5f;
        }
    }
}

