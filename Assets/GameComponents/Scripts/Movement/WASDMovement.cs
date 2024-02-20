using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
namespace RPG.Movement
{
    public class WASDMovement : MonoBehaviour
    {
        public float moveSpeed = 5f; // Speed of player movement
        public float lookSpeed = 2f; // Speed of camera rotation
        float currentLookSpeed;
        public float jumpForce = 5f; // Force applied when jumping
        private Rigidbody rb;
        private Transform mainCameraTransform;
        Health playerHealth;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            playerHealth = GetComponent<Health>();
            mainCameraTransform = Camera.main.transform;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (playerHealth.IsDead) return;
                // Handle player movement
                MovePlayer();

            // Handle camera follow


            // Handle player interactions
            HandleInteractions();
        }

        void MovePlayer()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = moveVertical * mainCameraTransform.forward +
                                    moveHorizontal * mainCameraTransform.right;
            moveDirection.y = 0f; // Ensure the player stays level with the ground
            moveDirection.Normalize();

            if (moveDirection.magnitude < 0.1f) return;

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentLookSpeed, lookSpeed);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            Vector3 rotatedMoveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            transform.Translate(rotatedMoveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
        }

        void HandleInteractions()
        {
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        bool IsGrounded()
        {
            RaycastHit hit;
            float distanceToGround = 0.1f;
            return Physics.Raycast(transform.position, -Vector3.up, out hit, distanceToGround);
        }
    }}