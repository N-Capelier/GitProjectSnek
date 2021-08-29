using UnityEngine;
using Player;
using Map;

namespace Enemy
{
    public class MouchouParanoiaBehaviour : MonoBehaviour
    {
        public Mesh[] meshes;
        public Material[] mat;
        public MouchouBaseMovement mBm;
        public GameObject poof;
        bool hadPoof = false;
        bool hadPoofed = false;
        bool isAttacking = false;
        bool canAttack = true;
        public GameObject renderGo;
        public int distance = 4;

        [Space]
        float distanceToPlayer;
        Vector2 directionToPlayer;

        private void Update()
        {
            if (MapGrid.Instance == null)
                return;

            if (isAttacking == false)
            {
                if (mBm.isMoving == false)
                {
                    TransformIntoProp();
                }
                else if (mBm.isMoving)
                {
                    TransformIntoMouchou();
                }                
            }

            if ((mBm.currentDirection == MouchouDirection.Up && transform.position.z >= MapGrid.Instance.GetWorldPos(0, (int)mBm.nextNode.z).z) ||
                (mBm.currentDirection == MouchouDirection.Right && transform.position.x >= MapGrid.Instance.GetWorldPos((int)mBm.nextNode.x, 0).x) ||
                (mBm.currentDirection == MouchouDirection.Down && transform.position.z <= MapGrid.Instance.GetWorldPos(0, (int)mBm.nextNode.z).z) ||
                (mBm.currentDirection == MouchouDirection.Left && transform.position.x <= MapGrid.Instance.GetWorldPos((int)mBm.nextNode.x, 0).x))
            {
                canAttack = true;
            }

            if (PlayerManager.Instance.currentController != null)
            {
                distanceToPlayer = Vector3.Distance(PlayerManager.Instance.currentController.transform.position, gameObject.GetComponentInParent<EnemyStats>().transform.position);

                if (distanceToPlayer < distance)
                {
                    AttackPlayer();
                }
                else
                {
                    isAttacking = false;
                    mBm.canMove = true;
                }
            }         
        }

        float directionX;
        float directionZ;

        void AttackPlayer()
        {
            TransformIntoMouchou();
            directionX = transform.position.x - PlayerManager.Instance.currentController.transform.position.x;
            directionZ = transform.position.z - PlayerManager.Instance.currentController.transform.position.z;
            isAttacking = true;
            mBm.canMove = false;
            directionToPlayer = new Vector2(directionX, directionZ).normalized;

            if(directionToPlayer.x > (Mathf.Sqrt(2)*0.5f))
            {
                mBm.currentDirection = MouchouDirection.Left;
            }
            else if(directionToPlayer.x < (-Mathf.Sqrt(2) * 0.5f))
            {
                mBm.currentDirection = MouchouDirection.Right;
            }
            else if(directionToPlayer.y > (Mathf.Sqrt(2) * 0.5f))
            {
                mBm.currentDirection = MouchouDirection.Down;
            }
            else
            {
                mBm.currentDirection = MouchouDirection.Up;
            }

            if (canAttack == true)
            {
                mBm.GetNextNode();
                mBm.UpdateMovementDash();
                canAttack = false;
            }           
        }

        void TransformIntoProp()
        {
            renderGo.GetComponentInChildren<MeshFilter>().mesh = meshes[1];
            renderGo.GetComponentInChildren<MeshRenderer>().material = mat[1];
            hadPoof = false;
            renderGo.transform.rotation = Quaternion.Euler(0, 0, 0);
            //if (hadPoofed == false)
            //{
            //    Instantiate(poof, transform.position, Quaternion.identity);
            //    hadPoofed = true;
            //}
        }

        void TransformIntoMouchou()
        {
            renderGo.GetComponentInChildren<MeshFilter>().mesh = meshes[0];
            renderGo.GetComponentInChildren<MeshRenderer>().material = mat[0];
            hadPoofed = false;
            if (hadPoof == false)
            {
                Instantiate(poof, transform.position, Quaternion.identity);
                hadPoof = true;
            }
        }

    }
}


