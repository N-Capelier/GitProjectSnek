using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    public class TransitionScreenManager : MonoBehaviour
    {

        public Image image;

        public IEnumerator AlphaUp(float _speed, string _sceneName)
        {
            image.gameObject.SetActive(true);
            while(image.color.a < 1)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + _speed);

                yield return new WaitForEndOfFrame();
            }
            SceneManager.LoadScene(_sceneName);
        }

        public IEnumerator AlphaDown(float _speed)
        {
            while (image.color.a > 0)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - _speed);

                yield return new WaitForEndOfFrame();
            }
            image.gameObject.SetActive(false);
        }

    }
}