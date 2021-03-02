using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class MouchouAnorexieBehaviour : MonoBehaviour
{
    public MouchouBaseMovement mBm;
    EnemyStats stats;

    public GameObject vomito;


    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponentInParent<EnemyStats>();

        stats.attackClock.ClockEnded += OnShouldAttack;
    }

    void OnShouldAttack()
    {
        mBm.canMove = false;
        StartCoroutine(SpitBehaviour());
    }

    IEnumerator SpitBehaviour ()
    {
        mBm.GetNextNode();
        mBm.UpdateMovement();
        Instantiate(vomito, mBm.currentNode, Quaternion.identity);
        yield return new WaitUntil(() => mBm.isMoving ==  true);
        mBm.canMove = true;
    }
}
