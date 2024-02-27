using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Inventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Item/Create New Item", order = 0)]
    public class ItemConfig : ScriptableObject
    {
        public Sprite ImagePlaceHolder;
        public ItemType type;
        public string Name;
        public int value;

        public enum ItemType
        {
            Weapon,
            Potion,
            Charm,
            Consumable,
            Valueble
        }
    }
}