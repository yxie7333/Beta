﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Proyecto26;
using static CombinedPlayer1;

public class CombinedPlayer1 : MonoBehaviour
{
    // checkpoint
    public CheckPoint CheckPointScript;

    public float speed = 5f;
    public float jumpForce = 7f;
    private bool isJumping = false;
    private Rigidbody2D rb;
    //private bool isCollidingWithBox = false;
    //private GameObject lightBox;
    //private bool isLeftOfBox = false;
    public GameObject textForLight;
    public GameObject textForBox;
    public GameObject textForGrass;
    public GameObject textForIce;
    public GameObject textForLava;
    public LayerMask groundLayer; // assign the Ground layer here in the editor
    public float groundedRayLength = 0.1f;


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
    public Text weightHintText;

    //Magnet

    public Vector2 targetPosition = new Vector2(149f, -67f); // 设置玩家需要到达的位置
    public float proximityThreshold = 2f; // 当玩家与目标位置之间的距离小于此值时，会显示文本
    public Text instructionText; // 在Unity中将InstructionText拖放到这个字段中
    public Vector2 targetPosition2 = new Vector2(153f, -63f); // 设置玩家需要到达的位置
    public float proximityThreshold2 = 2f; // 当玩家与目标位置之间的距离小于此值时，会显示文本
    public Text instruction2Text; // 在Unity中将Instruction2Text拖放到这个字段中

    // Recall
    public Material WaterMaterial1;
    public Material WaterMaterial2;
    public GameObject waterObject;
    public Material highlightMaterial;
    private Material originalMaterial;
    private GameObject highlightedObject;
    public Vector3 originPosition;
    public float interactDistance = 20f;
    public Text RecallText;
    public int RecallActivated = 0;
    Vector2 targetPositionToRecallText = new Vector2(121f, -67f);

    // sound
    public int eatenGemCount = 0;
    public SpriteMask mask;


    // analytics
    private string playerID = System.Guid.NewGuid().ToString();
    // metric 1
    private Vector3 lastPlayerPosition;
    private float analyticTime = 0.0f;
    private float currentTime = 0.0f;

    //Triangle Display
    // private TriangleController triangleController;
    public GameObject[] arrows;
    public float arrowDistance = 1f;

    [System.Serializable]
    public class AnalyticPath
    {
        public string tick;
        public AnalyticPosition position;
    }

    [System.Serializable]
    public class AnalyticPosition
    {
        public float x;
        public float y;
    }
    // metric 2
    private int resizeDirection = 0;
    private int resizeCount = 0;


    [System.Serializable]
    public class AnalyticShape
    {
        public string resizeCount;
        public string resizeDirection;
    }


    void Awake()
    {

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        resizeHintText.enabled = false; // 初始时设置提示为不可见
        rb.mass = playerMass;
        if (weightHintText != null)
        {
            weightHintText.enabled = false;
        }
        // sound
        eatenGemCount = 0;

        //playerTransform = this.transform;
        if (textForBox != null)
        {
            SetTextVisibility(false, textForBox);
        }
        if (textForLight != null)
        {
            SetTextVisibility(false, textForLight);
        }
        if (textForGrass !=  null)
        {
            SetTextVisibility(false, textForGrass);
        }
        if (textForIce != null) 
        {
            SetTextVisibility(false, textForIce);
        }
        if (textForLava != null)
        {
            SetTextVisibility(false, textForLava);
        }


        //Magnet
        instructionText.enabled = false; // 初始时隐藏文本
        instruction2Text.enabled = false;

        // Recall
        RecallText.enabled = false;

        // Analytics
        lastPlayerPosition = transform.position;

        //TriangleController
        // triangleController = GetComponent<TriangleController>();
        SetArrowsActive(false); // Ensure arrows are inactive at the start

    }

