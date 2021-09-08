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
        [SerializeField] GameObject cameraFlashFeedback;
        [SerializeField] GameObject cameraObject;

        public override IEnumerator SpellCast(PlayerDirection direction)
        {
            //Play Animation and stop player for anim duration
            GameObject camera = Instantiate(cameraObject, transform.position, Quaternion.identity, PlayerManager.Instance.currentController.objectRenderer.transform);
            switch (direction)
            {
                case PlayerDirection.Up:
                    camera.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case PlayerDirection.Down:
                    camera.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    break;
                case PlayerDirection.Left:
                    camera.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                    break;
                case PlayerDirection.Right:
                    camera.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    break;
            }

            PlayerManager.Instance.currentController.animator.Play(Animator.StringToHash("Anim_PlayerRun_Photo"));
            PlayerManager.Instance.currentController.spellMoveSpeedModifier = 0.01f;
            yield return new WaitForSeconds(0.5f); //Cooldown Anim Flash
            PlayerManager.Instance.currentController.spellMoveSpeedModifier = 1f;



            //Start Coroutine for each object
            for (int i = 0; i < MonochromElementsManager.Instance.elements.Count; i++)
            {
                if(MonochromElementsManager.Instance.elements[i] != null)
                    StartCoroutine(IntangibleCoroutine(MonochromElementsManager.Instance.elements[i]));
            }

            //Feedback prefab 
            Instantiate(cameraFlashFeedback, transform.position, Quaternion.identity);

            yield return null;
        }

        private IEnumerator IntangibleCoroutine(MonochromElement target)
        {
            MeshRenderer rd = target.rd;
            Material mat = rd.material;

            rd.material = transparentMaterial;

            rd.material.mainTexture = mat.mainTexture;

            if (target.shaderRenderer != null)
                target.shaderRenderer.SetActive(false);
            if (target.particles != null)
                target.particles.SetActive(false);

            Collider hitbox = target.hitbox;
            hitbox.enabled = false;

            yield return new WaitForSeconds(spellDuration);

            if(target != null)
            {
                rd.material = mat;
                hitbox.enabled = true;

                if (target.shaderRenderer != null)
                    target.shaderRenderer.SetActive(false);
                if (target.particles != null)
                    target.particles.SetActive(false);
            }
        }

        public void Abort()
        {
            if (PlayerManager.Instance.currentController.playerRunSpell.currentSpellCast != null)
            {
                StopCoroutine(PlayerManager.Instance.currentController.playerRunSpell.currentSpellCast);
                PlayerManager.Instance.currentController.spellMoveSpeedModifier = 1f;
                PlayerManager.Instance.currentController.playerRunSpell.spellCooldownTimer.Stop();
            }
        }
    }
}
