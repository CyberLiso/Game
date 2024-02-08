using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTargeter : MonoBehaviour, IRayCastable
    {
        public bool CanHandleRaycast(MovementController controller)
        {
            if (!controller.GetComponent<FightInitiator>().CanAttack(gameObject)) return false;
            if (Input.GetMouseButtonDown(0))
            {
                controller.GetComponent<FightInitiator>().Attack(gameObject);
            }
            return true;
        }

        public CursorModes GetCursorType()
        {
            return CursorModes.Attack;
        }
    }
}
