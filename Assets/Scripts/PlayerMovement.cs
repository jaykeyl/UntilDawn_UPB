using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 7.5f;
    public float jumpForce = 7f;

    private Vector2 movementInput;
    private Rigidbody rb;
    private bool isGrounded;
    private bool isSprinting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    public void OnMovement(InputValue data)
    {
        movementInput = data.Get<Vector2>();
    }

    public void OnSprint(InputValue data)
    {
        isSprinting = data.isPressed;
    }

    public void OnJump(InputValue data)
    {
        if (!data.isPressed) return;
        if (!isGrounded) return;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    void MovePlayer()
    {
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 direction = transform.right * movementInput.x + transform.forward * movementInput.y;
        direction = direction.normalized;

        rb.linearVelocity = new Vector3(
            direction.x * currentSpeed,
            rb.linearVelocity.y,
            direction.z * currentSpeed
        );
    }
}