using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health Player_Health;

        void Awake()
        {
            Player_Health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            gameObject.GetComponent<Text>().text = $"Health: {Player_Health.GetPercentage()}%";
        }
    }
}
