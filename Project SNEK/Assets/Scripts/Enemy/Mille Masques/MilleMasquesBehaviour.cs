using Player.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Thomas
    /// </summary>
    public class MilleMasquesBehaviour : MonoBehaviour
    {
        [SerializeField] EnemyStats stats;

        [Header("Body")]
        public int bodyPartsCount;
        [SerializeField] public GameObject head;
        [SerializeField] GameObject bodyPartPrefab;
        [SerializeField] GameObject bodyPartMonochromPrefab;
        [SerializeField] GameObject bodyPartParent;
        [SerializeField] public  List<GameObject> bodyParts = new List<GameObject>();
        [SerializeField] public MonochromElement headMonochromComponent;

        [SerializeField] bool isMonochrom;

        [Header("Hearts")]
        [SerializeField] public List<MilleMasquesHeart> hearts = new List<MilleMasquesHeart>();
        [HideInInspector] public bool canDie = false;

        [Header("Movement")]
        public MouchouPattern pattern;
        [HideInInspector] public Vector3 currentHeadDir;

        [Header("Feedback")]
        public GameObject feedbackObject;
        private Coroutine destroyCoroutine;
        
        // Start is called before the first frame update
        void Start()
        {
            if (isMonochrom)
                headMonochromComponent.enabled = true;
            else
                headMonochromComponent.enabled = false;

            stats.currentHp = hearts.Count;

            for (int i = 0; i < hearts.Count; i++)
            {
                hearts[i].behaviour = this;
            }

            InitBody();
        }

        private void Update()
        {
            RotateBodyPart();

            if(hearts.Count == 0 && !isMonochrom && canDie)
            {
                if (destroyCoroutine == null)
                    destroyCoroutine = StartCoroutine(DestroyCoroutine());
            }
        }

        void InitBody()
        {
            Vector3 originPosition = head.transform.position;
            bodyParts.Add(head);

            GameObject prefab = null;
            if (isMonochrom)
                prefab = bodyPartMonochromPrefab;
            else
                prefab = bodyPartPrefab;



            for (int i = 0; i < bodyPartsCount; i++)
            {
                int index = (pattern.patternList.Count - (i % pattern.patternList.Count)) - 1 ; //Get index of reverse patternList

                switch (pattern.patternList[index])
                {
                    case MouchouDirection.Up: //Offset Down
                        originPosition += new Vector3(0, 0, -1);
                        
                        break;

                    case MouchouDirection.Right: //Offset Left
                        originPosition += new Vector3(-1, 0, 0);

                        break;

                    case MouchouDirection.Down: //Offset Up
                        originPosition += new Vector3(0, 0, 1);

                        break;

                    case MouchouDirection.Left: //Offset Right
                        originPosition += new Vector3(1, 0, 0);

                        break;

                    default:
                        break;
                }

                GameObject temp = Instantiate(prefab, originPosition, Quaternion.identity, bodyPartParent.transform);
                bodyParts.Add(temp);
            }
        }

        void RotateBodyPart()
        {
            bodyParts[0].transform.forward = Vector3.Lerp(bodyParts[0].transform.forward, currentHeadDir, Time.deltaTime * 4);

            for (int i = 1; i < bodyParts.Count; i++)
            {
                bodyParts[i].transform.forward = bodyParts[i - 1].transform.position - bodyParts[i].transform.position;
            }
        }

        private IEnumerator DestroyCoroutine()
        {
            for (int i = 0; i < bodyParts.Count; i++)
            {
                Instantiate(feedbackObject, bodyParts[i].transform.position, Quaternion.identity);
            }
            yield return null;
            Destroy(gameObject.transform.parent.gameObject);
        }

    }
}

