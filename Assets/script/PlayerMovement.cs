using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;
    
    private Rigidbody2D rb;

    public GameObject childObject; // Reference to the child GameObject to be made visible

    private bool hasPressedZ = false;

    public float doubleKeyPressTimeThreshold = 0.5f; // Adjust this threshold as needed

    private bool pressedOnce = false;

    private float timeOfFirstKeyPress;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from the player
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            if (!pressedOnce)
            {
                // This block will be executed only once when the key is pressed
                // Start the timer
                timeOfFirstKeyPress = Time.time;
                pressedOnce = true;
            }
        }
        else
        {
            // Key is released
            pressedOnce = false;
            speed = 5f;
        }

        // Check if the key is pressed again and the timer is within the threshold
        if (pressedOnce && ((Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetKeyDown(KeyCode.RightArrow))) && (Time.time - timeOfFirstKeyPress) < doubleKeyPressTimeThreshold))
        {
            // Double key press detected
            Debug.Log("Double key press detected!");

            // Perform the double key action here
            speed = 10f;

            // Optionally, reset the timer to prevent multiple double key presses in quick succession
            timeOfFirstKeyPress = Time.time;
        }

        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        movement.Normalize();

        rb.velocity = movement * speed;

        if (transform.position.y > 2.0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 2.0f);

            transform.position = new Vector3(transform.position.x, 2.0f, -0.2f);
        }
        if (transform.position.y < -3.4f)
        {
            rb.velocity = new Vector2(rb.velocity.x, -3.4f);

            transform.position = new Vector3(transform.position.x, -3.4f, -0.2f);
        }
        if (transform.position.x < -15.9f)
        {
            rb.velocity = new Vector2(rb.velocity.x, -15.9f);

            transform.position = new Vector3(-15.9f, transform.position.y, -0.2f);
        }
        if (transform.position.x > 15.9f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 15.9f);

            transform.position = new Vector3(15.9f, transform.position.y, -0.2f);
        }

        // Check if the Z key is pressed and the action hasn't been performed yet
        if (Input.GetKeyDown(KeyCode.Z) && !hasPressedZ)
        {
            // Make the child GameObject visible
            if (childObject != null)
            {
                childObject.SetActive(true);
            }

            // Set the flag to indicate that the action has been performed
            hasPressedZ = true;

            // Start a coroutine to make the elbow disappear after a delay
            StartCoroutine(MakeElbowDisappear());
        }

        // Check if the Z key is pressed and the action hasn't been performed yet
        if (Input.GetKeyUp(KeyCode.Z) && hasPressedZ)
        {
            hasPressedZ = false;
        }
    }

    IEnumerator MakeElbowDisappear()
    {
        // Wait for 0.2 seconds
        yield return new WaitForSeconds(0.2f);

        // Make the child GameObject disappear
        if (childObject != null)
        {
            childObject.SetActive(false);
        }
    }
}
