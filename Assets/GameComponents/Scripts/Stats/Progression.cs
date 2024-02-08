using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{ 
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/Creat New Progression", order = 1)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<PlayerClasses, Dictionary<Stat, float[]>> lookupTable = null;
        public float GetStat(PlayerClasses CharacterClass, int level, Stat stat)
        {
            BuildLookup();

            var statLookupTable = lookupTable[CharacterClass];
            return statLookupTable[stat][level - 1];
        }

        public int GetLevels(PlayerClasses CharacterClass, Stat stat)
        {
            BuildLookup();

            float[] levels = lookupTable[CharacterClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;
            lookupTable = new Dictionary<PlayerClasses, Dictionary<Stat, float[]>>();
            foreach(var Class in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach(var ProgressStat in Class.stats)
                {
                    statLookupTable[ProgressStat.stat] = ProgressStat.levels;
                }

                lookupTable[Class.Class] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass 
        {
            public PlayerClasses Class;
            public ProgressStat[] stats;
        }

        [System.Serializable]
        class ProgressStat
        {
            public float[] levels;
            public Stat stat;
        }
    }
}
