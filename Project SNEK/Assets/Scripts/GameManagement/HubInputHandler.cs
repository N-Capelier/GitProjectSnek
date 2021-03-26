using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hub.Interaction;

namespace GameManagement
{
    public class HubInputHandler : MonoBehaviour
    {
        [SerializeField] Camera hubCamera;

        private void Start()
        {
            //InputHandler.InputReceived += OnTap;
        }

        private void OnDestroy()
        {
            //InputHandler.InputReceived -= OnTap;
        }

        private void Update()
        {
            OnTap();
        }

        void OnTap(/*InputType _input*/)
        {
            //if(_input == InputType.Tap)
            //{
                RaycastHit hit;

            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = hubCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<Interactor>())
                    {
                        StartCoroutine(hit.collider.gameObject.GetComponent<Interactor>().BeginInteraction());
                    }
                }
            }
            //}
        }
    }
}