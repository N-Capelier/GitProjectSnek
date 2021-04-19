using UnityEngine;

namespace Player
{
    public class PlayerRunFeebacks : MonoBehaviour
    {
        public GameObject left, right, top, down;
        Animator animLeft, animRight, animTop, animDown;

        private void Start()
        {
            animLeft = left.GetComponent<Animator>();
            animRight = right.GetComponent<Animator>();
            animTop = top.GetComponent<Animator>();
            animDown = down.GetComponent<Animator>();
        }

        public void PlayAnimLeft()
        {
            animTop.Play("SwipeLeft");
            animDown.Play("SwipeLeft");
        }

        public void PlayAnimRight()
        {
            animTop.Play("SwipeRight");
            animDown.Play("SwipeRight");
        }

        public void PlayAnimDown()
        {
            animLeft.Play("SwipeLeft");
            animRight.Play("SwipeLeft");
        }

        public void PlayAnimUp()
        {
            animLeft.Play("SwipeRight");
            animRight.Play("SwipeRight");
        }
    }
}
