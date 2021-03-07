using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class GloutonBehaviour : MonoBehaviour
{
    EnemyStats stats;
    public Animator anim;

    public int targetNumber;
    public GameObject targetMarker;

    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponentInParent<EnemyStats>();
        stats.attackClock.ClockEnded += OnShouldAttack;
    }

    void OnShouldAttack()
    {
        anim.SetBool("isAttacking", true);
        StartCoroutine(LaunchProjectile());
    }

    IEnumerator LaunchProjectile()
    {
        yield return new WaitForSeconds(1);
        TargetCell();
        yield return new WaitForSeconds(2);
        anim.SetBool("isAttacking", false);

    }

    void TargetCell()
    {
        for (int i = 0; i < targetNumber; i++)
        {
            targetPos = new Vector3(transform.position.x - Random.Range(-3,3), 0, transform.position.z - Random.Range(1,5));
            Instantiate(targetMarker, targetPos, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        stats.attackClock.ClockEnded -= OnShouldAttack;
    }

}
