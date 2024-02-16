using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Create New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController WeaponOverrideController;
        [SerializeField] Weapon WeaponPrefab = null;
        [SerializeField] float WeaponDamage = 10f;
        [Range(0, 50)] [SerializeField] float WeaponRange;
        [Range(0, 20)] [SerializeField] float attackThrotleTime;
        [SerializeField] bool isRightHanded = true;
        [Range(0, 100)] [SerializeField] float PercentageBuff = 0f;
        public bool isRanged = false;
        [SerializeField] ArrowProjectile WeaponProjectile;
        const string WeaponName = "Weapon";

        public Weapon SpawnWeapon(Transform RightHand, Transform LeftHand, Animator animator)
        {

            Weapon equippedWeapon = null;
            DestroyOldWeapon(RightHand, LeftHand);
            if (WeaponPrefab != null)
            {
                Transform handTransform = GetHandTranform(RightHand,LeftHand);
                equippedWeapon = Instantiate(WeaponPrefab, handTransform);
                equippedWeapon.gameObject.name = WeaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (WeaponOverrideController != null)
            {
                animator.runtimeAnimatorController = WeaponOverrideController;
            }
            else if(overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return equippedWeapon;
        }


        public float GetPercentageBuff()
        {
            return PercentageBuff;
        }
        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform WeaponHand = rightHand.Find(WeaponName);
            if(WeaponHand == null)
            {
                WeaponHand = leftHand.Find(WeaponName);
            }
            if (WeaponHand == null) return;

            WeaponHand.name = "Destroying...";

            Destroy(WeaponHand.gameObject);
        }

        private Transform GetHandTranform(Transform rightHand, Transform leftHand)
        {
            if (isRightHanded)
            {
                return rightHand;
            }
            else
            {
                return leftHand;
            }
        }

        public void SpawnProjectile(Health target, Transform HandTransform, GameObject Instigator)
        {
            GameObject projectile = Instantiate(WeaponProjectile.gameObject);
            projectile.transform.position = HandTransform.position;
            projectile.transform.rotation = HandTransform.rotation;
            projectile.GetComponent<ArrowProjectile>().SetTarget(target, Instigator);
        }

        public float GetDamage()
        {
            return WeaponDamage;
        }
        public float GetRange()
        {
            return WeaponRange;
        }
        public float GetAttackThrotleTime()
        {
            return attackThrotleTime;
        }
    }
}
