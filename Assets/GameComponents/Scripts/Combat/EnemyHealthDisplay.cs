using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using UnityEngine.UI;


namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        FightInitiator fighter;
        void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<FightInitiator>();
        }

        // Update is called once per frame
        void Update()
        {
            if(fighter != null && fighter.GetTarget() != null)
            {
                GetComponent<Text>().text = $"Enemy: {fighter.GetTarget().GetComponent<Health>().GetPercentage()}%";
            }
            else
            {
                GetComponent<Text>().text = $"Enemy:N/A";
            }
        }
    }
}
