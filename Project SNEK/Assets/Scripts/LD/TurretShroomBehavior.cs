using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Controller;
using Player;

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
		[SerializeField] private float bulletSpeed = 10;
		[Range(1f, 10f)]
		[SerializeField] private float shootingDelay;
		[Range(.5f, 4f)]
		[SerializeField] private float warmupTime; //must be lower than delay
		[SerializeField] private float activationRange = 10;
		[SerializeField] private float activationDelay;
		private Transform playerTransform;
		private Vector3 playerDistance;
		[SerializeField] Animator animator;
		[SerializeField] ParticleSystem windUp, shot;
		#endregion

		// Start is called before the first frame update
		void Start()
		{
			StartCoroutine(GetPlayer());

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
		}

		IEnumerator ShootingLoop()
        {
			if (playerTransform == null)
				yield break;

			playerDistance = playerTransform.position - transform.position;

			if(playerDistance.z > activationRange)
            {
				yield return new WaitForSeconds(0.1f);
            }
            else
            {
				yield return new WaitForSeconds(shootingDelay);

				windUp.Play();
				yield return new WaitForSeconds(warmupTime);
				windUp.Stop();
				animator.Play("animEyedFlowerShot");
				shot.Play();
				incomingBullet = Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
				incomingBullet.GetComponent<Rigidbody>().velocity = directionVector * bulletSpeed;
			}
			
			StartCoroutine(ShootingLoop());
		}

		IEnumerator GetPlayer()
        {
			yield return new WaitWhile(() => PlayerManager.Instance.currentController == null);

			while (playerTransform == null)
            {
				playerTransform = PlayerManager.Instance.currentController.transform;
			}
			animator.Play("animEyedFlowerOut");
			yield return new WaitForSeconds(activationDelay);
			StartCoroutine(ShootingLoop());
		}
	}
}

