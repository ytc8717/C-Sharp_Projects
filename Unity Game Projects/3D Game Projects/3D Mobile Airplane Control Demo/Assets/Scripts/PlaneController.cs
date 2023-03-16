using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public Joystick joystick;

    public float forwardSpeed = 15f;
    public float horizontalSpeed = 4f;
    public float verticalSpeed = 4f;

    public float maxHorizontalRoataion = 0.5f;
    public float maxVerticalRoataion = 0.5f;

    public float smoothness = 5f;
    public float rotationSmoothness = 5f;

    private Rigidbody rb;

    private float horizontalInput;
    private float verticalInput;

    private float forwardSpeedMultiplier = 100f;
    private float speedMultiplier = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) || Input.touches.Length != 0)
        {
            horizontalInput = joystick.Horizontal;
            verticalInput = joystick.Vertical;
        }
        else
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        HandlePlaneRoataion();
    }

    private void FixedUpdate()
    {
        HandlePlaneMovement();
    }

    private void HandlePlaneMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, forwardSpeed * forwardSpeedMultiplier * Time.deltaTime);

        float xVelocity = horizontalInput * speedMultiplier * horizontalSpeed * Time.deltaTime;
        float yVelocity = -verticalInput * speedMultiplier * verticalSpeed * Time.deltaTime;

        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(xVelocity, yVelocity, rb.velocity.z), Time.deltaTime * smoothness);
    }

    private void HandlePlaneRoataion()
    {
        float horizontalRotation = -horizontalInput * maxHorizontalRoataion;
        float verticalRotation = verticalInput * maxVerticalRoataion;

        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(verticalRotation, 
            transform.rotation.y, horizontalRotation,
            transform.rotation.w), Time.deltaTime * rotationSmoothness);
    }
}
