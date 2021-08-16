using UnityEngine;
using Player;
using System.Collections.Generic;
using DialogueManagement;
using System.Collections;

namespace Saving
{
    public class NPCManager : Singleton<NPCManager>
    {
        public BergamotHUB bergamot;
        public PoppyHUB poppy;
        public ThistleHUB thistle;
        [Space]
        public Animator bergamotHubAnim;
        public Animator poppyHubAnim, thistleHubAnim;
        [Space]
        public Animator anaelCutscene;
        public Animator bergamotCutscene, poppyCutscene, thistleCutscene, anaelRunCutscene;
        [Space]
        public Material darkBergamotMat;
        public Material darkPoppyMat;
        public Material darkThistleMat;

        public Animator anaelAnim
        {
            get
            {
                if(PlayerManager.Instance != null)
                {
                    if (PlayerManager.Instance.currentController is null)
                    {
                        return null;
                    }
                    return PlayerManager.Instance.currentController.animator;
                }

                return null;
            }
        }

        public static Dictionary<CharacterAnimator, Animator> characterAnimatorDictionary = new Dictionary<CharacterAnimator, Animator>();


        private void Awake()
        {
            CreateSingleton();
        }

        private IEnumerator Start()
        {
            if(characterAnimatorDictionary.ContainsKey(CharacterAnimator.None))
            {
                yield break;
            }

            yield return new WaitForSeconds(.5f);
            characterAnimatorDictionary.Add(CharacterAnimator.None, null);

            if(anaelAnim != null)
            {
                characterAnimatorDictionary.Add(CharacterAnimator.AnaelController, anaelAnim);
            }
            characterAnimatorDictionary.Add(CharacterAnimator.AnaelCutscene, anaelCutscene);
            characterAnimatorDictionary.Add(CharacterAnimator.AnaelRunCutscene, anaelRunCutscene);

            characterAnimatorDictionary.Add(CharacterAnimator.BergamotHub, bergamotHubAnim);
            characterAnimatorDictionary.Add(CharacterAnimator.BergamotCutscene, bergamotCutscene);

            characterAnimatorDictionary.Add(CharacterAnimator.PoppyHub, poppyHubAnim);
            characterAnimatorDictionary.Add(CharacterAnimator.PoppyCutscene, poppyCutscene);

            characterAnimatorDictionary.Add(CharacterAnimator.ThistleHub, thistleHubAnim);
            characterAnimatorDictionary.Add(CharacterAnimator.ThistleCutscene, thistleCutscene);
        }

        public void RefreshNPCs()
        {
            bergamot.Refresh();
            poppy.Refresh();
            thistle.Refresh();
        }

        //public void SetStartedNPCs()
        //{

        //}
    }
}