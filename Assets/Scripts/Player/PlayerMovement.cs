using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float playerSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Transform cam;

    Vector3 velocity;
    bool isGrounded;

    public PlayerSwap swapScript;

    public Animator cageAnimator;
    bool dead = false;
    public GameObject cage;

    public GameObject restartText;
    MeshRenderer text;

    void Start()
    {
        Cursor.visible = false;
        cage.SetActive(false);

        text = restartText.GetComponent<MeshRenderer>();
        text.enabled = false;
    }

    void Update()
    {
        if (swapScript.player1Active == false)
        {
            controller.enabled = false;
        }

        if (swapScript.player1Active == true && dead == false)
        {
            controller.enabled = true;
        }
            else
            {
                controller.enabled = false;
            }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f && swapScript.player1Active == true && dead == false)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * playerSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        if (swapScript.player1Active == true)
        {
            controller.Move(velocity * Time.deltaTime);
        }
    }

    public void Captured()
    {
        if (dead == false)
        {
            dead = true;
            cage.SetActive(true);
            controller.enabled = false;
            cageAnimator.SetBool("Captured", true);

            text.enabled = true;
        }
    }
}
