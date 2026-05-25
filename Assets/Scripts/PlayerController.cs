using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public ThirdPersonActionsAsset action;
    private Animator animator;
    private float walkSpeed = 2f;
    private float sprintSpeed = 4f;
    private float jumpSpeed = 4f; 
    private float playerGravity = -9f;
    private float velocity;
    public float radius = 0.5f;
    public float maxDistance = 0.3f;
    public float aboveTransform = 0.25f;
    private Vector2 move;

    void Awake() //Searching access to these
                 //components on Game Object script is attached to
    {
        rb = GetComponent<Rigidbody>();
        action = new ThirdPersonActionsAsset();//Creates new instance of it in memory, it isnt attached to object of player
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        playerMovement();
        jump();
        gravity();
    }

    private void OnEnable()//These are events that can be performed (not part of it) types are listed depending on its use case
    {
        action.Player.Sprint.started += DoSprint;
        action.Player.Sprint.canceled += StopSprint;
        action.Player.Attack.started += DoAttack;
        action.Player.Enable();
    }

    private void DoSprint(InputAction.CallbackContext context)
    {
        if (isGrounded() && action.Player.Move.ReadValue<Vector2>().sqrMagnitude > 0.1)
        {
            animator.SetBool("isRunning", true);
            walkSpeed = sprintSpeed;
            Debug.DrawRay(transform.position + transform.up * aboveTransform, -transform.up, Color.orange, 2f);
        }
    }

    private void StopSprint(InputAction.CallbackContext context)
    {
        animator.SetBool("isRunning", false);
        walkSpeed = 2f;
    }

    private void DoAttack(InputAction.CallbackContext context)
    {
        animator.SetTrigger("attack");
    }

    private void OnDisable()
    {
        action.Player.Sprint.started -= DoSprint;
        action.Player.Sprint.canceled -= StopSprint;
    }

    void playerMovement()
    {
        move = action.Player.Move.ReadValue<Vector2>();
        float Yinput = move.y; //represents the y directional number, which would be 1 or -1 same with Xinput
        float Xinput = move.x; 
   
        Vector3 forwardDirection = Camera.main.transform.forward;//pulling the transform of the camera, forward and right dir
        Vector3 rightDirection = Camera.main.transform.right;
        forwardDirection.y = 0; //set the y direction of that vector to 0 so if player walks in that direction they dont walk into ground
        rightDirection.y = 0;
        forwardDirection = forwardDirection.normalized; //set the magnitude to 1, maintain consistent speed since we're taking out 0
        rightDirection = rightDirection.normalized;

        Vector3 forwardRelativeInput = forwardDirection * Yinput; //scalar multiplication, we take the scalar (player input)
        Vector3 rightRelativeInput = rightDirection * Xinput; //and multiply it by the direction (example , y input * forward dir)
                                                              //y input represents W and S key and forward dir is (0,0,1), right dir is (1,0,0) 
        Vector3 movement = forwardRelativeInput + rightRelativeInput; //Creating a new vector3
       
        if (action.Player.Move.ReadValue<Vector2>().sqrMagnitude > 0.1f)
        {
            rb.rotation = Quaternion.LookRotation(movement, Vector3.up);
        }
        
        if (action.Player.Move.ReadValue<Vector2>().sqrMagnitude < 0.1)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

        Debug.DrawLine(transform.position, transform.position + forwardDirection * 1.5f, Color.blue, 0.01f);
        Debug.DrawLine(transform.position, transform.position + rightDirection * 1.5f, Color.red, 0.01f);
        Debug.DrawLine(transform.position, transform.position + Vector3.up * 1.5f, Color.green, 0.01f);
        Debug.DrawLine(transform.position, transform.position + movement * 1.5f, Color.white, 0.01f);
        transform.Translate(Time.deltaTime * movement * walkSpeed, Space.World); //scalar is here too, movement dir * walkSpeed = new speed
    }

    void jump()
    {
        if (isGrounded() && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            velocity = jumpSpeed; //if conditional met the velocity variable equals jumpSpeed when transforming object, so if no input velocity is 0
            animator.SetTrigger("isJump");
            Debug.DrawRay(transform.position + transform.up * aboveTransform, -transform.up, Color.orange, 2f);
        }
        transform.Translate(new Vector3(0, velocity, 0) * Time.deltaTime);// not a normalized vector, dependent on jumpSpeed float value, so magnitude is definitely above 1
    }

    void gravity()
    {
        if (!isGrounded())
        {
            velocity += playerGravity * Time.deltaTime;
        }
        else
        {
            velocity = 0f;
        }

        Debug.Log(isGrounded());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + transform.up * aboveTransform, radius);

        if (isGrounded())
        {
            Gizmos.DrawSphere(transform.position + -transform.up * maxDistance, radius);
        }
    }

    private bool isGrounded()
    {
        if (Physics.SphereCast(transform.position + transform.up * aboveTransform, radius, -transform.up, out RaycastHit hit, maxDistance))
            return true;
        else
            return false;
    }
}