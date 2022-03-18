using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2Movement : MonoBehaviour
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

    public Animator anim;

    public float timer;

    void Start()
    {
        Cursor.visible = false;

        timer = Random.Range(5f, 20f);
    }

    void Update()
    {
        if (swapScript.player2Active == false)
        {
            controller.enabled = false;
        }

        if (swapScript.player2Active == true)
        {
            controller.enabled = true;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f && swapScript.player2Active == true)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * playerSpeed * Time.deltaTime);

            anim.SetBool("Jogging", true);

            timer -= Time.deltaTime;
            if(timer < 0)
            {
                anim.SetTrigger("Stumble");
                timer = Random.Range(5f, 20f);
            }
        }

        else
        {
            anim.SetBool("Jogging", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            
            anim.SetTrigger("Jump");
        }

        velocity.y += gravity * Time.deltaTime;
        if (swapScript.player2Active == true)
        {
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            Debug.Log("DIED!");
            SceneManager.LoadScene("GameOverScreen");
        }
    }
}
