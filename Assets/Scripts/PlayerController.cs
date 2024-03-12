using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpHeight = 4f;
    public float gravity = 5f;
    public float airControl = 5f;

    CharacterController controller;
    Animator animator;
    Vector3 input, moveDir;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        animator.SetInteger("WeaponType_int", 0);
        animator.SetFloat("Speed_f", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = transform.right * moveHorizontal + transform.forward * moveVertical;
        input *= moveSpeed;

        if (input != Vector3.zero)
        {
            animator.SetFloat("Speed_f", 0.3f);
        } else
        {
            animator.SetFloat("Speed_f", 0.0f);
        }

        if (transform.position.y <= 0.5f)
        {
            moveDir = input;

            if (Input.GetButton("Jump"))
            {
                moveDir.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                animator.SetBool("Jump_b", true);
            }
            else
            {
                moveDir.y = 0.0f;
                animator.SetBool("Jump_b", false);
            }

        }
        else
        {
            // midair
            animator.SetBool("Jump_b", false);
            input.y = moveDir.y;
            moveDir = Vector3.Lerp(moveDir, input, airControl * Time.deltaTime);
        }

        moveDir.y -= gravity * Time.deltaTime;

        controller.Move(moveDir * Time.deltaTime);
    }
}
