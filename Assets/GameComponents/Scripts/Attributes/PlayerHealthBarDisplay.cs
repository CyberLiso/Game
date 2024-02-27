using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class PlayerHealthBarDisplay : MonoBehaviour
    {
        [SerializeField] Slider MainHealthBar;
        [SerializeField] Slider ResidueHealthBar;
        [SerializeField] float mainHealthBarDropTime;
        [SerializeField] float residueHealthBarDropTime;
        [SerializeField] float residueHealthBarTime;
        private void Start()
        {
            
        }

        public void ResetHealthBarValues(float value)
        {
            MainHealthBar.maxValue = value;
            ResidueHealthBar.maxValue = value;
            MainHealthBar.value = value;
            ResidueHealthBar.value = value;
        }

        public IEnumerator ReduceHealthBarValue(float currentHealth, float maxHealth)
        {
            MainHealthBar.maxValue = maxHealth;
            ResidueHealthBar.maxValue = maxHealth;
            yield return LowerMainHealthBar(currentHealth);

            yield return new WaitForSeconds(residueHealthBarTime);

            yield return LowerResidueHealthBar(currentHealth);
        }

        private IEnumerator LowerMainHealthBar(float value)
        {
            
            while (MainHealthBar.value > value)
            {
                MainHealthBar.value = Mathf.Max(value, MainHealthBar.value -= Time.deltaTime / mainHealthBarDropTime);
                yield return null;
            }
        }
        private IEnumerator LowerResidueHealthBar(float value)
        {

            while (ResidueHealthBar.value > value)
            {
                ResidueHealthBar.value = Mathf.Max(value, ResidueHealthBar.value -= Time.deltaTime / residueHealthBarDropTime);
                yield return null;
            }
        }
    }
}
