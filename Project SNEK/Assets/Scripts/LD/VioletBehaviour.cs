﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wall;

public class VioletBehaviour : MonoBehaviour
{
    public GameObject poisonSmoke;
    public ParticleSystem poisonFx;
    public float poisonSpawnTime, poisonDuration, Random1, Random2;

    void Start()
    {
        poisonSmoke.SetActive(false);
            StartCoroutine(PoisonSpawning(poisonSpawnTime, poisonDuration));
    }

    IEnumerator PoisonSpawning(float SpawnTime, float Duration)
    {
        poisonFx.Play();
        yield return new WaitForSeconds(SpawnTime);
        poisonSmoke.SetActive(true);
        poisonFx.Stop();
        yield return new WaitForSeconds(poisonDuration - 0.5f);
        poisonSmoke.gameObject.transform.GetComponentInChildren<ParticleSystem>().Stop();
        yield return new WaitForSeconds(0.5f);
        poisonSmoke.SetActive(false);
        StartCoroutine(RecoverPoison(Random.Range(Random1,Random2)));
    }

    IEnumerator RecoverPoison(float timeBeforeNewPoison)
    {
        yield return new WaitForSeconds(timeBeforeNewPoison);
        StartCoroutine(PoisonSpawning(poisonSpawnTime, poisonDuration));
    }
}