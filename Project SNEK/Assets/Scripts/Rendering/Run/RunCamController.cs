using Cinemachine;
using UnityEngine;

namespace Rendering.Run
{
    public enum CamState
    {
        FreeScrolling,
        PlayerScrolling,
        Fixed,
        SemiScrolling
    }

    /// <summary>
    /// Nico
    /// </summary>
    public class RunCamController : Singleton<RunCamController>
    {
        public CinemachineVirtualCamera vcam;
        Animator animator;
        public CamState ActiveState { get; private set; }
        [SerializeField] CamState stateAutoApply;
        [Range(0f, 2f)] public float scrollSpeed;
        [HideInInspector] public Rigidbody rb;
        public GameObject deathZone;
        [SerializeField] bool applyState;

        private void Awake()
        {
            CreateSingleton();

            animator = GetComponent<Animator>();
            rb = vcam.gameObject.GetComponent<Rigidbody>();

            //Set(startingState, true);
        }

        public void Set(CamState newState, bool forceState = false)
        {
            if(ActiveState != newState || forceState == true)
            {
                animator.Play(newState.ToString());
                ActiveState = newState;
            }
        }

#if UNITY_EDITOR

        private void Update()
        {
            if (applyState)
            {
                applyState = false;
                Set(stateAutoApply);
            }
        }

#endif
    }
}