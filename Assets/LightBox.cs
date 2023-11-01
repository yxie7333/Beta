using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBox : MonoBehaviour
{
    public float minX = -5f;
    public float maxX = 70f;

    private GameObject player;
    private Rigidbody2D rb;
    private Rigidbody2D playerRb;
    private bool isCollidingWithPlayer = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
        transform.position = currentPosition;

        if (isCollidingWithPlayer)
        {
            // If the player is to the left of the ice and is pushing right
            if (player.transform.position.x < transform.position.x && playerRb.velocity.x > 0)
            {
                rb.velocity = new Vector2(playerRb.velocity.x, rb.velocity.y);
            }
            // If the player is to the right of the ice and is pushing left
            else if (player.transform.position.x > transform.position.x && playerRb.velocity.x < 0)
            {
                rb.velocity = new Vector2(playerRb.velocity.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    Debug.Log("Object entered the trigger!");
    //}

    //void FixedUpdate()
    //{
    //    Vector3 currentPos = transform.position;
    //    currentPos.y = -2.49f;
    //    transform.position = currentPos;
    //}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == player)
        {
            isCollidingWithPlayer = true;
        }

        //if (other.gameObject.CompareTag("Lava"))
        //{
        //    Destroy(other.gameObject, 1f); // Destroy the lava
        //    Destroy(this.gameObject, 1.1f);
        //}
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject == player)
        {
            isCollidingWithPlayer = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

}

