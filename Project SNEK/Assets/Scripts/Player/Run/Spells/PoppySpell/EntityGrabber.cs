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
        
        public IEnumerator MoveTowardBomb(Vector3 bombPosition, float absorbSpeed, ParticleSystem fx)
        {
            float progress = 0;
            float offset = 1.3f;
            gameObject.GetComponent<Collider>().enabled = false;
            while (Vector3.Distance(transform.position, bombPosition + Vector3.up * offset) >= 0.2f)
            {
                Vector3 direction = (bombPosition + Vector3.up * offset) - transform.position;
                progress += Time.deltaTime;
                transform.position += direction * Time.fixedDeltaTime * absorbSpeed * progress;
                transform.Rotate(0, 30, 0);
                yield return new WaitForFixedUpdate();
            }
            Instantiate(fx, transform.position, Quaternion.identity);
            Destroy(gameObject);
            yield break;
        }
    }
}

