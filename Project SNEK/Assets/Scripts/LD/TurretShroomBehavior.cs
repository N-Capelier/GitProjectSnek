using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Controller;

namespace SpecialLD
{
	public class TurretShroomBehavior : MonoBehaviour
	{
		#region Variables
		public GameObject bullet;
		private GameObject incomingBullet;
		public Transform bulletSpawn;
		//public ParticleSystem warmupFx;
		[SerializeField] private PlayerDirection direction;
		private Vector3 directionVector;
		[SerializeField] private float bulletSpeed;
		[Range(1f, 10f)]
		[SerializeField] private float shootingDelay;
		[Range(.5f, 4f)]
		[SerializeField] private float warmupTime; //must be lower than delay
		#endregion

		// Start is called before the first frame update
		void Start()
		{
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

			StartCoroutine(ShootingLoop());
		}

		IEnumerator ShootingLoop()
        {
			yield return new WaitForSeconds(shootingDelay);

			//warmupFx.Play();
			yield return new WaitForSeconds(warmupTime);
			//warmupFx.Stop();
			incomingBullet = Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
			incomingBullet.GetComponent<Rigidbody>().velocity = directionVector * bulletSpeed;

			StartCoroutine(ShootingLoop());
		}
	}
}

