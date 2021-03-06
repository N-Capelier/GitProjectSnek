﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wall;

public class WallTimed : WallBehaviour
{
    public bool deadByTime;

    [Range(0, 5)] public float timeToDeath;

    // Start is called before the first frame update
    void Start()
    {
        if(deadByTime == true)
        {
            StartCoroutine(DeathByTime(timeToDeath));
        }
        
    }

    IEnumerator DeathByTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}