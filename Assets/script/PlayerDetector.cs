using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public bool heroIsNearby;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            heroIsNearby = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            heroIsNearby = false;
        }
    }
}
