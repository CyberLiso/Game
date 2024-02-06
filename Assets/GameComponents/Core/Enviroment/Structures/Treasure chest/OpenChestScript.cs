using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChestScript : MonoBehaviour
{
    Animator animator;
    bool isInOpenField = false;
    bool IsOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInOpenField)
        {
            IsOpen = !IsOpen;
            OpenChest(IsOpen);
        }
    }

    private void OpenChest(bool chestState)
    {
        animator.SetBool("ChestIsOpen", chestState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        isInOpenField = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        isInOpenField = false;
    }
}
