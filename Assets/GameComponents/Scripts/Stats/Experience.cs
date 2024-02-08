using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        public float experiencePoints = 0f;
        public event Action OnExperienceChangeEvent;
        public void GainExperiencePoints(float XP)
        {
            experiencePoints += XP;
            OnExperienceChangeEvent();
        }

        public void RestoreState(object state)
        {
            float savedExperiencePoints = (float)state;
            experiencePoints = savedExperiencePoints;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

    }
}
