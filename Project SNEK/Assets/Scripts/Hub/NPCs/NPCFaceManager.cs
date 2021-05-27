using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceManager
{
    public class NPCFaceManager : MonoBehaviour
    {
        /// <summary>
        /// Coco
        /// </summary>

        [SerializeField] Material faceMat;
        [SerializeField] Vector2[] expressionEyesList;
        [SerializeField] Vector2[] randomMouthList;
        [SerializeField] Vector2[] expressionMouthList;

        public void RandomizeMouth()
        {
            faceMat.SetVector("Mouth", randomMouthList[Random.Range(0, randomMouthList.Length)]);
        }
        public void SetEyesExpression(int expression)
        {
            faceMat.SetVector("Eyes", expressionEyesList[expression]);

            ///Pour THISTLE :
            ///Int 1 = Neutral
            ///Int 2 = Surprise
            ///Int 3 = Blush
            ///Int 4 = Sad
            ///
            ///Pour BERGAMOT:
            ///Int 1 = Neutral
            ///Int 2 = Sad
            ///Int 3 = Empty/Calm/Upset
            ///Int 4 = Happy
            ///
            ///Pour POPPY :
            ///Int 1 = Neutral
            ///Int 2 = Sad
            ///Int 3 = Scared/Very sad/cry
            ///Int 4 = Happy
        }
        public void SetMouthExpression(int expression)
        {
            faceMat.SetVector("Mouth", expressionMouthList[expression]);

            ///Pour THISTLE :
            ///Int 1 = Neutral
            ///Int 2 = Happy
            ///Int 3 = Surprise
            ///Int 4 = Upset/Thinking
            ///
            ///Pour BERGAMOT :
            ///Int 1 = Neutral
            ///Int 2 = Sad
            ///Int 3 = Surprise
            ///Int 4 = Happy
            ///
            ///Pour POPPY :
            ///Int 1 = Neutral
            ///Int 2 = Sad
            ///Int 3 = Disguts
            ///Int 4 = Happy
        }
    }
}

