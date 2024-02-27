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
    public class DynamicCombatController : MonoBehaviour, IAction, ISaveable, IGetAdditiveMofifiers, IGetPercentageModifiers
    {
        Health targetLocation;

        [SerializeField] Transform RightHandPosition;
        [SerializeField] Transform LeftHandPosition;
        [SerializeField] WeaponConfig DefaultWeapon = null;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;
        [SerializeField] float targetLockRadius;
        GameObject lastOnFocusTarget = null;

        private float timeSinceLastAttack = Mathf.Infinity;
        private bool canAttack = true;
        float minimumThrottleTime = 0f;


        public void Attack()
        {
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
            {
                RaycastHit[] hitTransforms;
                Dictionary<float, CombatTargeter> targetDistances = new Dictionary<float, CombatTargeter>();

                hitTransforms = Physics.SphereCastAll(transform.position, targetLockRadius, transform.forward);
                if (hitTransforms.Length == 0)
                {
                    return;
                }
                foreach (RaycastHit hit in hitTransforms)
                {
                    CombatTargeter target = hit.transform.GetComponent<CombatTargeter>();
                    if (target == null) continue;
                    float distanceFromPlayer = Vector3.Distance(transform.position, hit.transform.position);
                    targetDistances[distanceFromPlayer] = target;
                }
                if (targetDistances.Count > 0)
                {
                    float smallestValue = float.MaxValue; // Start with the largest possible float value
                    foreach (float key in targetDistances.Keys)
                    {
                        if (key < smallestValue)
                        {
                            smallestValue = key;
                        }
                    }
                    targetLocation = targetDistances[smallestValue].GetComponent<Health>();
                }
            }
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
            Attack();
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= minimumThrottleTime) canAttack = true; else canAttack = false;
            if (targetLocation == null || targetLocation.IsDead) return;
            //print(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        }

        private void LateUpdate()
        {
            CheckIfPlayerIsInRange();
        }

        private void CheckIfPlayerIsInRange()
        {
            if (targetLocation != null)
            {
                if (Input.GetMouseButton(1))
                {
                    transform.LookAt(targetLocation.transform);
                    if (lastOnFocusTarget == null)
                    {
                        lastOnFocusTarget = targetLocation.transform.Find("OnFocus").gameObject;
                        return;
                    }
                    lastOnFocusTarget = targetLocation.transform.Find("OnFocus").gameObject;
                    lastOnFocusTarget.SetActive(true);
                }
                else if(lastOnFocusTarget != null)
                {
                    lastOnFocusTarget.SetActive(false);
                }
            }
            if (canAttack)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    timeSinceLastAttack = 0;
                    PlayAttackAnimation();
                }
            }
            else
            {
                GetComponent<Animator>().ResetTrigger("Attack");
            }
        }

        public void PlayAttackAnimation()
        {
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
            minimumThrottleTime = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
            if (targetLocation != null && !GetIsInRange()) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (targetLocation == null) return;
            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnWeaponHit();
            }

            targetLocation.TakeDamage(gameObject, damage);
        }

        void Shoot()
        {
            if (!GetIsInRange())
            {
                return;
            }
            currentWeaponConfig.SpawnProjectile(targetLocation, LeftHandPosition, gameObject);
            currentWeapon.value.OnWeaponHit();
        }

        public object CaptureState()
        {
            if (currentWeaponConfig != null)
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
            string savedWeaponName = (string)state;
            WeaponConfig SavedWeapon = Resources.Load<WeaponConfig>(savedWeaponName);
            print(savedWeaponName);
            EquipWeapon(SavedWeapon);
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBuff();
            }
        }
    }
}
