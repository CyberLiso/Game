using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class MovementController : MonoBehaviour
    {
        Health health;
        [Range(0, 1)] [SerializeField] float playerSpeed = 0.6f;
        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            InitiatePlayerMovement();
            if (!GetComponent<Health>().IsDead)
            {
                GetComponent<NavMeshAgent>().enabled = true;
            }
        }

        private void InitiatePlayerMovement()
        {
            if (health.IsDead) return;
            if (InteractWithCombat()) return;
            if (CheckForMouseInput()) return;

        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(ConvertMousePosToRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTargeter target = hit.collider.gameObject.GetComponent<CombatTargeter>();
                if (target == null) continue;
                if (!target.GetComponent<FightInitiator>().CanAttack(target.gameObject)) continue;
                if (Input.GetMouseButton(0))
                { 
                    GetComponent<FightInitiator>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool CheckForMouseInput()
        {
            Debug.DrawRay(ConvertMousePosToRay().origin, ConvertMousePosToRay().direction * 100000);

            //This casts a ray which upon collision sends the point at which it collided
            RaycastHit hit;
            bool hasRayHit = Physics.Raycast(ConvertMousePosToRay(), out hit);

            //The player moves to that point if a collision does occur
            if (hasRayHit)
            {
                //We access the players Move script.
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Move>().PassiveMoveTo(hit.point, playerSpeed);
                }
                return true;
            }
            return false;
        }

        private static Ray ConvertMousePosToRay()
        {
            //Scene Raycast debug tools:
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
