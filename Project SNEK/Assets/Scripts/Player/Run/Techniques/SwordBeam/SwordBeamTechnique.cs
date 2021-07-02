using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Technique
{
    /// <summary>
    /// Coco
    /// </summary>
    public class SwordBeamTechnique : PlayerTechnique
    {
        public GameObject beamPrefab;
        GameObject beam;
        [SerializeField] float beamSpeed;

        public override IEnumerator TechniqueCast(Controller.PlayerDirection techniqueDirection)
        {
            PlayerManager.Instance.currentController.spellMoveSpeedModifier = 0.01f;
            PlayerManager.Instance.currentController.animator.Play(Animator.StringToHash("Anim_PlayerRun_SwordBeam"));
            yield return new WaitForSeconds(0.5f); // Timing partie 1
            beam = Instantiate(beamPrefab, transform.position, Quaternion.identity);
            switch (techniqueDirection)
            {
                case Controller.PlayerDirection.Up:
                    beam.GetComponent<Rigidbody>().velocity = Vector3.forward * beamSpeed;
                    beam.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case Controller.PlayerDirection.Down:
                    beam.GetComponent<Rigidbody>().velocity = Vector3.back * beamSpeed;
                    beam.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    break;
                case Controller.PlayerDirection.Right:
                    beam.GetComponent<Rigidbody>().velocity = Vector3.right * beamSpeed;
                    beam.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    break;
                case Controller.PlayerDirection.Left:
                    beam.GetComponent<Rigidbody>().velocity = Vector3.left * beamSpeed;
                    beam.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                    break;
            }
            yield return new WaitForSeconds(0.416f); // Timing partie 2
            PlayerManager.Instance.currentController.spellMoveSpeedModifier = 1f;
        }
    }

}
