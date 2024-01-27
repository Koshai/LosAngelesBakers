using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5f;
    public float followThreshold = 1f; // Adjust this to set the distance threshold for camera follow

    void Update()
    {
        if (player != null)
        {
            // Calculate the distance between the player and the center of the camera
            float distanceToPlayer = Mathf.Abs(player.position.x - transform.position.x);

            // Check if the distance exceeds the follow threshold
            if (distanceToPlayer > followThreshold)
            {
                // Calculate the target position for the camera
                Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);

                // Smoothly interpolate the current position towards the target position
                transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

                if(transform.position.x < -6.72f)
                {
                    transform.position = new Vector3(-6.72f, transform.position.y, transform.position.z);
                }
                if(transform.position.x > 6.71f)
                {
                    transform.position = new Vector3(6.71f, transform.position.y, transform.position.z);
                }
            }
        }
    }
}
