using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon pickup;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                FightInitiator fightInitiator = other.gameObject.GetComponent<FightInitiator>();
                fightInitiator.EquipWeapon(pickup);
                Destroy(gameObject);
            }
        }
    }
}
