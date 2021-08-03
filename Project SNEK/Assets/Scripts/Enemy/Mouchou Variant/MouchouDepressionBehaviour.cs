using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class MouchouDepressionBehaviour : MonoBehaviour
    {

        [SerializeField] EnemyStats stats;

        [Header("Intangible behaviour")]
        [SerializeField] MeshRenderer maskRenderer;
        [SerializeField] Material intangibleMaterial;
        [SerializeField] Material defaultMaterial;
        Coroutine intangibleCoroutine;

        void Start()
        {
            stats.currentHp = 10000;
            defaultMaterial = maskRenderer.material;
        }

        void Update()
        {
            CheckIntangible();
        }

        public void CheckIntangible()
        {
            if(maskRenderer.material != defaultMaterial && intangibleCoroutine == null)
            {
                intangibleCoroutine = StartCoroutine(IntangibleCoroutine());
            }
        }

        private IEnumerator IntangibleCoroutine()
        {
            stats.currentHp = 1;
            yield return new WaitUntil(() => maskRenderer.material.name == defaultMaterial.name);
            stats.currentHp = 10000;
            yield return null;
        }
    }

}

