using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private bool isJumping = false;
    private Rigidbody2D rb;
    //private bool isCollidingWithBox = false;
    ////private GameObject lightBox;
    //private bool isLeftOfBox = false;

    // Recall
    public Material WaterMaterial1;
    public Material WaterMaterial2;
    public Material WaterMaterial3;
    public Material WaterMaterial4;
    public GameObject waterObject1;
    public GameObject waterObject2;
    public Material highlightMaterial;
    private Material originalMaterial;
    private GameObject highlightedObject;
    public float interactDistance = 20f;
    public int RecallActivated = 0;

    //Magnet

    public Vector2 targetPosition = new Vector2(-74f, 4f); // 设置玩家需要到达的位置
    public float proximityThreshold = 2f; // 当玩家与目标位置之间的距离小于此值时，会显示文本
    public Text instructionText; // 在Unity中将InstructionText拖放到这个字段中

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //Magnet
        instructionText.enabled = false; // 初始时隐藏文本
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");

        //If colliding with the light box and player is on the left side and 'D' is pressed
        //if (isCollidingWithBox && isLeftOfBox && Input.GetKey(KeyCode.D))
        //{
        //    // Move both player and light box to the right
        //    rb.velocity = new Vector2(speed, rb.velocity.y);
        //    lightBox.transform.position = new Vector2(lightBox.transform.position.x + speed * Time.deltaTime, lightBox.transform.position.y);
        //}
        //// If colliding with the light box and player is on the right side and 'A' is pressed
        //else if (isCollidingWithBox && !isLeftOfBox && Input.GetKey(KeyCode.A))
        //{
        //    // Move both player and light box to the left
        //    rb.velocity = new Vector2(-speed, rb.velocity.y);
        //    lightBox.transform.position = new Vector2(lightBox.transform.position.x - speed * Time.deltaTime, lightBox.transform.position.y);
        //}
        //else
        //{
        //    // Regular movement without moving the box
        //    rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
        //}
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
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
            RecallActivated = 0;
        }
        if (highlightedObject != null)
        {
            RecallActivated = 1;

        }

        //Magnet

        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        if (distanceToTarget <= proximityThreshold)
        {
            instructionText.enabled = true; // 当玩家接近目标位置时，显示文本
        }
        else if (instructionText.enabled) // 如果玩家远离目标区域，并且文本当前是可见的
        {
            instructionText.enabled = false; // 隐藏文本
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

        if (collision.gameObject.CompareTag("Lava"))
        {
            transform.position = new Vector3(-134.3f, 1.85f);
        }

        //if (collision.gameObject.CompareTag("LightBox"))
        //{
        //    isCollidingWithBox = true;
        //    lightBox = collision.gameObject;

        //    // Check if player is on the left or right side of the box upon collision
        //    isLeftOfBox = transform.position.x < lightBox.transform.position.x;
        //}
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("LightBox"))
    //    {
    //        isCollidingWithBox = false;
    //        lightBox = null;
    //    }
    //}


    // Recall
    void HighlightInteractableObjects()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactDistance);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Recall"))
            {
                // ¸ßÁÁÎïÌå£¬¿ÉÒÔÍ¨¹ýÐÞ¸Ä²ÄÖÊÑÕÉ«µÈ·½Ê½À´ÊµÏÖ
                highlightedObject = collider.gameObject;
                originalMaterial = highlightedObject.GetComponent<Renderer>().material;
                // ÊµÏÖ¸ßÁÁÐ§¹û£¬¸Ä±ä²ÄÖÊÑÕÉ«µÈ
                //highlightedObject.GetComponent<Renderer>().material = highlightMaterial;
                waterObject1.GetComponent<Renderer>().material = WaterMaterial2;
                waterObject2.GetComponent<Renderer>().material = WaterMaterial4;
            }
        }
    }
    void RemoveHighlight()
    {
        if (highlightedObject != null)
        {
            // ÒÆ³ý¸ßÁÁÐ§¹û£¬»¹Ô­²ÄÖÊÑÕÉ«µÈ
            highlightedObject.GetComponent<Renderer>().material = originalMaterial;
            highlightedObject = null;
            waterObject1.GetComponent<Renderer>().material = WaterMaterial1;
            waterObject2.GetComponent<Renderer>().material = WaterMaterial3;
        }
    }
}