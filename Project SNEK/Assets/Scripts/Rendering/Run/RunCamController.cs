using Cinemachine;
using UnityEngine;


using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;

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
        public CinemachineBrain brain;
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

        [Header("Camera Offset")]
        public float yOffset;
        public float zOffset;


        private void Awake()
        {
            CreateSingleton();

            animator = GetComponent<Animator>();
            rb = vcam.gameObject.GetComponent<Rigidbody>();

            //Set(startingState, true);

        }

        private void Start()
        {
            if(SceneManager.GetActiveScene().name == "Boss Anorexia")
                Set(CamState.SemiScrolling, true);
        }

        public void Set(CamState newState, bool forceState = false)
        {
            if(ActiveState != newState || forceState == true)
            {
                animator.Play(Animator.StringToHash(newState.ToString()));
                ActiveState = newState;
            }
        }

        public void EnableVCAM()
        {
            StartCoroutine(EnableVCAMRoutine());
        }

        private IEnumerator EnableVCAMRoutine()
        {
            yield return null;
            vcam.enabled = true;
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