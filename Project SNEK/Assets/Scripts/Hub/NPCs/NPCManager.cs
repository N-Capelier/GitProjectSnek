using UnityEngine;
using Player;

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

        public Animator anaelAnim
        {
            get
            {
                return PlayerManager.Instance.currentController.animator;
            }
        }

        private void Awake()
        {
            CreateSingleton();
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