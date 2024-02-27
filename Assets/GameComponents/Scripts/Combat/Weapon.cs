using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent OnHitEvent;
        DynamicCombatController playerController;

        private void Start()
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<DynamicCombatController>();
        }
        public void OnWeaponHit()
        {
            OnHitEvent.Invoke();
        }
        /*private void OnTriggerEnter(Collider other)
        {
            // Check if the collision occurred during an attack
            if (playerController != null && playerController.isAttacking)
            {
                playerController.DealDamageOnImpact();
            }
        }*/
    }
}
