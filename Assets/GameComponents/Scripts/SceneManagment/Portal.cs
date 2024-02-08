using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Core;
using RPG.Control;

namespace RPG.SceneManagment
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneIndex = -1;
        [SerializeField] Transform portalSpawnPoint;
        [Range(0, 6)] [SerializeField] float fadeOutTime = 3f;
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
            if(sceneIndex < 0)
            {
                Debug.LogError("Scene has not been set!");
                yield break;
            }
            Fader fader = FindObjectOfType<Fader>();
            EditPlayerControl(false);
            yield return fader.FadeOut(fadeOutTime);
            DontDestroyOnLoad(gameObject);
            SavableWrapping saver = FindObjectOfType<SavableWrapping>();
            saver.ResaveSaveFile();
            yield return SceneManager.LoadSceneAsync(sceneIndex);
            EditPlayerControl(false);
            saver.LoadSaveFile();
            Portal NextPortal = GetPortal();
            UpdatePlayerPosition(NextPortal);
            saver.ResaveSaveFile();
            yield return new WaitForSeconds(timeToWaitInbetweenScenes);
            yield return fader.FadeIn(fadeOutTime);
            EditPlayerControl(true);
            Destroy(gameObject);
        }

        private void UpdatePlayerPosition(Portal nextPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(nextPortal.portalSpawnPoint.transform.position);
        }

        public void EditPlayerControl(bool returnPlayerControl)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<MovementController>().enabled = returnPlayerControl;
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
