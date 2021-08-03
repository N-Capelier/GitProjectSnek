using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Controller;
using Player;

namespace SpecialLD
{
	public class BoulderBehavior : MonoBehaviour
	{
		#region Variables
		PlayerDirection direction;
        private Vector3 directionVector;

		[SerializeField] float speed;
		[SerializeField] Rigidbody rb;
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Attack"))
            {
                direction = PlayerManager.Instance.currentController.currentDirection;
				
				switch (direction)
				{
					case PlayerDirection.Up:
						directionVector = Vector3.forward;
						break;
					case PlayerDirection.Down:
						directionVector = -Vector3.forward;
						break;
					case PlayerDirection.Left:
						directionVector = -Vector3.right;
						break;
					case PlayerDirection.Right:
						directionVector = Vector3.right;
						break;
				}

				rb.velocity = directionVector * speed;
			}
            else if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
				rb.velocity = Vector3.zero;

                switch (direction)
                {
					case PlayerDirection.Up:
					case PlayerDirection.Down:
						transform.position = new Vector3(transform.position.x, 0, other.transform.position.z - directionVector.z);
						break;

					case PlayerDirection.Left:
					case PlayerDirection.Right:
						transform.position = new Vector3(other.transform.position.x - directionVector.x, 0, transform.position.z);
						break;
				}
            }
        }
    }
}

