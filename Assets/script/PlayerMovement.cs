using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;
    
    private Rigidbody2D rb;

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
    }
}
