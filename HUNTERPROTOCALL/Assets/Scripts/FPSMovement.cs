using UnityEngine;
using UnityEngine.InputSystem;

public class FPSMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 2.2f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation;

    void Awake()
    {
        // Always make sure a CharacterController exists
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
            controller.height = 1.8f;
            controller.center = new Vector3(0, 0.9f, 0);
            controller.radius = 0.3f;
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleMovement()
    {
        // --- Ground check FIRST ---
        bool grounded = controller.isGrounded;

        if (grounded && velocity.y < 0)
            velocity.y = -2f;

        // --- Input ---
        float x = 0f;
        float z = 0f;

        if (Keyboard.current.aKey.isPressed) x -= 1;
        if (Keyboard.current.dKey.isPressed) x += 1;
        if (Keyboard.current.wKey.isPressed) z += 1;
        if (Keyboard.current.sKey.isPressed) z -= 1;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // --- Jump ---
        if (grounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // --- Gravity ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        if (Mouse.current == null || cameraTransform == null)
            return;

        Vector2 mouse = Mouse.current.delta.ReadValue() * mouseSensitivity;

        xRotation -= mouse.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouse.x);
    }
}
