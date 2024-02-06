using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.SceneManagment
{
    public class CinemachineControl : MonoBehaviour
    {
        GameObject Player;
        private bool cutsceneIsRunning = false;
        void OnCutsceneFinished(PlayableDirector aha)
        {
            ReturnPlayerControl();
            cutsceneIsRunning = false;
        }
        void OnCutsceneBegan(PlayableDirector aha)
        {
            StopPlayerControl();
            cutsceneIsRunning = true;
        }
        void ReturnPlayerControl()
        {
            Player.GetComponent<MovementController>().enabled = true;
        }

        void StopPlayerControl()
        {
            Player.GetComponent<ActionSchedular>().CancelCurrentAction();
            Player.GetComponent<MovementController>().enabled = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<PlayableDirector>().played += OnCutsceneBegan;
            GetComponent<PlayableDirector>().stopped += OnCutsceneFinished;
            Player = GameObject.FindWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            SkipCutsceneAwait();
        }

        private void SkipCutsceneAwait()
        {
            if (cutsceneIsRunning)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GetComponent<PlayableDirector>().Stop();
                }
            }
        }
    }
}
