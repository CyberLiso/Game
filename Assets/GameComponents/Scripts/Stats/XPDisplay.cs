using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Stats
{
    public class XPDisplay : MonoBehaviour
    {
        Experience Player_XP;

        void Awake()
        {
            Player_XP = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        // Update is called once per frame
        void Update()
        {
            gameObject.GetComponent<Text>().text = $"XP: {Player_XP.experiencePoints}";
        }
    }
}

