using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private bool isJumping = false;
    private Rigidbody2D rb;
    private bool isCollidingWithBox = false;
    private GameObject lightBox;
    private bool isLeftOfBox = false;

    // For WallTouching Effect
    public AudioClip wallTouchSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");

        // If colliding with the light box and player is on the left side and 'D' is pressed
        if (isCollidingWithBox && isLeftOfBox && Input.GetKey(KeyCode.D))
        {
            // Move both player and light box to the right
            rb.velocity = new Vector2(speed, rb.velocity.y);
            lightBox.transform.position = new Vector2(lightBox.transform.position.x + speed * Time.deltaTime, lightBox.transform.position.y);
        }
        // If colliding with the light box and player is on the right side and 'A' is pressed
        else if (isCollidingWithBox && !isLeftOfBox && Input.GetKey(KeyCode.A))
        {
            // Move both player and light box to the left
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            lightBox.transform.position = new Vector2(lightBox.transform.position.x - speed * Time.deltaTime, lightBox.transform.position.y);
        }
        else
        {
            // Regular movement without moving the box
            rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isJumping = false;
        }

        if (collision.gameObject.CompareTag("LightBox"))
        {
            isCollidingWithBox = true;
            lightBox = collision.gameObject;

            // Check if player is on the left or right side of the box upon collision
            isLeftOfBox = transform.position.x < lightBox.transform.position.x;
        }

        if (collision.gameObject.CompareTag("LeftWall"))
        {
            PlayWallTouchSound(-1);
        }
        else if (collision.gameObject.CompareTag("RightWall"))
        {
            PlayWallTouchSound(1);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LightBox"))
        {
            isCollidingWithBox = false;
            lightBox = null;
        }
    }

    private void PlayWallTouchSound(float pan)
    {
        audioSource.panStereo = pan;
        audioSource.PlayOneShot(wallTouchSound);
    }

}