    private void Update()
    {
        // for element UI Text
        if (textForBox != null && textForLight != null && textForGrass != null && textForIce != null && textForLava!=null)
        {
            if (transform.position.x > -8f && transform.position.x < -3.2f)
            {
                SetTextVisibility(true, textForBox);
            }
            else
            {
                SetTextVisibility(false, textForBox);
            }
            if (transform.position.x > -3.2f && transform.position.x < 0f)
            {
                SetTextVisibility(true, textForLight);
                SetTextVisibility(true, textForGrass);
            }
            else
            {
                SetTextVisibility(false, textForLight);
                SetTextVisibility(false, textForGrass);
            }
            if (transform.position.x > 13f && transform.position.x < 24f)
            {
                SetTextVisibility(true, textForIce);
            }
            else
            {
                SetTextVisibility(false, textForIce);
            }
            if (transform.position.x > 25f && transform.position.x < 46f)
            {
                SetTextVisibility(true, textForLava);
            }
            else
            {
                SetTextVisibility(false, textForLava);
            }
        }
        
        if (transform.position.x > 157f)
        {
            Debug.Log("HandleMovement and HandleResize should be active now");
            HandleMovement();
            HandleResize();
        }
        if (transform.position.x <= 68f)
        {
            float moveX = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveX * speed, rb.velocity.y);

            CheckGrounded();

            if ((Input.GetButtonDown("Jump") && !isJumping))
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                isJumping = true;
            }
        }
        if (transform.position.x <= 157f && transform.position.x >= 68f)
        {
            
            //float moveX = Input.GetAxis("Horizontal");

            // If colliding with the light box and player is on the left side and 'D' is pressed
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

            //rb.velocity = new Vector2(moveX * speed, rb.velocity.y);

            //// Jumping
            //if (Input.GetButtonDown("Jump") && !isJumping)
            //{
            //    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            //    isJumping = true;
            //}

            float moveX = Input.GetAxis("Horizontal");

            rb.velocity = new Vector2(moveX * speed, rb.velocity.y);

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
            RecallActivated = 0;
        }
        if (highlightedObject != null)
        {
            RecallActivated = 1;

        }
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
                    waterObject.GetComponent<Renderer>().material = WaterMaterial2;
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
                waterObject.GetComponent<Renderer>().material = WaterMaterial1;

            }
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

        float distanceToTarget2 = Vector2.Distance(transform.position, targetPosition2);
        if (distanceToTarget2 <= proximityThreshold2)
        {
            instruction2Text.enabled = true; // 当玩家接近目标位置时，显示文本
        }
        else if (instruction2Text.enabled) // 如果玩家远离目标区域，并且文本当前是可见的
        {
            instruction2Text.enabled = false; // 隐藏文本
        }

        // Recall Text
        float distanceToRecallText = Vector2.Distance(transform.position, targetPositionToRecallText);
        if (distanceToRecallText <= 1.0f)
        {
            RecallText.enabled = true;
        }
        if(transform.position.x > 139.0f)
        {
            RecallText.enabled = false;
        }

        // analytics
        if (mask.transform.localScale.x < 2 && mask.transform.localScale.y < 2) // only collect data without vision
        {
            if (transform.position != lastPlayerPosition) // posiiton change
            {
                currentTime = Time.timeSinceLevelLoad;
                if ((currentTime - analyticTime) > 0.1) // data-collection intervals 
                {
                    string levelInf = "1";
                    string stageInf = "1";

                    AnalyticPath analyticPath = new AnalyticPath();
                    analyticPath.tick = currentTime.ToString();
                    AnalyticPosition analyticPosition = new AnalyticPosition();
                    analyticPosition.x = transform.position.x;
                    analyticPosition.y = transform.position.y;
                    analyticPath.position = analyticPosition;

                    string analyticJson = JsonUtility.ToJson(analyticPath);
                    string DBurl = "https://yanjungu-unity-analytics-default-rtdb.firebaseio.com/"
                                + "levels/" + levelInf + "/stages/" + stageInf + "/players/" + playerID + ".json";

                    RestClient.Post(DBurl, analyticJson);
                    analyticTime = currentTime;
                }
                lastPlayerPosition = transform.position;
            }
        }
       
    }

    private void CheckGrounded()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundedRayLength, groundLayer);
        //if (hit.collider != null)
        //{
        //    isJumping = false;
        //}


        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundedRayLength, groundLayer);
        float yPos = transform.position.y;

        //if (hit.collider != null)
        //{
        //    Debug.Log(hit.collider.name);
        //}

        //if (hit.collider != null
        //   || (yPos >= -2.486f && yPos <= -2.48f)
        //   || (yPos >= 2.48f && yPos <= 2.486f)
        //   || hit.collider.CompareTag("IceLake")
        //   || hit.collider.CompareTag("Sand"))
        //{
        //    isJumping = false;
        //}
        //else
        //{
        //    isJumping = true;
        //}

        if (hit.collider != null)
        {
            isJumping = false;
        }
        else if (yPos >= -2.4866f && yPos <= -2.48f)
        {
            isJumping = false;
        }
        else
        {
            isJumping = true;
        }



    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Recall"))
        {
            isJumping = false;
        }

        if (collision.gameObject.CompareTag("IceLake"))
        {
            isJumping = false;
        }

        //if (collision.gameObject.CompareTag("LightBox"))
        //{
        //    isJumping = false;
        //}


        if (collision.gameObject.CompareTag("Sand"))
        {
            isJumping = false;
        }

        //if (collision.gameObject.CompareTag("LightBox"))
        //{
        //    isCollidingWithBox = true;
        //    lightBox = collision.gameObject;

        //    // Check if player is on the left or right side of the box upon collision
        //    isLeftOfBox = transform.position.x < lightBox.transform.position.x;
        //}

        if (collision.gameObject.CompareTag("Water") || collision.gameObject.CompareTag("Lava") || collision.gameObject.CompareTag("Grass"))
        {
            if (CheckPointScript.ischecked == true)
            {
                transform.position = new Vector3(114.68f, -67.48499f);
            }
            else 
            { 
                transform.position = new Vector2(-3.84f, 0f);
            }
        }

        if (collision.gameObject.CompareTag("LeftWall"))
        {
            SoundScreenEffect.Instance.FlashLeft();

        }
        else if (collision.gameObject.CompareTag("RightWall"))
        {
            SoundScreenEffect.Instance.FlashRight();

        }

    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("LightBox"))
    //    {
    //        isCollidingWithBox = false;
    //        lightBox = null;
    //    }
    //}

    private void SetTextVisibility(bool isVisible, GameObject text)
    {
        // If TextForLight is a UI Text object
        if (text.GetComponent<TMPro.TextMeshProUGUI>() != null)
        {
            text.GetComponent<TMPro.TextMeshProUGUI>().enabled = isVisible;
        }

        // If TextForLight has a Renderer (like a SpriteRenderer for 2D games)
        else if (text.GetComponent<Renderer>() != null)
        {
            text.GetComponent<Renderer>().enabled = isVisible;
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
            Debug.Log($"canResize: {canResize}, canGrow: {canGrow}");
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
                    resizeDirection = 1;
                }
                // Grow Downwards
                else if (mousePosition.y < bottomBoundary && canGrowDown)
                {
                    Debug.Log("Growing Downwards");
                    transform.localScale += new Vector3(0, -1, 0);
                    transform.position -= new Vector3(0, 0.5f, 0);
                    hasGrown = true;
                    resizeDirection = 2;
                }
                // Grow Left
                else if (mousePosition.x < leftBoundary && canGrowLeft)
                {
                    Debug.Log("Growing Leftwards");
                    transform.localScale += new Vector3(1, 0, 0);  // 在x轴上增加1
                    transform.position -= new Vector3(0.5f, 0, 0);  // 位置左移0.5
                    hasGrown = true;
                    resizeDirection = 3;
                }
                // Grow Right
                else if (mousePosition.x > rightBoundary && canGrowRight)
                {
                    Debug.Log("Growing Rightwards");
                    transform.localScale += new Vector3(1, 0, 0);
                    transform.position += new Vector3(0.5f, 0, 0);
                    hasGrown = true;
                    resizeDirection = 4;
                }

                // If the player has grown in any direction, reset the ability to grow
                if (hasGrown)
                {
                    canGrow = false;
                    resizeHintText.enabled = false;
                    //Arrow Disappear
                    SetArrowsActive(false);
                    // analytic
                    string levelInf = "1";
                    string stageInf = "2";
                    resizeCount += 1;
                    AnalyticShape analyticShape = new AnalyticShape();
                    analyticShape.resizeCount = resizeCount.ToString();
                    analyticShape.resizeDirection = resizeDirection.ToString();
                    //Arrow Disappear
                    SetArrowsActive(false);

                    string analyticJson = JsonUtility.ToJson(analyticShape);
                    string DBurl = "https://yanjungu-unity-analytics-default-rtdb.firebaseio.com/"
                                + "levels/" + levelInf + "/stages/" + stageInf + "/players/" + playerID + ".json";

                    RestClient.Post(DBurl, analyticJson);
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
            SetArrowsActive(true);
            if (weightHintText != null)
            {
                weightHintText.enabled = false;

            }
            eatenGemCount += 1;

            if (mask != null)
            {

                if (eatenGemCount == 1)
                {
                    mask.transform.localScale = new Vector3(5, 5, 1);
                }
                else if (eatenGemCount == 1)
                {
                    mask.transform.localScale = new Vector3(500, 500, 1);
                }
                else
                {
                    mask.transform.localScale += new Vector3(eatenGemCount + 1, eatenGemCount + 1, 0);
                }

            }
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
                    if (weightHintText != null)
                    {
                        weightHintText.enabled = true;
                    }
                }
            }
        }
    }


    private void SetArrowsActive(bool isActive)
    {
        foreach (GameObject arrow in arrows)
        {
            if (isActive)
           {
               Vector3 direction = arrow.transform.localPosition.normalized;
               arrow.transform.position = transform.position + direction * arrowDistance;
           }
            arrow.SetActive(isActive);
        }
    }

}
