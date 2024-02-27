using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.SceneManagment;

namespace RPG.Core
{
    public class SavableWrapping : MonoBehaviour
    {
        const string saveFileName = "Save";
        [SerializeField] float fadeInTime = 1f;
        // Start is called before the first frame update

        IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(saveFileName);
            Fader fader = FindObjectOfType<Fader>();
            fader.InstantFadeOut();
            yield return fader.FadeIn(fadeInTime);
        }
        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        // Update is called once per frame
        void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadSaveFile();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                ResaveSaveFile();
            }
            */
            if (Input.GetKeyDown(KeyCode.P))
            {
                DeleteSaveFile();
            }
            
        }

        private void DeleteSaveFile()
        {
            GetComponent<SavingSystem>().Delete(saveFileName);
        }

        public void ResaveSaveFile()
        {
            print("haallo");
            GetComponent<SavingSystem>().Save(saveFileName);
        }

        public void LoadSaveFile()
        {
            print("byye");
            GetComponent<SavingSystem>().Load(saveFileName);
        }
    }
}