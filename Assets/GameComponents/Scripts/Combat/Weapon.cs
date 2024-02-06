using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon",menuName = "Weapons/Create New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController WeaponOverrideController;
        [SerializeField] GameObject WeaponPrefab = null;
        [SerializeField] float WeaponDamage = 10f;
        [Range(0, 10)] [SerializeField] float WeaponRange;
        [SerializeField] bool isRightHanded = true;
        public bool isRanged = false;
        [SerializeField] ArrowProjectile WeaponProjectile;

        public void SpawnWeapon(Transform RightHand, Transform LeftHand, Animator animator)
        {
            if (WeaponPrefab != null)
            {
                if (isRightHanded)
                {
                    Instantiate(WeaponPrefab, RightHand);
                }
                else
                {
                    Instantiate(WeaponPrefab, LeftHand);
                }
            }
            if (WeaponOverrideController != null) animator.runtimeAnimatorController = WeaponOverrideController;
        }

        public void SpawnProjectile(Health target, Transform HandTransform)
        {
            GameObject projectile = Instantiate(WeaponProjectile.gameObject);
            projectile.transform.position = HandTransform.position;
            projectile.transform.rotation = HandTransform.rotation;
            projectile.GetComponent<ArrowProjectile>().target = target;
        }

        public float GetDamage()
        {
            return WeaponDamage;
        }
        public float GetRange()
        {
            return WeaponRange;
        }
    }
}
