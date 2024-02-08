using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour,  IRayCastable
    {
        [SerializeField] Weapon pickup;
        [SerializeField] float RespawnTime = 10f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                PickUp(other.GetComponent<FightInitiator>());
            }
        }

        private void PickUp(FightInitiator other)
        {
            other.EquipWeapon(pickup);
            StartCoroutine(HideForSeconds(RespawnTime));
        }

        private IEnumerator HideForSeconds(float t)
        {
            ShowOrHidePickup(false);
            yield return new WaitForSeconds(t);
            ShowOrHidePickup(true);
        }

        private void ShowOrHidePickup(bool shouldBeVisible)
        {
            gameObject.GetComponent<SphereCollider>().enabled = shouldBeVisible;
            foreach(Transform g in GetComponentsInChildren<Transform>())
            {
                if (g == transform) continue;
                g.gameObject.GetComponent<Renderer>().enabled = shouldBeVisible;
            }
        }
        public bool CanHandleRaycast(MovementController controller)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickUp(controller.GetComponent<FightInitiator>());
            }
            return true;
        }

        public CursorModes GetCursorType()
        {
            return CursorModes.Pickup;
        }
    }
}
