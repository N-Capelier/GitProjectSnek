using System.Collections;
using UnityEngine;
using Saving;
using AudioManagement;


public class PickUpObject : MonoBehaviour
{
    /// <summary>
    /// iD name is "secretObject_iDNumber"
    /// </summary>
    public string iD;
    public GameObject render;

    //public GameObject fX;

    private void Start()
    {
        //Check if already picked
        CheckIfPicked();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
        {
            AddToSave();
            StartCoroutine(Depop());
        }
    }

    IEnumerator Depop()
    {
        render.SetActive(false);
        AudioManager.Instance.PlaySoundEffect("ObjectSecretItemCollect");
        //Instantiate Fx
        yield return new WaitForSeconds(0.5f);        
        Destroy(gameObject);
    }

    void CheckIfPicked()
    {
        if (iD == SaveManager.Instance.state.secretObject_1.ToString())
        {
            switch (SaveManager.Instance.state.secretObject_1)
            {
                case 0:
                    break;
                case 1:
                    Destroy(gameObject);
                    break;
                case 2:
                    Destroy(gameObject);
                    break;
            }
        }
        else if (iD == SaveManager.Instance.state.secretObject_2.ToString())
        {
            switch (SaveManager.Instance.state.secretObject_2)
            {
                case 0:
                    break;
                case 1:
                    Destroy(gameObject);
                    break;
                case 2:
                    Destroy(gameObject);
                    break;
            }
        }
        else if (iD == SaveManager.Instance.state.secretObject_3.ToString())
        {
            switch (SaveManager.Instance.state.secretObject_3)
            {
                case 0:
                    break;
                case 1:
                    Destroy(gameObject);
                    break;
                case 2:
                    Destroy(gameObject);
                    break;
            }
        }
        else
        {
            Debug.Log("secret object iD name is wrong");
        }
    }

    void AddToSave()
    {
        if (iD == SaveManager.Instance.state.secretObject_1.ToString())
        {
            SaveManager.Instance.state.secretObject_1 = 1;
        }
        else if (iD == SaveManager.Instance.state.secretObject_2.ToString())
        {
            SaveManager.Instance.state.secretObject_2 = 1;
        }
        else if (iD == SaveManager.Instance.state.secretObject_3.ToString())
        {
            SaveManager.Instance.state.secretObject_3 = 1;
        }
        else
        {
            Debug.Log("secret object iD name is wrong");
        }
    }
}
