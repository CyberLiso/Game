using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.SceneManagment
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneIndex = -1;
        [SerializeField] Transform portalSpawnPoint;
        [Range(0, 6)] [SerializeField] float fadeOutTime = 3f;
        [Range(0, 6)] [SerializeField] float fadeInTime = 3f;
        [Range(0, 2)] [SerializeField] float timeToWaitInbetweenScenes = 0f;

        enum portalDestinations
        {
            A, B, C, D
        }

        [SerializeField] portalDestinations portalIdentifier;
        [SerializeField] portalDestinations thisPortal;

        private void Start()
        {
            
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(PortalLoadWait());
            }
        }

        private IEnumerator PortalLoadWait()
        {
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            yield return fader.FadeOut(fadeOutTime);
            DontDestroyOnLoad(gameObject);
            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneIndex);
            yield return new WaitForSeconds(1f);
            savingWrapper.Load();
            Portal NextPortal = GetPortal();
            UpdatePlayerPosition(NextPortal);
            savingWrapper.Save();
            yield return new WaitForSeconds(timeToWaitInbetweenScenes);
            yield return fader.FadeIn(fadeOutTime);
            Destroy(gameObject);
        }

        private void UpdatePlayerPosition(Portal nextPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(nextPortal.portalSpawnPoint.transform.position);
        }

        private Portal GetPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal.thisPortal == portalIdentifier) return portal;
            }
            return null;
        }
    }
}
