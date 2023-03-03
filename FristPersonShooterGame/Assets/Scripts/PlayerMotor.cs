using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool lerpCrouch;
    private bool crouching;
    private bool sprinting;

    public float speed = 5f;
    public float jumpHight = 3f;
    public float crouchTimer = 2f;
    public float gravity = -9.8f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1f;
            p *= p;

            if(crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if(p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }    
        }
    }

    //recive input from InputManger.cs and applay them to the CharacterController.
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirction = Vector3.zero;
        moveDirction.x = input.x;
        moveDirction.z = input.y;
        controller.Move(transform.TransformDirection(moveDirction) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;

        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        controller.Move(playerVelocity * Time.deltaTime);

    }

    //Handeling Jump
    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHight * -3.0f * gravity);
        }
    }

    //Handeling Crouch
    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0f;
        lerpCrouch = true;
    }

    //Handeling Sprinting
    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
            speed = 8f;
        else
            speed = 5f;
    }
}
