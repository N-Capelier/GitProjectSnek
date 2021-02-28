using Cinemachine;
using UnityEngine;

namespace Rendering.Run
{
    public enum CamState
    {
        FreeScrolling,
        PlayerScrolling,
        Boss
    }

    /// <summary>
    /// Nico
    /// </summary>
    public class RunCamController : Singleton<RunCamController>
    {
        public CinemachineVirtualCamera vcam;
        Animator animator;
        public CamState ActiveState { get; private set; }
        [SerializeField] CamState startingState;
        [Range(0f, 2f)] public float scrollSpeed;
        [HideInInspector] public Rigidbody rb;

        private void Awake()
        {
            CreateSingleton();

            animator = GetComponent<Animator>();
            rb = vcam.gameObject.GetComponent<Rigidbody>();

            Set(startingState);
        }

        public void Set(CamState newState)
        {
            if(ActiveState != newState)
            {
                animator.Play(newState.ToString());
                ActiveState = newState;
            }
        }
    }
}