using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.SceneManagment
{
    public class CinemachineTrigger : MonoBehaviour
    {
        bool hasBeenTriggered = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !hasBeenTriggered)
            {
                GetComponent<PlayableDirector>().Play();
                hasBeenTriggered = true;
            }
        }
    }
}
