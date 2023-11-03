using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tground : MonoBehaviour
{
    public float breakForceThreshold = 50f; // 破坏阈值

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > breakForceThreshold)
        {
            BreakWall();
        }
    }

    void BreakWall()
    {
        // Add visual and audio effects for breaking the wall
        Destroy(gameObject);
    }
}
