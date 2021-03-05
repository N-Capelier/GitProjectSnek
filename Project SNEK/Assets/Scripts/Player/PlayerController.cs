using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player.Attack;

namespace Player.Controller
{
    public enum PlayerDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    /// <summary>
    /// Nico
    /// </summary>
    public abstract class PlayerController : MonoBehaviour
    {
        [HideInInspector] public PlayerRunAttack playerRunAttack;

        public Vector3 startingNode = new Vector3(3, 0, 1);
        [HideInInspector] public PlayerDirection currentDirection;
        [HideInInspector] public PlayerDirection nextDirection;

        [Space]
        [HideInInspector] public Rigidbody rb = null;
        [Range(0, 4)] public float moveSpeed = 50;

        [Space]
        [SerializeField] [Range(0, 10)] int baseHP = 4;
        int currentHP;
        public bool canMove = true;
        public Transform checkPoint;


        public virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Init(int _bonusHP)
        {
            currentHP = baseHP + _bonusHP;
        }

        public void Death()
        {
            currentHP--;
            if(currentHP <= 0)
            {
                StartCoroutine(DeathCoroutine());
            }
            else
            {
                StartCoroutine(RespawnCoroutine());
            }
        }

        IEnumerator RespawnCoroutine()
        {
            //play death anim

            //yield return new WaitForSeconds(0.5f);
            AsyncOperation _loadingScene = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            transform.position = checkPoint.position;
            yield return new WaitUntil(() => _loadingScene.isDone);
            //play respawn anim
        }

        IEnumerator DeathCoroutine()
        {
            //play defeat anim

            yield return new WaitForSeconds(0.5f);
            GameManagement.GameManager.Instance.gameState.Set(GameManagement.GameState.Hub, "Hub");
        }
    }
}