using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement
{
    public class Move : MonoBehaviour, IAction, IESavaeble
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
            return new SavaebleVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SavaebleVector3 restoredPlayerPosition = (SavaebleVector3)state;
            transform.position = restoredPlayerPosition.ToVector3();
            GetComponent<ActionSchedular>().CancelCurrentAction();
        }
    }
}

 