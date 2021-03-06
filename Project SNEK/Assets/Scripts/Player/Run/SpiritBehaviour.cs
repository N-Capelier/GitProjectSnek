﻿using System.Collections;
using UnityEngine;
using Player.Controller;

namespace Player.Spirits
{
    public class SpiritBehaviour : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;
        public GameObject objectRenderer;
        PlayerDirection currentDir;

       [SerializeField] SpiritBehaviour nextSpirit;

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
                    break;
                case PlayerDirection.Right:
                    rb.velocity = Vector3.right * _speedModifier;
                    break;
                case PlayerDirection.Down:
                    rb.velocity = Vector3.back * _speedModifier;
                    break;
                case PlayerDirection.Left:
                    rb.velocity = Vector3.left * _speedModifier;
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


            yield return new WaitForSeconds(1f);
            objectRenderer.SetActive(false);
        }

    }
}