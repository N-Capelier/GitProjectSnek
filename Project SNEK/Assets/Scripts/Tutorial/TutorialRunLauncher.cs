using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tutorial
{
    public class TutorialRunLauncher : MonoBehaviour
    {

        public void StartTutoRun()
        {
            SceneManager.LoadScene("TutorialMap");
        }

    }
}