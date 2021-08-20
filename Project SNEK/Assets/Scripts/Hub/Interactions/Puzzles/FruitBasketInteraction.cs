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
        public string winSoundEffectName;
        public string defeatSoundEffectName;
        public List<FruitInteraction> fruitsList = new List<FruitInteraction>();
        public List<GameObject> fruitsInBasket = new List<GameObject>();
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

            if(miniGameCoroutine == null)//+ check save state
            {
                clockCoroutine = StartCoroutine(Clock());
                miniGameCoroutine = StartCoroutine(MiniGameCoroutine());
                for (int i = 0; i < fruitsList.Count; i++)
                {
                    fruitsList[i].gameObject.SetActive(true);
                }
                for (int i = 0; i < fruitsInBasket.Count; i++)
                {
                    fruitsInBasket[i].SetActive(false);
                }
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

            AudioManager.Instance.PlaySoundEffect(winSoundEffectName);//+ give reward

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
            for(int i = 0; i < fruitsInBasket.Count; i++)
            {
                fruitsInBasket[i].SetActive(true);
            }
            AudioManager.Instance.PlaySoundEffect(defeatSoundEffectName);
            clockCoroutine = null;
        }
    }
}
