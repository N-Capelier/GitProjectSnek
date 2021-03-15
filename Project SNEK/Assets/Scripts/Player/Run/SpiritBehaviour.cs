using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Spirits
{
    public class SpiritBehaviour : MonoBehaviour
    {

        public IEnumerator Death()
        {
            //Play death anim


            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

    }
}