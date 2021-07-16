using SpecialLD;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wall
{
	public class TurretShroomBullet : WallTimed
	{
        #region Variables
        [SerializeField] private Rigidbody rb;
        #endregion

        // Start is called before the first frame update
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if(other.gameObject.layer == LayerMask.NameToLayer("LaBuBulle"))
            {
                Reflect();
            }

            if(other.gameObject.layer == LayerMask.NameToLayer("LePapaChampi"))
            {
                other.gameObject.GetComponent<TurretShroomBehavior>().GetDestroyed();
            }
        }

        private void Reflect()
        {
            rb.velocity = -rb.velocity;
        }
    }
}

