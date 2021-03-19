using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class MouchouParanoiaBehaviour : MonoBehaviour
    {
        public Mesh[] meshes;
        public Material[] mat;
        public MeshFilter mFilter;
        public MeshRenderer mRender;
        public MouchouBaseMovement mBm;
        public GameObject poof;
        bool hadPoof = false;
        public GameObject renderGo;

        private void Update()
        {
            if(mBm.isMoving == false)
            {
                mFilter.mesh = meshes[1];
                mRender.material = mat[1];
                hadPoof = false;
                renderGo.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (mBm.isMoving)
            {
                mFilter.mesh = meshes[0];
                mRender.material = mat[0];
                if (hadPoof == false)
                {
                    Instantiate(poof, transform.position, Quaternion.identity);
                    hadPoof = true;
                }
            }
        }
    }
}


