using System.Collections;
using UnityEngine;
using Hub.Interaction;
using Saving;
using UnityEngine.SceneManagement;

namespace Debugging
{
    public class StartLevelDebugInteraction : Interactor
    {
        [SerializeField] string sceneName;

        protected override void Interact()
        {
            print("INTERRRRACTION");
            SaveManager.Instance.Save();
            SceneManager.LoadScene(sceneName);
        }

        public override IEnumerator BeginInteraction()
        {

            Interact();
            yield return null;
        }
    }
}