using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Utils;
using UnityEngine.AI;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] PlayerClasses Class;
        [SerializeField] Progression Progress;
        [Range(1, 3)] [SerializeField] int startingLevel = 1;
        [SerializeField] GameObject LevelUpEffectPrefab;
        public event Action OnLevelUp;
        LazyValue<int> currentLevel;

        private void Awake()
        {
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        public void Start()
        {
            currentLevel.ForceInit();
            if(GetComponent<Experience>() != null)
            GetComponent<Experience>().OnExperienceChangeEvent += SetCurrentLevel;
        }

        public float GetStat(Stat stat)
        {
            float finalStatValue = (100 + GetPercentageModifiers(stat)) / 100 * (Progress.GetStat(Class, GetLevel(), stat) + GetAdditiveModifiers(stat));
            return finalStatValue;
        }

        private float GetAdditiveModifiers(Stat stat)
        {
            float totalStatModifier = 0f;
            foreach(IGetAdditiveMofifiers modifier in GetComponents<IGetAdditiveMofifiers>())
            {
                foreach(float statModifierValue in modifier.GetAdditiveModifier(stat))
                {
                    totalStatModifier += statModifierValue;
                }
            }

            return totalStatModifier;
        }

        private float GetPercentageModifiers(Stat stat)
        {
            float totalStatModifier = 0f;
            foreach (IGetPercentageModifiers modifier in GetComponents<IGetPercentageModifiers>())
            {
                foreach (float statModifierValue in modifier.GetPercentageModifier(stat))
                {
                    totalStatModifier += statModifierValue;
                }
            }

            return totalStatModifier;
        }

        public int CalculateLevel()
        {
            if (GetComponent<Experience>() == null) return startingLevel;

           float currentXP = GetComponent<Experience>().experiencePoints;
            
           for(int i = 1; i <= Progress.GetLevels(Class,Stat.ExperienceToLevelUp); i++)
           {
                if(Progress.GetStat(Class, i, Stat.ExperienceToLevelUp) > currentXP)
                {
                    return i;
                }
           }

            return Progress.GetLevels(Class, Stat.ExperienceToLevelUp)
                ;
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        public void SetCurrentLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                OnLevelUp();
                LevelUpEffect();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(LevelUpEffectPrefab, transform);
        }
    }
}
