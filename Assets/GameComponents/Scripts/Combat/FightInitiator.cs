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
using UnityEngine.Events;

namespace RPG.Combat
{
    public class FightInitiator : MonoBehaviour, IAction, ISaveable, IGetAdditiveMofifiers, IGetPercentageModifiers
    {
        Health targetLocation;

        [SerializeField] Transform RightHandPosition;
        [SerializeField] Transform LeftHandPosition;
        [SerializeField] WeaponConfig DefaultWeapon = null;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        private float timeSinceLastAttack = Mathf.Infinity;
        private bool canAttack = true;


        public void Attack(GameObject target)
        {
            GetComponent<ActionSchedular>().StartAction(this);
            targetLocation = target.GetComponent<Health>();
        }

        private void Awake()
        {
            currentWeaponConfig = DefaultWeapon;
            currentWeapon = new LazyValue<Weapon>(GetDefaultWeapon);
        }

        private Weapon GetDefaultWeapon()
        {
           return AttachWeapon(DefaultWeapon);
        }

        // Start is called before the first frame update
        void Start()
        {
            currentWeapon.ForceInit();
        }

        public void EquipWeapon(WeaponConfig EquippedWeapon)
        {
            currentWeaponConfig = EquippedWeapon;
            currentWeapon.value = AttachWeapon(EquippedWeapon);
        }

        private Weapon AttachWeapon(WeaponConfig EquippedWeapon)
        {
            Animator animator = GetComponent<Animator>();
            return EquippedWeapon.SpawnWeapon(RightHandPosition, LeftHandPosition, animator);
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
            if (timeSinceLastAttack >= currentWeaponConfig.GetAttackThrotleTime()) canAttack = true; else canAttack = false;
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

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        public void TriggerAttackAnims()
        {
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, targetLocation.transform.position) < currentWeaponConfig.GetRange();
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
            if(currentWeapon.value != null)
            {
                currentWeapon.value.OnWeaponHit();
            }

            targetLocation.TakeDamage(gameObject, damage);
        }

        void Shoot()
        {
            currentWeaponConfig.SpawnProjectile(targetLocation, LeftHandPosition, gameObject);
            currentWeapon.value.OnWeaponHit();
        }

        public object CaptureState()
        {
           if(currentWeaponConfig != null)
           {
               return currentWeaponConfig.name;
           }
           else
           {
               return DefaultWeapon.name;
           }
        }

        public void RestoreState(object state)
        {
            string savedWeaponName = (string) state;
            WeaponConfig SavedWeapon = Resources.Load<WeaponConfig>(savedWeaponName);
            EquipWeapon(SavedWeapon);
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if(stat == Stat.Damage)
            {
               yield return currentWeaponConfig.GetPercentageBuff();
            }
        }
    }
}
