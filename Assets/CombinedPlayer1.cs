using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinedPlayer1 : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private bool isJumping = false;
    private Rigidbody2D rb;
    private bool isCollidingWithBox = false;
    private GameObject lightBox;
    private bool isLeftOfBox = false;
    public GameObject textForLight;
    //private Transform playerTransform;


    private SpriteRenderer sr;
    private bool canResize = false;
    private bool canGrowUp = true;
    private bool canGrowDown = false;
    private bool canGrowLeft = true;
    private bool canGrowRight = true;
    private bool canGrow = false; // Start with player not being able to grow
    private float playerMass = 1f;

    public Text resizeHintText;
    
    //Magnet
   
    public Vector2 targetPosition = new Vector2(143f, -67f); // 设置玩家需要到达的位置
    public float proximityThreshold = 2f; // 当玩家与目标位置之间的距离小于此值时，会显示文本
    public Text instructionText; // 在Unity中将InstructionText拖放到这个字段中

    // Recall
    public Material highlightMaterial;
    private Material originalMaterial;
    private GameObject highlightedObject;
    public Vector3 originPosition;
    public float interactDistance = 20f;
    public Text RecallText;
    public int RecallActivated = 0;
    Vector2 targetPositionToRecallText = new Vector2(121f, -67f);



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        resizeHintText.enabled = false; // 初始时设置提示为不可见
        rb.mass = playerMass;

        //playerTransform = this.transform;
        SetTextVisibility(false);

        //Magnet
        instructionText.enabled = false; // 初始时隐藏文本

        // Recall
        RecallText.enabled = false;


    }

    private void Update()
    {

        if (transform.position.x > -5 && transform.position.x < 45)
        {
            SetTextVisibility(true);
        }
        else
        {
            SetTextVisibility(false);
        }

        if (transform.position.x > 157f)
        {
            HandleMovement();
            HandleResize();
        }
        if (transform.position.x <= 157f) { 
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

        // Recall Text
        float distanceToRecallText = Vector2.Distance(transform.position, targetPositionToRecallText);
        if (distanceToRecallText <= 1.0f)
        {
            RecallText.enabled = true;
        }
        if(transform.position.x > 140.0f)
        {
            RecallText.enabled = false;
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

        if (collision.gameObject.CompareTag("Water") || collision.gameObject.CompareTag("Lava") || collision.gameObject.CompareTag("Grass"))
        {
            transform.position = new Vector2(-3.84f, 0f);
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

    private void SetTextVisibility(bool isVisible)
    {
        // If TextForLight is a UI Text object
        if (textForLight.GetComponent<TMPro.TextMeshProUGUI>() != null)
        {
            textForLight.GetComponent<TMPro.TextMeshProUGUI>().enabled = isVisible;
        }

        // If TextForLight has a Renderer (like a SpriteRenderer for 2D games)
        else if (textForLight.GetComponent<Renderer>() != null)
        {
            textForLight.GetComponent<Renderer>().enabled = isVisible;
        }
    }

    private void HandleMovement()
    {
        float xDir = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(xDir * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }


    private void HandleResize()
    {
        if (canResize && canGrow)
        {
            sr.color = Color.yellow;
            CheckDirectionsForWalls();

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Define the boundaries of the current object
                float topBoundary = transform.position.y + transform.localScale.y / 2;
                float bottomBoundary = transform.position.y - transform.localScale.y / 2;
                float leftBoundary = transform.position.x - transform.localScale.x / 2;
                float rightBoundary = transform.position.x + transform.localScale.x / 2;

                bool hasGrown = false; // Track if player has grown

                // Grow Upwards
                if (mousePosition.y > topBoundary && canGrowUp)
                {
                    Debug.Log("Growing Upwards");
                    transform.localScale += new Vector3(0, 1, 0);
                    transform.position += new Vector3(0, 0.5f, 0);
                    hasGrown = true;
                }
                // Grow Downwards
                else if (mousePosition.y < bottomBoundary && canGrowDown)
                {
                    Debug.Log("Growing Downwards");
                    transform.localScale += new Vector3(0, -1, 0);
                    transform.position -= new Vector3(0, 0.5f, 0);
                    hasGrown = true;
                }
                // Grow Left
                else if (mousePosition.x < leftBoundary && canGrowLeft)
                {
                    Debug.Log("Growing Leftwards");
                    transform.localScale += new Vector3(1, 0, 0);  // 在x轴上增加1
                    transform.position -= new Vector3(0.5f, 0, 0);  // 位置左移0.5
                    hasGrown = true;
                }
                // Grow Right
                else if (mousePosition.x > rightBoundary && canGrowRight)
                {
                    Debug.Log("Growing Rightwards");
                    transform.localScale += new Vector3(1, 0, 0);
                    transform.position += new Vector3(0.5f, 0, 0);
                    hasGrown = true;
                }

                // If the player has grown in any direction, reset the ability to grow
                if (hasGrown)
                {
                    canGrow = false;
                    resizeHintText.enabled = false;
                }
            }
        }
        else
        {
            sr.color = Color.white;
        }
    }




    private void CheckDirectionsForWalls()
    {
        // 使用射线检测周围的墙壁
        canGrowUp = !IsBlocked(Vector2.up, transform.localScale.y / 2 + 0.5f);
        canGrowDown = false;  // Always false, because we don't want to grow downwards
        canGrowLeft = !IsBlocked(Vector2.left, transform.localScale.x / 2 + 0.5f);
        canGrowRight = !IsBlocked(Vector2.right, transform.localScale.x / 2 + 0.5f);

    }

    private bool IsBlocked(Vector2 direction, float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);
        return hit.collider != null && hit.collider.tag == "Wall";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player collided with: " + collision.gameObject.name);

        if (collision.gameObject.tag == "Gem")
        {
            Debug.Log("Gem detected");
            canResize = true;
            canGrow = true;
            resizeHintText.enabled = true;

            playerMass *= 2;  // 玩家的质量翻倍
            rb.mass = playerMass;

            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Stone")
        {
            Rigidbody2D stoneRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (stoneRb)
            {
                // 检查玩家的质量是否大于等于石头的质量
                if (playerMass >= stoneRb.mass)
                {
                    stoneRb.constraints = RigidbodyConstraints2D.None;  // 允许石头移动
                }
                else
                {
                    stoneRb.constraints = RigidbodyConstraints2D.FreezeAll;  // 冻结石头，防止玩家移动它
                }
            }
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

