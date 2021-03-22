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
        [SerializeField] float beamLifeTime, beamSpeed;

        public override IEnumerator TechniqueCast(Controller.PlayerDirection techniqueDirection)
        {

            
                PlayerManager.Instance.currentController.spellMoveSpeedModifier = 0.01f;
                yield return new WaitForSeconds(0.5f);
                PlayerManager.Instance.currentController.spellMoveSpeedModifier = 1f;
                beam = Instantiate(beamPrefab, transform.position, Quaternion.identity);
                switch (techniqueDirection)
                {
                    case Controller.PlayerDirection.Up:
                        beam.GetComponent<Rigidbody>().velocity = Vector3.forward * beamSpeed;
                        break;
                    case Controller.PlayerDirection.Down:
                        beam.GetComponent<Rigidbody>().velocity = Vector3.back * beamSpeed;
                        break;
                    case Controller.PlayerDirection.Right:
                        beam.GetComponent<Rigidbody>().velocity = Vector3.right * beamSpeed;
                        break;
                    case Controller.PlayerDirection.Left:
                        beam.GetComponent<Rigidbody>().velocity = Vector3.left* beamSpeed;
                        break;
                }
                yield return new WaitForSeconds(beamLifeTime);
                if(beam != null)
                {
                    beam.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    StartCoroutine(beam.GetComponent<SworBeamBehaviour>().DestroyBeam());
                }
            

        }
    }

}
