using System.Collections;
using UnityEngine;
using Saving;
using AudioManagement;
using CoinUI;

public class PickUpObject : MonoBehaviour
{
    /// <summary>
    /// iD name is "secretObject_iDNumber"
    /// </summary>
    public string iD;
    public GameObject render;

    public GameObject fX;

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
        Instantiate(fX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);        
        Destroy(gameObject);
    }

    void CheckIfPicked()
    {
        if (iD == nameof(SaveManager.Instance.state.secretObject_1))
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
        else if (iD == nameof(SaveManager.Instance.state.secretObject_2))
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
        else if (iD == nameof(SaveManager.Instance.state.secretObject_3))
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
            Debug.Log(iD);
            Debug.Log(nameof(SaveManager.Instance.state.secretObject_1));
        }
    }

    void AddToSave()
    {
        if (iD == nameof(SaveManager.Instance.state.secretObject_1))
        {            
            SaveManager.Instance.state.secretObject_1 = 1;
            SaveManager.Instance.state.heartCoinAmount++;
            CoinCountUI.Instance.UpdateCoinCount();

        }
        else if (iD == nameof(SaveManager.Instance.state.secretObject_2))
        {
            SaveManager.Instance.state.secretObject_2 = 1;
            SaveManager.Instance.state.heartCoinAmount++;
            CoinCountUI.Instance.UpdateCoinCount();
        }
        else if (iD == nameof(SaveManager.Instance.state.secretObject_3))
        {
            SaveManager.Instance.state.secretObject_3 = 1;
            SaveManager.Instance.state.heartCoinAmount++;
            CoinCountUI.Instance.UpdateCoinCount();
        }
        else
        {
            Debug.Log("secret object iD name is wrong to save");
        }
    }
}
