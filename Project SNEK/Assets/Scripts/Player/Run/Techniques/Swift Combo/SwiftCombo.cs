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

        public GameObject FX;
        [SerializeField] float fxOffSet = 0.15f;

        PlayerRunAttack pc;

        private void Start()
        {
            base.Start();
            pc = PlayerManager.Instance.currentController.playerRunAttack;
        }

        public override IEnumerator TechniqueCast(PlayerDirection techniqueDirection)
        {
            PlayerManager.Instance.currentController.objectRenderer.GetComponent<Animator>().Play("Anim_PlayerRun_SwiftCombo");
            yield return new WaitForSeconds(0.1f);
            Attack(1);
            InstantiateFx1();
            yield return new WaitForSeconds(0.33f);
            Destroy(attack);
            yield return new WaitForSeconds(0.01f);
            Attack(0.75f);
            InstantiateFx2();
            yield return new WaitForSeconds(0.33f);
            Destroy(attack);
            yield return new WaitForSeconds(0.01f);
            Attack(0.75f);
            InstantiateFx3();
            yield return new WaitForSeconds(0.4f);
            Destroy(attack);
            yield return new WaitForSeconds(0.02f);
            PlayerManager.Instance.currentController.attackMoveSpeedModifier = 1f;
            PlayerManager.Instance.currentController.rb.velocity = PlayerManager.Instance.currentController.rb.velocity * PlayerManager.Instance.currentController.attackMoveSpeedModifier;
        }

        void Attack(float modifier)
        {
            PlayerManager.Instance.currentController.attackMoveSpeedModifier = moveSpeedDuringAttack;
            PlayerManager.Instance.currentController.rb.velocity = PlayerManager.Instance.currentController.rb.velocity * PlayerManager.Instance.currentController.attackMoveSpeedModifier;            
            attack = Instantiate(comboOne, transform.position, Quaternion.identity);
            attack.GetComponent<ComboCollision>().damageModifier = modifier;

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

        void InstantiateFx1()
        {

            GameObject slashFx = Instantiate(FX, gameObject.transform.GetChild(0).gameObject.transform.position, Quaternion.identity);
            slashFx.gameObject.transform.localScale = new Vector3(PlayerManager.Instance.currentController.playerRunAttack.rangeBonus + 0.2f, 1, PlayerManager.Instance.currentController.playerRunAttack.rangeBonus + 0.2f);
            //AudioManager.Instance.PlaySoundEffect("PlayerAttack01");
            switch (PlayerManager.Instance.currentController.currentDirection)
            {
                case PlayerDirection.Up:
                    slashFx.transform.Rotate(new Vector3(90, 0, 0));
                    slashFx.transform.position += new Vector3(0, -0.8f, -fxOffSet);
                    break;
                case PlayerDirection.Down:
                    slashFx.transform.Rotate(new Vector3(90, 0, 180));
                    slashFx.transform.position += new Vector3(0, -0.8f, fxOffSet);
                    break;
                case PlayerDirection.Left:
                    slashFx.transform.Rotate(new Vector3(90, 0, 90));
                    slashFx.transform.position += new Vector3(fxOffSet, -0.8f, 0);
                    break;
                case PlayerDirection.Right:
                    slashFx.transform.Rotate(new Vector3(90, 0, 270));
                    slashFx.transform.position += new Vector3(-fxOffSet, -0.8f, 0);
                    break;
            }
        }

        void InstantiateFx2()
        {

            GameObject slashFx = Instantiate(FX, gameObject.transform.GetChild(0).gameObject.transform.position, Quaternion.identity);
            slashFx.gameObject.transform.localScale = new Vector3(-PlayerManager.Instance.currentController.playerRunAttack.rangeBonus + 0.2f, 1, PlayerManager.Instance.currentController.playerRunAttack.rangeBonus + 0.2f);
            //AudioManager.Instance.PlaySoundEffect("PlayerAttack01");
            switch (PlayerManager.Instance.currentController.currentDirection)
            {
                case PlayerDirection.Up:
                    slashFx.transform.Rotate(new Vector3(130, 90, 90));
                    slashFx.transform.position += new Vector3(0, 0, -fxOffSet);
                    break;
                case PlayerDirection.Down:
                    slashFx.transform.Rotate(new Vector3(130, -90, 90));
                    slashFx.transform.position += new Vector3(0, 0, fxOffSet);
                    break;
                case PlayerDirection.Left:
                    slashFx.transform.Rotate(new Vector3(130, 0, 90));
                    slashFx.transform.position += new Vector3(fxOffSet, 0, 0);
                    break;
                case PlayerDirection.Right:
                    slashFx.transform.Rotate(new Vector3(130, 180, 90));
                    slashFx.transform.position += new Vector3(-fxOffSet, 0, 0);
                    break;
            }
        }

        void InstantiateFx3()
        {

            GameObject slashFx = Instantiate(FX, gameObject.transform.GetChild(0).gameObject.transform.position, Quaternion.identity);
            slashFx.gameObject.transform.localScale = new Vector3(PlayerManager.Instance.currentController.playerRunAttack.rangeBonus + 0.2f, 1, PlayerManager.Instance.currentController.playerRunAttack.rangeBonus + 0.2f);
            //AudioManager.Instance.PlaySoundEffect("PlayerAttack01");
            switch (PlayerManager.Instance.currentController.currentDirection)
            {
                case PlayerDirection.Up:
                    slashFx.transform.Rotate(new Vector3(130, 90, 90));
                    slashFx.transform.position += new Vector3(0, 0, -fxOffSet);
                    break;
                case PlayerDirection.Down:
                    slashFx.transform.Rotate(new Vector3(130, -90, 90));
                    slashFx.transform.position += new Vector3(0, 0, fxOffSet);
                    break;
                case PlayerDirection.Left:
                    slashFx.transform.Rotate(new Vector3(130, 0, 90));
                    slashFx.transform.position += new Vector3(fxOffSet, 0, 0);
                    break;
                case PlayerDirection.Right:
                    slashFx.transform.Rotate(new Vector3(130, 180, 90));
                    slashFx.transform.position += new Vector3(-fxOffSet, 0, 0);
                    break;
            }
        }

    }

    
}