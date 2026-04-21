using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpForce = 7f;
    public InputActionReference sprintAction;

    //public AudioSource audioSource;
    //public AudioClip walkClip;
    //public AudioClip sprintClip;
    public float stepRate = 0.4f;
    public float sprintStepRate = 0.25f;

    private Vector2 movementInput;
    private Rigidbody rb;
    private bool isGrounded;
    private float stepTimer;

    //private Note currentNote;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

    public void OnJump(InputValue data)
    {
        if (!isGrounded) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    void MovePlayer()
    {
        bool isSprinting = sprintAction.action.IsPressed();
        Vector3 direction = transform.right * movementInput.x + transform.forward * movementInput.y;

        movementSpeed = isSprinting ? 7.5f : 5f;
        rb.linearVelocity = new Vector3(direction.x * movementSpeed, rb.linearVelocity.y, direction.z * movementSpeed);

        //HandleFootsteps(isSprinting, direction);
    }

    //private void HandleFootsteps(bool isSprinting, Vector3 direction)
    //{
    //    bool isMoving = direction.magnitude > 0.1f && isGrounded;

    //    if (isMoving)
    //    {
    //        stepTimer -= Time.fixedDeltaTime;

    //        if (stepTimer <= 0f)
    //        {
    //            AudioClip clipToPlay = isSprinting ? sprintClip : walkClip;
    //            audioSource.PlayOneShot(clipToPlay);
    //            stepTimer = isSprinting ? sprintStepRate : stepRate;
    //        }
    //    }
    //    else
    //    {
    //        stepTimer = 0f;
    //    }
    //}


    //public void SetCurrentNote(Note note)
    //{
    //    currentNote = note;
    //}


    //public void OnCollectObject()
    //{

    //    if (currentNote != null)
    //    {
    //        currentNote.CollectNote();
    //        currentNote = null;
    //    }
    //}
}