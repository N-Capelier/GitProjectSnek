using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialToolTEST : MonoBehaviour
{
    [SerializeField] GameObject tuto;
    private void Start()
    {
        tuto.transform.localScale = Vector3.zero;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
        {
            tuto.LeanScale(Vector3.one, 0.2f).setIgnoreTimeScale(true);
            Time.timeScale = 0f;
        }
    }

    public void CloseTutoBox()
    {
        tuto.LeanScale(Vector3.zero, 0.2f).setIgnoreTimeScale(true);
        Time.timeScale = 1f;
    }
}
