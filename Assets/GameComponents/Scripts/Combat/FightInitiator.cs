using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class FightInitiator : MonoBehaviour, IAction
    {
        Health targetLocation;
        [Range(0, 10)] [SerializeField] float StoppingDistance;
        [Range(0, 5)] [SerializeField] float attackThrotleTime;
        [SerializeField] GameObject EquippedWeapon = null;
        [SerializeField] Transform weaponPosition;
        private float timeSinceLastAttack = Mathf.Infinity;
        private bool canAttack = true;
        public float fistAttackDamage = 10f;
        public void Attack(GameObject target)
        {
            GetComponent<ActionSchedular>().StartAction(this);
            targetLocation = target.GetComponent<Health>();
        }



        // Start is called before the first frame update
        void Start()
        {
            //Instantiate(EquippedWeapon, weaponPosition);
        }

        public bool CanAttack(GameObject combatTarget)
        {
            Health targetHealth = combatTarget.GetComponent<Health>();
            return targetHealth != null && !targetHealth.IsDead;
        }

        // Update is called once per frame
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (timeSinceLastAttack >= attackThrotleTime) canAttack = true; else canAttack = false;
            if (targetLocation == null || targetLocation.IsDead) return;

            CheckIfPlayerIsInRange();
        }

        private void CheckIfPlayerIsInRange()
        {
            if (!GetIsInRange())
            {
                GetComponent<Move>().MoveTo(targetLocation.transform.position, 1f);
            }
            else if(GetIsInRange())
            {
                GetComponent<Move>().Cancel();
                if (canAttack)
                {
                    timeSinceLastAttack = 0;
                    PlayAttackAnimation();    
                }
                else
                {
                    GetComponent<Animator>().ResetTrigger("Attack");   
                }
            }
        }

        public void PlayAttackAnimation()
        {
            transform.LookAt(targetLocation.transform);
            TriggerAttackAnims();
        }

        public void TriggerAttackAnims()
        {
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, targetLocation.transform.position) < StoppingDistance;
        }

        public void Cancel()
        {
            targetLocation = null;
            ResetAttackAnims();
            GetComponent<Move>().Cancel();
        }

        private void ResetAttackAnims()
        {
            GetComponent<Animator>().SetTrigger("StopAttack");
            GetComponent<Animator>().ResetTrigger("Attack");
        }

        //AnimationEvent
        void Hit()
        {
            if (targetLocation == null) return;
            targetLocation.TakeDamage(fistAttackDamage);
        }

    }
}
