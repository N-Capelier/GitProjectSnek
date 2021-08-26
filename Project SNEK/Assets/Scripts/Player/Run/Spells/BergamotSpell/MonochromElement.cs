using Player;
using Player.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Spells
{
    public class MonochromElement : MonoBehaviour
    {
        public Collider hitbox;
        public MeshRenderer rd;
        public GameObject particles;
        public GameObject shaderRenderer;

        private void Start()
        {
            MonochromElementsManager.Instance.elements.Add(this);
        }
    }
}

