using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {

        [Header("Suspicion mechanic")]
        [Range(0, 20)] [SerializeField] float distanceToGiveChase = 5f;
        [Range(0, 20)] [SerializeField] float suspicionTime = 5f;
        private float timeSinceLastSawPlayer = Mathf.Infinity;

        [Header("Patrolling mechanic")]
        [SerializeField] PatrolPathLogic PatrolPath;
        [Range(0, 20)] [SerializeField] float WaypointTollerance = 1f;
        [Range(0, 20)] [SerializeField] float DwellingTime = 2.5f;
        [Range(0, 1)] [SerializeField] float patrollingSpeedFraction = 0.3f;

        [Header("BaseStats")]
        //[Range(0.5f, 1)] [SerializeField] float ChaseSpeedFraction;
        private float timeSpentOnPoint;
        private int index = 0;

        NavMeshAgent navMeshAgent;
        GameObject player;
        FightInitiator fightInitiator;
        Health health;
        Move moveComponent;
        private bool isAttacking = false;

        Vector3 guardLocation;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("Player");
            fightInitiator = GetComponent<FightInitiator>();
            health = GetComponent<Health>();
            moveComponent = GetComponent<Move>();
            guardLocation = transform.position;
        }
        // Update is called once per frame
        void Update()
        {
            if (health.IsDead) return;
            if (PlayerIsInRange() && fightInitiator.CanAttack(player))
            {
                Attack();
                isAttacking = true;
                return;
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionWait();
            }
            else
            {
                StopAttacking();
                isAttacking = false;
            }
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void SuspicionWait()
        {
            GetComponent<ActionSchedular>().CancelCurrentAction();
        }

        private bool PlayerIsInRange()
        {
            return PlayerDistance() <= distanceToGiveChase;
        }

        public float PlayerDistance()
        {
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            return playerDistance;
        }

        public void Attack()
        {
            timeSinceLastSawPlayer = 0f;
            fightInitiator.Attack(player);
        }
        
        public void StopAttacking()
        {
            PatrollingBehaviour();
        }

        private void PatrollingBehaviour()
        {
            Vector3 nextPosition = guardLocation;
            if (PatrolPath!= null)
            {
                if (AtWaypoint())
                {
                    if (timeSpentOnPoint >= DwellingTime)
                    {
                        CycleWaypoints();
                    }
                    else
                    {
                        timeSpentOnPoint += Time.deltaTime;
                    }
                }
                nextPosition = GetCurrentWaypoint();
            }

            moveComponent.MoveTo(nextPosition, Mathf.Clamp01(patrollingSpeedFraction));

        }

        private void CycleWaypoints()
        {
            timeSpentOnPoint = 0;
            index = PatrolPath.GetNextWaypoint(index);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return PatrolPath.GetWaypoint(index);
        }

        private bool AtWaypoint()
        { 
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < WaypointTollerance;
        }

        public void OnDrawGizmos()
        {
            if (isAttacking) Gizmos.color = Color.red; else Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, distanceToGiveChase);
        }
    }
}
