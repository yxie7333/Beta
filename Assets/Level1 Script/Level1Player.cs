﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Proyecto26;
using static Level1Player;

public class Level1Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private bool isJumping = true;
    private Rigidbody2D rb;

    //private bool isCollidingWithBox = false;
    //private GameObject lightBox;
    //private bool isLeftOfBox = false;

    //Resize
    private SpriteRenderer sr;
    private bool canResize = false;
    private bool canGrowUp = true;
    private bool canGrowDown = false;
    private bool canGrowLeft = true;
    private bool canGrowRight = true;
    private bool canGrow = false; // Start with player not being able to grow
    private float playerMass = 1f;
    
    //Movement
    private bool canMove = true; 


    //public Text resizeHintText;


    // sound
    public int eatenGemCount = 0;
    public SpriteMask playerMask;
    public SpriteMask gem1Mask;
    public SpriteMask gem2Mask;
    public SpriteMask gem3Mask;


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
    private class AnalyticPath
    {
        public string tick;
        public AnalyticPosition position;
    }

    [System.Serializable]
    private class AnalyticPosition
    {
        public float x;
        public float y;
    }
    // metric 2
    private int resizeDirection = 0;
    private int resizeCount = 0;


    [System.Serializable]
    private class AnalyticShape
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
        //resizeHintText.enabled = false; // 初始时设置提示为不可见
        rb.mass = playerMass;

        // sound
        eatenGemCount = 0;

        // Analytics
        lastPlayerPosition = transform.position;

        //TriangleController
        // triangleController = GetComponent<TriangleController>();
        SetArrowsActive(false); // Ensure arrows are inactive at the start

    }

    private void Update()
    {      
        HandleMovement();
        HandleResize();

        // analytics
        if (transform.position.x > 137f && transform.position.x < 180f && transform.position.y > -52f && transform.position.y < -17f) // only collect data without vision
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LeftWall"))
        {
            SoundScreenEffect.Instance.FlashLeft();

        }
        else if (collision.gameObject.CompareTag("RightWall"))
        {
            SoundScreenEffect.Instance.FlashRight();

        }
        if (collision.gameObject.CompareTag("ground"))
        {
            isJumping = false;
        }

    }

    private void HandleMovement()
    {
        if (canMove)
        {
            float xDir = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(xDir * speed, rb.velocity.y);

            // handle jumping
            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                if (playerMass == 1)
                {
                    jumpForce = 7f * playerMass;
                }
                else
                {
                    jumpForce = 7f * playerMass * 1.25f;
                }
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                isJumping = true;
            }
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
                    canMove = true;

                    //Increase quality
                    playerMass *= 2;  // 玩家的质量翻倍
                    rb.mass = playerMass;
                    Debug.Log("Gem Quality: " + rb.mass);

                    //Arrow Disappear
                    SetArrowsActive(false);

                    //analysis
                    string levelInf = "1";
                    string stageInf = "2";
                    resizeCount += 1;
                    AnalyticShape analyticShape = new AnalyticShape();
                    analyticShape.resizeCount = resizeCount.ToString();
                    analyticShape.resizeDirection = resizeDirection.ToString();

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
            Debug.Log("Gem Quality: " + rb.mass);
            canResize = true;
            canGrow = true;
            canMove = false;
            SetArrowsActive(true);

            eatenGemCount += 1;

            if (playerMask != null)
            {

                if (eatenGemCount == 1)
                {
                    playerMask.transform.localScale = new Vector3(15, 15, 1);
                    if (gem1Mask != null)
                    {
                        gem1Mask.transform.localScale = new Vector3(0, 0, 1);
                    }
                    if (gem2Mask != null)
                    {
                        gem2Mask.transform.localScale = new Vector3(2, 2, 1);
                    }
                    if (gem3Mask != null)
                    {
                        gem3Mask.transform.localScale = new Vector3(0, 0, 1);
                    }
                }
                else if (eatenGemCount == 2)
                {
                    playerMask.transform.localScale = new Vector3(20, 20, 1);
                    if (gem1Mask != null)
                    {
                        gem1Mask.transform.localScale = new Vector3(0, 0, 1);
                    }
                    if (gem2Mask != null)
                    {
                        gem2Mask.transform.localScale = new Vector3(0, 0, 1);
                    }
                    if (gem3Mask != null)
                    {
                        gem3Mask.transform.localScale = new Vector3(2, 2, 1);
                    }
                }
                else if (eatenGemCount == 3)
                {
                    playerMask.transform.localScale = new Vector3(500, 500, 1);
                    if (gem1Mask != null)
                    {
                        gem1Mask.transform.localScale = new Vector3(0, 0, 1);
                    }
                    if (gem2Mask != null)
                    {
                        gem2Mask.transform.localScale = new Vector3(0, 0, 1);
                    }
                    if (gem3Mask != null)
                    {
                        gem3Mask.transform.localScale = new Vector3(0, 0, 1);
                    }
                }
                else
                {
                    ;
                }

            }

            // playerMass *= 2;  // 玩家的质量翻倍
            // rb.mass = playerMass;

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

    private void SetArrowsActive(bool isActive)
    {
        // 玩家的当前y位置代表地板的高度
        float groundLevel = transform.position.y;

        foreach (GameObject arrow in arrows)
        {
            if (isActive)
            {
                Vector3 direction = arrow.transform.localPosition.normalized;
                Vector3 newPosition = transform.position; // Start with the current player position

                // 计算箭头的新位置
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) // 如果箭头主要指向水平方向
                {
                    newPosition += new Vector3(direction.x * (transform.localScale.x / 2 + 0.5f), 0f, 0f);
                }
                else // 如果箭头主要指向垂直方向
                {
                    newPosition += new Vector3(0f, direction.y * (transform.localScale.y / 2 + 0.5f), 0f);

                    // 确保箭头的y值不小于地板的高度（玩家的y值）
                    newPosition.y = Mathf.Max(newPosition.y, groundLevel);
                }

                arrow.transform.position = newPosition; // 应用新位置
            }
            arrow.SetActive(isActive);
        }
    }


    // private void SetArrowsActive(bool isActive)
    // {
    //     foreach (GameObject arrow in arrows)
    //     {
    //         //if (isActive)
    //         //{
    //         //    Vector3 direction = arrow.transform.localPosition.normalized;
    //         //    arrow.transform.position = transform.position + direction * arrowDistance;
    //         //}
    //         arrow.SetActive(isActive);
    //     }
    // }

}