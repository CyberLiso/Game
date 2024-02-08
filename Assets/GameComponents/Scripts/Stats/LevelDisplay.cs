using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
            BaseStats Player_Stats;

            void Awake()
            {
                Player_Stats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            }

            // Update is called once per frame
            void Update()
            {
                gameObject.GetComponent<Text>().text = $"Level: {Player_Stats.GetLevel()}";
            }
    }
}
