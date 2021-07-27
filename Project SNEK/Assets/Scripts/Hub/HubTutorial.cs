using GameManagement;
using Hub.UI;
using Player;
using Player.Controller;
using Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubTutorial : MonoBehaviour
{

    PlayerHubController playerHub;

    void Start()
    {
        if(SaveManager.Instance.state.isTutorialFinished && SaveManager.Instance.state.tutorialState != 0)
        {
            UpdateTutorialState(SaveManager.Instance.state.tutorialState);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTutorialState(int state)
    {
        SaveManager.Instance.state.tutorialState = state;
        switch(state)
        {
            case 0:
                //Afficher la box de tutoriel + "Try to find Bergamot in the village"
                GameManager.Instance.uiHandler.hubUI.OpenTutoBox(0);

                //Bergamot devient la target (flèche + feedback au dessus de bergamot)
                playerHub = (PlayerHubController)PlayerManager.Instance.currentController;

                playerHub.directionArrow.SetActive(true);
                playerHub.tutorlalTarget = NPCManager.Instance.bergamot.transform;

                //NPCManager.Instance.bergamot.exclamationMark.SetActive(true);
                

                //Quand le joueur est assez près de Bergamot passer au state suivant

                break;

            case 1:
                break;

            case 2:
                break;

            case 3:
                break;

            default:
                Debug.LogError("Wrong state input");
                break;
        }
    }

    public IEnumerator tutorialCase0()
    {
        yield return new WaitUntil(()=> (NPCManager.Instance.transform.position - playerHub.transform.position).magnitude < 2);
    }
}
