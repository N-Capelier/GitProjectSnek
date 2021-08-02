using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Controller;

namespace Player.Spells
{
    /// <summary>
    /// Thomas
    /// </summary>
    public class BergamotSpell : PlayerSpell
    {

        [SerializeField] float spellDuration;

        [Header("Feedbacks")]
        [SerializeField] Material transparentMaterial;

        public override IEnumerator SpellCast(PlayerDirection direction)
        {
            //Find all possible intangible objects



            //  Play Animation and stop player for anim duration

            //PlayerManager.Instance.currentController.animator.Play(Animator.StringToHash("Anim_PlayerRun_Shield"));
            PlayerManager.Instance.currentController.spellMoveSpeedModifier = 0.01f;
            yield return new WaitForSeconds(0.4f); //Cooldown Anim Flash
            PlayerManager.Instance.currentController.spellMoveSpeedModifier = 1f;



            //Start Coroutine for each object
            for (int i = 0; i < MonochromElementsManager.Instance.elements.Count; i++)
            {
                if(MonochromElementsManager.Instance.elements[i] != null)
                    StartCoroutine(IntangibleCoroutine(MonochromElementsManager.Instance.elements[i]));
            }

            //Feedback prefab ?


            yield return null;
        }

        private IEnumerator IntangibleCoroutine(MonochromElement target)
        {
            MeshRenderer rd = target.rd;
            Material mat = rd.material;

            rd.material = transparentMaterial;

            rd.material.mainTexture = mat.mainTexture;

            Collider hitbox = target.hitbox;
            hitbox.enabled = false;

            yield return new WaitForSeconds(spellDuration);

            if(target != null)
                rd.material = mat;
                hitbox.enabled = true;

        }
    }
}
