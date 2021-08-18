using AudioManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hub.Interaction
{
    public class FruitBasketInteraction : Interactor
    {
        public string animationName;
        public Animator objectAnimator;

        [Header("Fruits Mini-Game")]
        public string fruitBasketSoundEffectName;
        public List<FruitInteraction> fruitsList = new List<FruitInteraction>();
        public float miniGameLength;
        private Coroutine miniGameCoroutine;
        private Coroutine clockCoroutine;

        public override IEnumerator BeginInteraction()
        {
            Interact();
            yield return null;
        }

        protected override void Interact()
        {
            objectAnimator.Play(Animator.StringToHash(animationName));
            AudioManager.Instance.PlaySoundEffect(fruitBasketSoundEffectName);

            clockCoroutine = StartCoroutine(Clock());
            miniGameCoroutine = StartCoroutine(MiniGameCoroutine());
            for (int i = 0; i < fruitsList.Count; i++)
            {
                fruitsList[i].gameObject.SetActive(true);
            }
    
        }

        private IEnumerator MiniGameCoroutine()
        {
            for (int i = 0; i < fruitsList.Count; i++)
            {
                yield return new WaitUntil(() => fruitsList[i].interacting);
                fruitsList[i].interacting = false;
            }

            if (clockCoroutine != null)
                StopCoroutine(clockCoroutine);

            clockCoroutine = null;
            miniGameCoroutine = null;
            print("Finished MiniGame");
        }

        IEnumerator Clock()
        {
            yield return new WaitForSeconds(miniGameLength);
            if (miniGameCoroutine != null)
            {
                StopCoroutine(miniGameCoroutine);
                miniGameCoroutine = null;
            }
            for (int i = 0; i < fruitsList.Count; i++)
            {
                fruitsList[i].gameObject.SetActive(false);
            }
            clockCoroutine = null;
        }
    }
}
