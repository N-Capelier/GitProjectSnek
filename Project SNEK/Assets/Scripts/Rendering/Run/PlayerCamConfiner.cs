using UnityEngine;

namespace Rendering.Run
{
    public class PlayerCamConfiner : MonoBehaviour
    {
		Vector3 confinerBaseSize;
		[Tooltip("Default: 4")]
		[SerializeField] float confinerLargeSizeWidth;

		private void Start()
		{
			confinerBaseSize = RunCamController.Instance.confinerCollider.size;
		}

		private void OnTriggerEnter(Collider other)
		{
			if(other.CompareTag("ConfinerSizeUp"))
			{
				print("size up");
				RunCamController.Instance.confinerCollider.size = new Vector3(confinerLargeSizeWidth, confinerBaseSize.y, confinerBaseSize.z);
			}
			else if(other.CompareTag("ConfinerSizeDown"))
			{
				print("size down");
				RunCamController.Instance.confinerCollider.size = confinerBaseSize;
			}
		}

	}
}