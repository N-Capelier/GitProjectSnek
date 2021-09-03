using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemy
{
    /// <summary>
    /// Thomas
    /// </summary>
    public class MilleMasquesMovement : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] MilleMasquesBehaviour behaviour;
        [SerializeField] EnemyStats stats;

        private MouchouPattern pattern;
        private int patternOffset = 0;
        private Coroutine lastCoroutine;
        private bool canMove = true;

        void Start()
        {
            pattern = behaviour.pattern;
            switch (pattern.patternList[0])
            {
                case MouchouDirection.Up:
                    behaviour.head.transform.forward = new Vector3(0,0,1);
                    break;
                case MouchouDirection.Right:
                    behaviour.head.transform.forward = new Vector3(1,0,0);
                    break;
                case MouchouDirection.Down:
                    behaviour.head.transform.forward = new Vector3(0,0,-1);
                    break;
                case MouchouDirection.Left:
                    behaviour.head.transform.forward = new Vector3(-1,0,0);
                    break;
                default:
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(canMove)
            {
                Move();
            }
        }

        void Move()
        {
            for (int i = 0; i < behaviour.bodyParts.Count; i++)
            {
                 int index = ((pattern.patternList.Count - i) + patternOffset) % pattern.patternList.Count;

                switch (pattern.patternList[index])
                {
                    case MouchouDirection.Up:
                        StartCoroutine(MoveCoroutine(behaviour.bodyParts[i], new Vector3(0,0,1), i));
                        break;
                    case MouchouDirection.Right:
                        StartCoroutine(MoveCoroutine(behaviour.bodyParts[i], new Vector3(1, 0, 0), i));
                        break;
                    case MouchouDirection.Down:
                        StartCoroutine(MoveCoroutine(behaviour.bodyParts[i], new Vector3(0, 0, -1), i));
                        break;
                    case MouchouDirection.Left:
                        StartCoroutine(MoveCoroutine(behaviour.bodyParts[i], new Vector3(-1, 0, 0), i));
                        break;
                    default:
                        break;
                }

            }
            canMove = false;
            patternOffset++;
        }

        private IEnumerator MoveCoroutine(GameObject bodypartToMove, Vector3 dir, int index)
        {
            if (index == 0)
                behaviour.currentHeadDir = dir;

            float timer = 0;
            Vector3 target = bodypartToMove.transform.position + dir;
            Vector3 origin = bodypartToMove.transform.position;
            while (timer < 1)
            {
                bodypartToMove.transform.position = Vector3.Lerp(origin, target, timer);
                timer += Time.deltaTime * stats.moveSpeed;
                yield return new WaitForEndOfFrame();
            }

            if (index == behaviour.bodyParts.Count - 1)
                canMove = true;
        }
    }
}

