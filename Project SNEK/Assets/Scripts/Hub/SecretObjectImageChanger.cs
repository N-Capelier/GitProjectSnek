using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Saving;

public class SecretObjectImageChanger : MonoBehaviour
{
    public int iD;
    public Sprite spriteToChange;

    private void Start()
    {
        switch (iD)
        {
            case 0:
                return;
            case 1:
                if(SaveManager.Instance.state.secretObject_1 >= 1)
                {
                    GetComponent<Image>().sprite = spriteToChange;
                }
                return;
            case 2:
                if (SaveManager.Instance.state.secretObject_2 >= 1)
                {
                    GetComponent<Image>().sprite = spriteToChange;
                }
                return;
            case 3:
                if (SaveManager.Instance.state.secretObject_3 >= 1)
                {
                    GetComponent<Image>().sprite = spriteToChange;
                }
                return;
        }
    }

    private void Update()
    {
        if(GetComponent<Image>().sprite != spriteToChange)
        {
            switch (iD)
            {
                case 0:
                    return;
                case 1:
                    if (SaveManager.Instance.state.secretObject_1 >= 1)
                    {
                        GetComponent<Image>().sprite = spriteToChange;
                    }
                    return;
                case 2:
                    if (SaveManager.Instance.state.secretObject_2 >= 1)
                    {
                        GetComponent<Image>().sprite = spriteToChange;
                    }
                    return;
                case 3:
                    if (SaveManager.Instance.state.secretObject_3 >= 1)
                    {
                        GetComponent<Image>().sprite = spriteToChange;
                    }
                    return;
            }
        }        
    }
}
