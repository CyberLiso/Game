using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.SceneManagment;

namespace RPG.Movement
{
    public class Move : MonoBehaviour, IAction, ISaveable
    {

        //Variables:

        public NavMeshAgent player;
        public Animator movementBlendTree;
        public float maxSpeed = 10f;

        Ray lazerbeam;
        public float rayDistance;

        // Start is called before the first frame update
        void Start()
        {
            //Gets the player's nav mesh component
        }

        private void Awake()
        {
            player = GetComponent<NavMeshAgent>();
        }
        public void Cancel()
        {
            player.isStopped = true;
        }

        // Update is called once per frame
        void Update()
        {
            BlendPlayerMovementAnimations();
        }

        private void BlendPlayerMovementAnimations()
        {
            movementBlendTree.SetFloat("Blend", transform.InverseTransformDirection(player.velocity).z);
        }

        public void PassiveMoveTo(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionSchedular>().StartAction(this);
            MoveTo(destination, speedFraction);
           // GetComponent<FightInitiator>().Cancel();
        }
        public void MoveTo(Vector3 destination,float speedFraction)
        {
            player.speed = maxSpeed * speedFraction;
            player.destination = destination;
            player.isStopped = false;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public bool DoesPathExist(Vector3 hit, float maxDistance)
        {
            NavMeshPath path = new NavMeshPath();
            if(!player.isOnNavMesh || player == null) return false;
            bool pathExists = player.CalculatePath(hit, path);
            float pathDistance = 0f;
            for(int i  = 1; i < path.corners.Length; i++)
            {
                pathDistance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
            return pathDistance <= maxDistance && pathExists;
        }

        public void RestoreState(object state)
        { 
            SerializableVector3 savedPlayerPosition = (SerializableVector3)state;
            GetComponent<ActionSchedular>().CancelCurrentAction();
            player.enabled = false;
            transform.position = savedPlayerPosition.ToVector();
            player.enabled = true;
        }

    }
}

 