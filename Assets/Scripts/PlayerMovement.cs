using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 2.5f;
    public float mouseSensitivity = 100f;
    public LayerMask groundLayer;
    public Transform playerCamera;

    public AudioClip walkSound;
    public AudioSource movementAudioSource;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isCrouching = false;
    private float originalHeight;
    private CapsuleCollider playerCollider;
    private float rotationY = 0f;
    private float rotationX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        originalHeight = playerCollider.height;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        movementAudioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        HandleMovement();
        HandleJumping();
        HandleCrouching();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        float currentSpeed = moveSpeed * (isCrouching ? crouchSpeed : moveSpeed);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        Vector3 move = transform.TransformDirection(moveDirection);

        transform.position += move * currentSpeed * Time.deltaTime;

        if (move != Vector3.zero)
        {
            PlayMovementSound(walkSound);
        }
        else
        {
            movementAudioSource.Stop();
        }
    }

    void HandleJumping()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void HandleCrouching()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            playerCollider.height = isCrouching ? crouchHeight : originalHeight;
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationX -= mouseY;
        rotationY += mouseX;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(0f, rotationY, 0f);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    void FixedUpdate()
    {
        CheckGrounded();
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void PlayMovementSound(AudioClip clip)
    {
        if (!movementAudioSource.isPlaying || movementAudioSource.clip != clip)
        {
            movementAudioSource.clip = clip;
            movementAudioSource.loop = true;
            movementAudioSource.Play();
        }
    }

    public void OnGameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        enabled = false;
    }
}