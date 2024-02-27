using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] ItemConfig itemPickup;
        [SerializeField] float RespawnTime = 10f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                PickUp();
            }
        }

        private void PickUp()
        {
            FindObjectOfType<InventoryScript>().AddToInventory(itemPickup);
            HideForSeconds(RespawnTime);
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
    }
}
