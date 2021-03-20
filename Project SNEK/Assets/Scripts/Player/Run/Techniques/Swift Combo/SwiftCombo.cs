using System.Collections;
using System.Collections.Generic;
using Player.Controller;
using UnityEngine;
using Player.Attack;

namespace Player.Technique
{
    public class SwiftCombo : PlayerTechnique
    {
        float moveSpeedDuringAttack = 0.01f;

        GameObject attack;
        public GameObject comboOne;
        public GameObject comboTwo;

        PlayerRunAttack pc;

        private void Start()
        {
            base.Start();
            pc = PlayerManager.Instance.currentController.playerRunAttack;
        }

        public override IEnumerator TechniqueCast(PlayerDirection techniqueDirection)
        {                    
            Attack(1);
            yield return new WaitForSeconds(0.5f);
            Destroy(attack);
            yield return new WaitForSeconds(0.01f);
            Attack(0.75f);
            yield return new WaitForSeconds(0.5f);
            Destroy(attack);
            yield return new WaitForSeconds(0.01f);
            Attack(0.75f);
            yield return new WaitForSeconds(0.05f);
            Destroy(attack);
            PlayerManager.Instance.currentController.attackMoveSpeedModifier = 1f;
            PlayerManager.Instance.currentController.rb.velocity = PlayerManager.Instance.currentController.rb.velocity * PlayerManager.Instance.currentController.attackMoveSpeedModifier;
        }

        void Attack(float modifier)
        {
            PlayerManager.Instance.currentController.attackMoveSpeedModifier = moveSpeedDuringAttack;
            PlayerManager.Instance.currentController.rb.velocity = PlayerManager.Instance.currentController.rb.velocity * PlayerManager.Instance.currentController.attackMoveSpeedModifier;
            PlayerManager.Instance.currentController.objectRenderer.GetComponent<Animator>().Play("Anim_PlayerRun_attack");
            attack = Instantiate(comboOne, transform.position, Quaternion.identity);
            attack.GetComponent<ComboCollision>().damageModifier = modifier;
            print("Attack " + modifier);
            switch (PlayerManager.Instance.currentController.currentDirection)
            {
                // Ajouter un * par rapport à la range
                case Controller.PlayerDirection.Up:
                    attack.transform.localScale = new Vector3(2.9f * pc.rangeBonus, 1, 2 * pc.rangeBonus);
                    attack.transform.position = transform.position + new Vector3(0, 0, 0.5f * pc.rangeBonus * pc.rangeBonusOffSet);
                    break;
                case Controller.PlayerDirection.Down:
                    attack.transform.localScale = new Vector3(2.9f * pc.rangeBonus, 1, 2 * pc.rangeBonus);
                    attack.transform.position = transform.position + new Vector3(0, 0, -0.5f * pc.rangeBonus * pc.rangeBonusOffSet);
                    break;
                case Controller.PlayerDirection.Left:
                    attack.transform.localScale = new Vector3(2 * pc.rangeBonus, 1, 2.9f * pc.rangeBonus);
                    attack.transform.position = transform.position + new Vector3(-0.5f * pc.rangeBonus * pc.rangeBonusOffSet, 0, 0);
                    break;
                case Controller.PlayerDirection.Right:
                    attack.transform.localScale = new Vector3(2 * pc.rangeBonus, 1, 2.9f * pc.rangeBonus);
                    attack.transform.position = transform.position + new Vector3(0.5f * pc.rangeBonus * pc.rangeBonusOffSet, 0, 0);
                    break;
            }
        }
    }

    
}