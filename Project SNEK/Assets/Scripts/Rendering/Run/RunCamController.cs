using Cinemachine;
using UnityEngine;

namespace Rendering.Run
{
    public enum CamState
    {
        PlayerScrolling,
        FreeScrolling,
        Fixed,
        SemiScrolling
    }

    /// <summary>
    /// Nico
    /// </summary>
    public class RunCamController : Singleton<RunCamController>
    {
        public Camera cam;
        public CinemachineVirtualCamera vcam;
        Animator animator;
        public CamState ActiveState { get; private set; }
        [SerializeField] CamState stateAutoApply;
        [Range(0f, 2f)] public float scrollSpeed;
        [HideInInspector] public Rigidbody rb;
        public GameObject deathZone;
        [SerializeField] bool applyState;

        [Header("Feedback")]
        public ParticleSystem[] feedbackObjects;
        [HideInInspector] public bool playing = false;
        [HideInInspector] public int index;


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
                animator.Play(Animator.StringToHash(newState.ToString()));
                ActiveState = newState;
            }
        }

#if UNITY_EDITOR

        private void Update()
        {
            //if (applyState)
            //{
            //    applyState = false;
            //    Set(stateAutoApply);
            //}
            if(Input.GetKey(KeyCode.D))
                Debug.Log(ActiveState.ToString());
        }

#endif
    }
}