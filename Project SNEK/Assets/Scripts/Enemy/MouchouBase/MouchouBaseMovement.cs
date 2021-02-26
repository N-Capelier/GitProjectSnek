using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Map;


public class MouchouBaseMovement : MonoBehaviour
{
    public EnemyStats stats;
    Rigidbody rb;

    public Vector3 oui;


    void Start()
    {
        stats = GetComponentInParent<EnemyStats>();
        rb = GetComponentInParent<Rigidbody>();

        stats.movementClock.ClockEnded += OnShouldMove;
    }


    void Update()
    {

    }

    void OnShouldMove()
    {
        rb.velocity = oui;
        print("Allo");
    }
}
