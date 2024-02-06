using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Control
{
    public class PatrolPathLogic : MonoBehaviour
    {
        [Range(0, 10)] [SerializeField] float WaypointRadius = 2f;
        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextWaypoint(i);
                Gizmos.DrawSphere(GetWaypoint(i), WaypointRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextWaypoint(int i)
        {
            if (i  == transform.childCount - 1)
            {
                return 0;
            }
            else
            {
                return i + 1;
            }
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).transform.position;
        }
    }
}

