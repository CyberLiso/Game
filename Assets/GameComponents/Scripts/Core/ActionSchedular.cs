using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionSchedular : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        public IAction currentActionRunning;
        public void StartAction(IAction action)
        {
            if (currentActionRunning == action) return;
            if (currentActionRunning != null)
            {
                currentActionRunning.Cancel();
            }
            currentActionRunning = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
