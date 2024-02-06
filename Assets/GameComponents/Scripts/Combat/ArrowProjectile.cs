using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    public class ArrowProjectile : MonoBehaviour
    {

        [SerializeField] float ArrowSpeed = 10f;
        [SerializeField] float ArrowDamage;
        public Health target = null;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(target != null) MoveProjectile(ArrowSpeed);
        }

        private void MoveProjectile(float arrowSpeed)
        {
            transform.LookAt(GetAimLocation());
            transform.Translate(arrowSpeed * Time.deltaTime * Vector3.forward);
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject == target.gameObject)
            {
                target.TakeDamage(ArrowDamage);
                Destroy(gameObject);
            }
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
