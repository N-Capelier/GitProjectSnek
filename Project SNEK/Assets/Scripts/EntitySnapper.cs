using UnityEngine;

namespace LD
{
    public class EntitySnapper : MonoBehaviour
    {

        private void Start()
        {
            SnapPosition();
        }

        public void SnapPosition()
        {
            transform.position = new Vector3(
                Mathf.RoundToInt(transform.position.x),
                transform.position.y,
                Mathf.RoundToInt(transform.position.z)
                );
        }

    }
}