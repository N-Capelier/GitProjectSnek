using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Spells
{
    public class ThistleSpellBehaviour : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Beam"))
            {
                //Détruire l'objet;
            }
        }
    }
}

