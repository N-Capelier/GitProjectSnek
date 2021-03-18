using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Spells
{
    /// <summary>
    /// Coco
    /// </summary>
    public class EntityGrabber : MonoBehaviour
    {
        public IEnumerator MoveTowardBomb(Vector3 bombPosition, float absorbSpeed)
        {

            while (Vector3.Distance(transform.position, bombPosition) >= 0.2f)
            {
                Vector3 direction = bombPosition - transform.position;
                transform.position += direction * Time.fixedDeltaTime * absorbSpeed;
                transform.Rotate(0, 30, 0);
                yield return new WaitForFixedUpdate();
            }
            Destroy(gameObject);
            yield break;
        }
    }
}

