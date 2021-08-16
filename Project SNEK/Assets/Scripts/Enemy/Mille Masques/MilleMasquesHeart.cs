using AudioManagement;
using Enemy;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilleMasquesHeart : MonoBehaviour
{
    [HideInInspector] public MilleMasquesBehaviour behaviour;
    [SerializeField] GameObject objectRenderer;
    [SerializeField] ParticleSystem fx;
    [SerializeField] string soundName;
    [SerializeField] LineRenderer binding;

    public int deathIndex;
    protected virtual void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
        {
            PlayerManager.Instance.currentController.Death(deathIndex);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Attack"))
        {
            StartCoroutine(GetDestroyed());
        }
    }

    public IEnumerator GetDestroyed()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        objectRenderer.SetActive(false);
        binding.enabled = false;
        behaviour.hearts.Remove(this);

        if (fx != null)
        {
            fx.Play();
            AudioManager.Instance.PlaySoundEffect(soundName);
            yield return new WaitForSeconds(fx.main.duration);
        }
        Destroy(gameObject);
    }

    public void Update()
    {
        binding.SetPosition(1, behaviour.head.transform.position + new Vector3(0, 1, 0));
    }
}
