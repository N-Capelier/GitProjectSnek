﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player.Attack;
using Rendering.Run;

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

        public Vector3 startingNode = new Vector3(5, 0, 0);
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
        public Vector3 respawnNode;

        public GameObject objectRenderer;

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
            objectRenderer.GetComponent<Animator>().Play("Anim_PlayerRun_death");
            yield return new WaitForSeconds(0.5f);
            PlayerManager.Instance.gameObject.SetActive(false);
            /*AsyncOperation _loadingScene = */SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //yield return new WaitUntil(() => _loadingScene.isDone);
            PlayerManager.Instance.gameObject.SetActive(true);
            transform.position = checkPoint.position;
            RunCamController.Instance.Set(CamState.PlayerScrolling, true);
            //play respawn anim

        }

        IEnumerator DeathCoroutine()
        {
            //play defeat anim
            objectRenderer.GetComponent<Animator>().Play("Anim_PlayerRun_death");
            yield return new WaitForSeconds(1f);
            GameManagement.GameManager.Instance.gameState.Set(GameManagement.GameState.Hub, "Hub");
        }
    }
}