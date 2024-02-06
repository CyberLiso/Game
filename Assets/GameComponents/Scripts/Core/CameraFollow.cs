using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {
            FixCameraToPlayer();
        }
        /// <summary>
        /// The method synchronizes the player and camera positions
        /// </summary>
        private void FixCameraToPlayer()
        { 
            transform.position = target.position;
        }
    }
}
