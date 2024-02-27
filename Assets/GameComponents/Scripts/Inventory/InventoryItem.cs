using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Movement;

namespace RPG.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        private ItemConfig Item;
        bool areOptionsShown = false;
        [Range(0, 1.5f)] [SerializeField] float minimumTimeBetweenEquipClicks;
        float timeSinceLastClick = 0f;
        bool hasClicked = false;

        public void SetButtonConfig(ItemConfig Item) 
        {
            this.Item = Item;
        }

        public void EquipItem()
        {
            if (!hasClicked)
            {
                timeSinceLastClick = 0f;
                hasClicked = true;
                return;
            }
            else
            {
                if (timeSinceLastClick <= minimumTimeBetweenEquipClicks)
                {
                    timeSinceLastClick = 0f;
                    hasClicked = false;
                }
                else
                {
                    hasClicked = false;
                    return;
                }

            }

            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<WASDMovement>().EquipInventoryItem(Item);
            return;
        }

        private void Update()
        {
            timeSinceLastClick += Time.deltaTime;
        }

        public void ShowOrHideOptions()
        {
            areOptionsShown = !areOptionsShown;
            transform.Find("ButtonToEquip").gameObject.SetActive(areOptionsShown);
        }

        public void DeleteItem()
        {
            Destroy(gameObject);
        }
    }
}
