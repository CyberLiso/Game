using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Attributes;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class MovementController : MonoBehaviour
    {
        Health health;
        [Range(0, 1)] [SerializeField] float playerSpeed = 0.6f;
        [SerializeField] float MaxMoveDistance;
        [SerializeField] CursorClass[] CursorsForDifferentActions;
        CursorModes CurrentCursorMode;

        // Start is called before the first frame update

        private void Awake()
        {
            health = GetComponent<Health>();
            CurrentCursorMode = CursorModes.Disabled;
        }

        private void Start()
        {
            SetCursor(CurrentCursorMode);
        }
        // Update is called once per frame
        void Update()
        {
            InitiatePlayerMovement();
            if (!GetComponent<Health>().IsDead)
            {
                GetComponent<NavMeshAgent>().enabled = true;
            }
        }

        private void InitiatePlayerMovement()
        {
           /* if (IsInteractingWithUI())
            {
                SetCursor(CursorModes.UI);
                return;
            }
            if (health.IsDead)
            {
                SetCursor(CursorModes.Dead);
                return;
            }
            if (InteractWithComponents()) return;
            if (CheckForMouseInput())
            {
                return;
            }
            SetCursor(CursorModes.Disabled);
           */
        }

        private bool InteractWithComponents()
        {
            RaycastHit[] hits = GetSortedRaycasts();
            foreach (RaycastHit hit in hits)
            {
                IRayCastable[] CastableComponents = hit.transform.GetComponents<IRayCastable>();
                foreach (IRayCastable component in CastableComponents)
                {
                    if (component.CanHandleRaycast(this))
                    {
                        SetCursor(component.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] GetSortedRaycasts()
        {
            RaycastHit[] hits = Physics.RaycastAll(ConvertMousePosToRay());
            float[] distancesFromHits = new float[hits.Length];

            for(int i = 0; i < hits.Length; i++)
            {
                distancesFromHits[i] = Vector3.Distance(gameObject.transform.position, hits[i].transform.position);
            }

            Array.Sort<float, RaycastHit>(distancesFromHits, hits);

            return hits;
            
        }

        private bool IsInteractingWithUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }


        private void SetCursor(CursorModes cursorMode)
        {
            if (CurrentCursorMode == cursorMode)
            {
                return;
            }
            else
            {
                CursorClass cursor = GetCursorClass(cursorMode);
                CurrentCursorMode = cursorMode;
                Cursor.SetCursor(cursor.cursorTexture, cursor.cursorPosition, CursorMode.Auto);
            }
        }

        [System.Serializable]
        struct CursorClass
        {
            public CursorModes actionMode;
            public Texture2D cursorTexture;
            public Vector2 cursorPosition;
        }

        private CursorClass GetCursorClass(CursorModes Mode)
        {
            foreach (CursorClass cursor in CursorsForDifferentActions)
            {
                if(cursor.actionMode == Mode)
                {
                    return cursor;
                }
            }

            return new CursorClass();
        }

        private bool CheckForMouseInput()
        {
            Debug.DrawRay(ConvertMousePosToRay().origin, ConvertMousePosToRay().direction * 100000);

            //This casts a ray which upon collision sends the point at which it collided
            RaycastHit hit;
            bool hasRayHit = Physics.Raycast(ConvertMousePosToRay(), out hit);

            //The player moves to that point if a collision does occur
            if (hasRayHit)
            {
                if (GetComponent<Move>().DoesPathExist(hit.point, MaxMoveDistance))
                {
                    SetCursor(CursorModes.Default);
                    if (Input.GetMouseButton(0))
                    {
                        GetComponent<Move>().PassiveMoveTo(hit.point, playerSpeed);
                    }
                }
                else
                {
                    SetCursor(CursorModes.Disabled);
                }
                //We access the players Move script.
            return true;
            }
            return false;
        }

        private static Ray ConvertMousePosToRay()
        {
            //Scene Raycast debug tools:
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
