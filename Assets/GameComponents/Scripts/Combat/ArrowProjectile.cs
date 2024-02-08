using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{
    public class ArrowProjectile : MonoBehaviour
    {

        [SerializeField] float ProjectileSpeed = 10f;
        [SerializeField] float ProjectileDamage;
        Health target = null;
        [SerializeField] float ProjectileDestructionTime;
        [SerializeField] float ProjectileDeathTime = 10f;
        private bool hasHit = false;
        public bool isHoming = false;
        private bool hasBeenShot = false;
        [SerializeField] GameObject ImpactEffect = null;
        [SerializeField] GameObject[] DestoryOnHit = null;
        GameObject Instigator;
        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, ProjectileDeathTime);
        }

        // Update is called once per frame
        void Update()
        {
            if (target != null)
            {
                if (hasHit == false)
                {
                    if (isHoming)
                    {
                        MoveHomingArrow(ProjectileSpeed);
                    }
                    else
                    {
                        MoveBasicArrow(ProjectileSpeed);
                    }
                }
                else
                {
                    gameObject.transform.position = GetAimLocation();
                }
                if (target.IsDead) Destroy(gameObject);
            }
        }

        private void MoveHomingArrow(float arrowSpeed)
        {
            transform.LookAt(GetAimLocation());
            transform.Translate(arrowSpeed * Time.deltaTime * Vector3.forward);
        }
        private void MoveBasicArrow(float arrowSpeed)
        {
            if(!hasBeenShot)
            transform.LookAt(GetAimLocation());
            transform.Translate(arrowSpeed * Time.deltaTime * Vector3.forward);
            hasBeenShot = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != null && target != null)
            {
                if (target.IsDead) return;
                if (other.gameObject == target.gameObject)
                {
                    if (ImpactEffect != null) 
                    {
                        Instantiate(ImpactEffect, GetAimLocation(), transform.rotation);
                    }
                    target.TakeDamage(Instigator, ProjectileDamage);
                    hasHit = true;
                    if (DestoryOnHit.Length > 0)
                    {
                        foreach(GameObject obj in DestoryOnHit)
                        {
                            Destroy(obj);
                        }
                    }
                    Destroy(gameObject, ProjectileDestructionTime);
                }
            }
        }
        public void SetTarget(Health target, GameObject Instigator)
        {
            this.target = target;
            this.Instigator = Instigator;
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider TargetCollider = target.transform.GetComponent<CapsuleCollider>();
            if (TargetCollider == null)
            {
                Debug.LogWarning("The target does not have a valid capsule collider!");
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * TargetCollider.height / 2; 
        }
    }
}
