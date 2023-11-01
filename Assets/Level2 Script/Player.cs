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

    // Recall
    public Material highlightMaterial;
    private Material originalMaterial;
    private GameObject highlightedObject;
    public float interactDistance = 20f;
    public int RecallActivated = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");

        //If colliding with the light box and player is on the left side and 'D' is pressed
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
        //rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
        // Jumping
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }

        // Recall Operation
        if (Input.GetKey(KeyCode.J))
        {
            HighlightInteractableObjects();
        }
        else
        {
            RemoveHighlight();
        }
        if (highlightedObject != null)
        {
            RecallActivated = 1;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isJumping = false;
        }

        if (collision.gameObject.CompareTag("Sand"))
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
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LightBox"))
        {
            isCollidingWithBox = false;
            lightBox = null;
        }
    }

    // Recall
    void HighlightInteractableObjects()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactDistance);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Recall"))
            {
                // 高亮物体，可以通过修改材质颜色等方式来实现
                highlightedObject = collider.gameObject;
                originalMaterial = highlightedObject.GetComponent<Renderer>().material;
                // 实现高亮效果，改变材质颜色等
                //highlightedObject.GetComponent<Renderer>().material = highlightMaterial;

            }
        }
    }
    void RemoveHighlight()
    {
        if (highlightedObject != null)
        {
            // 移除高亮效果，还原材质颜色等
            highlightedObject.GetComponent<Renderer>().material = originalMaterial;
            highlightedObject = null;
            RecallActivated = 0;
        }
    }
}