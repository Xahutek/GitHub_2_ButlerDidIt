using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Walk();   
    }
    void Walk()
    {
        bool isMovingHorizontally = controller.isMovingHorizontally != 0;

        if (!GameManager.isPaused)
        {
            if (isMovingHorizontally && controller.grounded)
            {
                anim.SetBool("IsWalking", true);
            }
            if (!isMovingHorizontally)
            {
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsRunning", false);
                anim.SetBool("IsIdle", true);
            }
            if (isMovingHorizontally)
            {
                anim.SetBool("IsIdle", false);
            }
            if (isMovingHorizontally && controller.Input.Sprinting)
            {
                anim.SetBool("IsRunning", true);
            }
            if (controller.Input.Sprinting == false)
            {
                anim.SetBool("IsRunning", false);
            }

            if (!controller.grounded||controller.Input.Jumping)
            {
                anim.SetBool("IsJumping", true);
            }
            if (controller.grounded||!controller.Input.Jumping)
            {
                anim.SetBool("IsJumping", false);
            }
        }
        else
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsIdle", true);
        }
    }

}
