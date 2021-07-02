using System.Collections;
using UnityEngine;
using Player.Controller;

namespace Player.Spirits
{
    public class SpiritBehaviour : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;
        public GameObject objectRenderer;
        PlayerDirection currentDir;
        [SerializeField] Animator animator;

        [SerializeField] SpiritBehaviour nextSpirit;
        [SerializeField] ParticleSystem poof;

        private void Start()
        {
            rb.velocity = Vector3.forward * PlayerManager.Instance.currentController.moveSpeed * PlayerManager.Instance.currentController.attackMoveSpeedModifier;
        }


        private void FixedUpdate()
        {
            rb.velocity = rb.velocity.normalized * PlayerManager.Instance.currentController.rb.velocity.magnitude;
        }

        //public void UpdateSpeed()
        //{
        //    if (PlayerManager.Instance.currentController.rb.velocity.magnitude == 0)
        //    {
        //        rb.velocity = Vector3.zero;
        //    }
        //    else
        //    {
        //        rb.velocity = rb.velocity.normalized * PlayerManager.Instance.currentController.rb.velocity.magnitude;
        //    }
        //}

        public void SetDirection(PlayerDirection _direction)
        {
            PlayerDirection _foo = currentDir;

            currentDir = _direction;
            float _speedModifier = PlayerManager.Instance.currentController.moveSpeed * PlayerManager.Instance.currentController.attackMoveSpeedModifier;

            switch (_direction)
            {
                case PlayerDirection.Up:
                    rb.velocity = Vector3.forward * _speedModifier;
                    objectRenderer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case PlayerDirection.Right:
                    rb.velocity = Vector3.right * _speedModifier;
                    objectRenderer.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    break;
                case PlayerDirection.Down:
                    rb.velocity = Vector3.back * _speedModifier;
                    objectRenderer.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    break;
                case PlayerDirection.Left:
                    rb.velocity = Vector3.left * _speedModifier;
                    objectRenderer.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                    break;
                default:
                    break;
            }

            SnapPosition();

            if (nextSpirit != null)
                nextSpirit.SetDirection(_foo);
        }

        void SnapPosition()
        {
            transform.position = new Vector3(
                Mathf.RoundToInt(transform.position.x),
                transform.position.y,
                Mathf.RoundToInt(transform.position.z)
                );
        }

        public void SendDeathInfo()
        {
            PlayerManager.Instance.currentController.playerRunSpirits.CutChain(this);
        }

        public IEnumerator Death()
        {
            //Play death anim
            animator.Play(Animator.StringToHash("animSpiritDisappear"));
            yield return new WaitForSeconds(0.66f);
            if(objectRenderer.activeSelf == true)
            Instantiate(poof, transform);
            //Prevenir Nico quand on change la durée
            yield return new WaitForSeconds(0.44f);
            objectRenderer.SetActive(false);
        }

    }
}