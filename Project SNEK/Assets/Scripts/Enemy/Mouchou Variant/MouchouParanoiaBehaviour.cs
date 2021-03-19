using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public GameObject renderGo;

        private void Update()
        {
            if(mBm.isMoving == false)
            {
                renderGo.GetComponentInChildren<MeshFilter>().mesh = meshes[1];
                renderGo.GetComponentInChildren<MeshRenderer>().material = mat[1];
                hadPoof = false;
                renderGo.transform.rotation = Quaternion.Euler(0, 0, 0);
                if (hadPoofed == false)
                {
                    Instantiate(poof, transform.position, Quaternion.identity);
                    hadPoofed = true;
                }
            }
            else if (mBm.isMoving)
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
}


