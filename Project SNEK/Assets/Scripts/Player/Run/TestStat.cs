using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStat : MonoBehaviour
{
    public float Hp = 1;
    
    public void TakeDamage(float damage)
    {
        Hp -= damage;
        Debug.Log("AIe, j'ai plus que " + Hp + "point de vie");
        if(Hp <= 0)
        {
            Destroy(gameObject);
        }
    }

}
