using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using RPG.Saving;
using RPG.Core;
using UnityEngine.SceneManagement;
using RPG.Stats;
using System;
using GameDevTV.Utils;

namespace RPG.Attributes
{


    public class Health : MonoBehaviour, ISaveable
    {
        public LazyValue<float> currentHealth;
        private static bool hasBeenSaved;
        private Animator animator;
        bool hasRetrievedHealthPoints = false;
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent deathEvent;
        public bool IsDead { get; private set; }


        void Awake()
        {
            currentHealth = new LazyValue<float>(GetInitialHealth);
            hasRetrievedHealthPoints = true;
            animator = GetComponent<Animator>();
        }

        public float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        void Start()
        {
            currentHealth.ForceInit();
            FindObjectOfType<PlayerHealthBarDisplay>().ResetHealthBarValues(GetInitialHealth());
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += SetHealthToMax;
        }
        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= SetHealthToMax;
        }


        public void TakeDamage(GameObject Instigator, float damage)
        {
            currentHealth.value = Mathf.Max(currentHealth.value - damage, 0);
            //transform.GetComponentInChildren<SpawnDamageText>().SpawnDamageTextMethod(damage);
            if (currentHealth.value == 0 && !IsDead)
            {
                takeDamage.Invoke(damage);
                deathEvent.Invoke();
                Death();
                AwardInstigatorExperience(Instigator);
            }
            if (gameObject.tag == "Player")
            {
               StartCoroutine(FindObjectOfType<PlayerHealthBarDisplay>().ReduceHealthBarValue(currentHealth.value, GetInitialHealth()));
            }
        }




        private void AwardInstigatorExperience(GameObject Instigator)
        {
            Experience XP = Instigator.GetComponent<Experience>();
            if (XP == null) return;
            XP.GainExperiencePoints(gameObject.GetComponent<BaseStats>().GetStat(Stat.ExperienceAward));
        }

        public void Death()
        {
            if (IsDead) return;
            IsDead = true;
            GetComponent<ActionSchedular>().CancelCurrentAction();
            StartCoroutine(RunAnimationAfterSceneHasLoaded());
        }
        // Start is called before the first frame update
        public IEnumerator RunAnimationAfterSceneHasLoaded()
        {
            yield return new WaitForSeconds(0.01f);
            animator.SetTrigger("Death");
        }

        private void SetHealthToMax()
        {
            currentHealth.value = GetComponent<BaseStats>().GetStat(Stat.Health);
            takeDamage.Invoke(0);
        }
        public object CaptureState()
        {
            hasBeenSaved = true;
            return currentHealth.value;
        }
        public float GetPercentage() 
        {
            return Mathf.Round(currentHealth.value/ GetComponent<BaseStats>().GetStat(Stat.Health) * 100);
        }

        public void RestoreState(object state)
        {
            float SaveableHealth = (float)state;
            currentHealth.value = SaveableHealth;
            if (currentHealth.value == 0)
            {
                Death();
            }
            hasRetrievedHealthPoints = true;
            hasRetrievedHealthPoints = true;
        }
    }
}
