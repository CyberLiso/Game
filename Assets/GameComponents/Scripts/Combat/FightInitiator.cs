using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Attributes;
using RPG.Control;
using RPG.Saving;
using RPG.Stats;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class FightInitiator : MonoBehaviour, IAction, ISaveable, IGetAdditiveMofifiers, IGetPercentageModifiers
    {
        Health targetLocation;

        [SerializeField] Transform RightHandPosition;
        [SerializeField] Transform LeftHandPosition;
        [SerializeField] Weapon DefaultWeapon = null;
        LazyValue<Weapon> currentWeapon = null;

        private float timeSinceLastAttack = Mathf.Infinity;
        private bool canAttack = true;


        public void Attack(GameObject target)
        {
            GetComponent<ActionSchedular>().StartAction(this);
            targetLocation = target.GetComponent<Health>();
        }

        private void Awake()
        {
            currentWeapon = new LazyValue<Weapon>(GetDefaultWeapon);
        }

        private Weapon GetDefaultWeapon()
        {
            AttachWeapon(DefaultWeapon);
            return DefaultWeapon;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (currentWeapon == null)
            {
                currentWeapon.ForceInit();
            }
        }

        public void EquipWeapon(Weapon EquippedWeapon)
        {
            currentWeapon.value = EquippedWeapon;
            AttachWeapon(EquippedWeapon);
        }

        private void AttachWeapon(Weapon EquippedWeapon)
        {
            Animator animator = GetComponent<Animator>();
            EquippedWeapon.SpawnWeapon(RightHandPosition, LeftHandPosition, animator);
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
            if (timeSinceLastAttack >= currentWeapon.value.GetAttackThrotleTime()) canAttack = true; else canAttack = false;
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
                transform.LookAt(targetLocation.transform);
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

        public Weapon GetCurrentWeapon()
        {
            return currentWeapon.value;
        }

        public void TriggerAttackAnims()
        {
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, targetLocation.transform.position) < currentWeapon.value.GetRange();
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

        public GameObject GetTarget()
        {
            if (targetLocation == null) return null;
            return targetLocation.gameObject;
        }


        //AnimationEvent
        void Hit()
        {

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (targetLocation == null) return;
            targetLocation.TakeDamage(gameObject,damage);
        }

        void Shoot()
        {
            currentWeapon.value.SpawnProjectile(targetLocation, LeftHandPosition, gameObject);
        }

        public object CaptureState()
        {
           if(currentWeapon != null)
           {
               return currentWeapon.value.name;
           }
           else
           {
               return DefaultWeapon.name;
           }
        }

        public void RestoreState(object state)
        {
            string savedWeaponName = (string) state;
            Weapon SavedWeapon = Resources.Load<Weapon>(savedWeaponName);
            print(savedWeaponName);
            EquipWeapon(SavedWeapon);
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if(stat == Stat.Damage)
            {
               yield return currentWeapon.value.GetPercentageBuff();
            }
        }
    }
}
