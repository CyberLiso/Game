using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventory
{
    public class InventoryScript : MonoBehaviour
    {
        [SerializeField] GameObject inventoryItemPrefab;
        [SerializeField] Transform inventoryContents;
        [SerializeField] GameObject inventory;
        [SerializeField] ItemConfig test;
        public void AddToInventory(ItemConfig item)
        {
            GameObject addedItem = Instantiate(inventoryItemPrefab, inventoryContents);
            addedItem.transform.Find("ItemName").GetComponent<Text>().text = item.Name;
            addedItem.transform.Find("ItemPlaceholder").GetComponent<Image>().sprite = item.ImagePlaceHolder;
            addedItem.GetComponent<InventoryItem>().SetButtonConfig(item);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                inventory.SetActive(true);
            }
        }
        private void Start()
        {
            AddToInventory(test);
        }
    }
}
