using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class HealthBarScript : MonoBehaviour
{
    Slider health;
    Health Health;
    float MaxHealth;
    float currentHealth;
    [SerializeField] GameObject slider;
    private void Start()
    {
        health = GetComponentInChildren<Slider>();
        Health = transform.parent.GetComponent<Health>();
        slider.SetActive(false);
    }

    public void SetHealth(float damage)
    {
        slider.SetActive(true);
        MaxHealth = Health.GetInitialHealth();
        currentHealth = Health.currentHealth.value;
        health.maxValue = MaxHealth;
        health.value = currentHealth;
        if(currentHealth <= 0)
        {
            slider.SetActive(false);
        }
    }
    private void Update()
    {

    }
}
 