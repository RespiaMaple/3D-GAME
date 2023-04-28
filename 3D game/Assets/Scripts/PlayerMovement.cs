using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    [SerializeField] Transform orientation;
    public float bounceForce = 30f;

    [Header("PlayerMovement")]
    public float moveSpeed = 6f;
    public float moveMultiplier = 10f;
    [SerializeField] float airMultiplier = 0.2f;

    [Header("Sprint")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jump")]
    public float jumpForce = 5f;

    [Header("Crouch")]
    public float crouchSpeed = 2f;
    public float crouchYSclae;
    public float startSclae;

    [Header("Drag")]
    public float groundDrag = 6f;
    public float airDrag = 2f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    bool isGrounded;
    float groundDistance = 0.4f;

    float playerHeight = 2f;

    float horizontalMovement;
    float verticalMovement;


    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    public Rigidbody rb;

    RaycastHit slopeHit;

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startSclae = transform.localScale.y;
    }

    private void Update() 
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance,groundLayer); //CheckSphere(Vector3 position, float radius, int layerMask)

        MyInput();
        ControlDrag();
        Sprint();
        Crouch();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }


        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);//Vector3.ProjectOnPlane(要投影的向量,斜面的法向量)
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f)) //out參數修飾詞，獲得方法的返回值
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "JumpPad")
        {
            rb.velocity = new Vector3(rb.velocity.x, bounceForce, rb.velocity.z);
        }
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * Mathf.Sqrt (jumpForce * -2f * Physics.gravity.y), ForceMode.Impulse);
    }

    void Crouch()
    {
        if (Input.GetKey(crouchKey) && isGrounded)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYSclae, transform.localScale.z);
            rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
            moveSpeed = crouchSpeed;
        }
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startSclae, transform.localScale.z);
            moveSpeed = walkSpeed;
        }
    }

    void Sprint()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime); //Mathf.Lerp(起始值,結束值,插值)
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate() 
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration); //Vector3.normalized使向量變成單位向量
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

}
