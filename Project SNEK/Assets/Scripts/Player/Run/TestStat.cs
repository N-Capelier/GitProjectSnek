using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStat : MonoBehaviour
{
    public int Hp = 10;
    
    public void TakeDamage(int damage)
    {
        Hp -= damage;
        Debug.Log("AIe, j'ai plus que " + Hp + "point de vie");
        if(Hp <= 0)
        {
            Destroy(gameObject);
        }
    }

}
