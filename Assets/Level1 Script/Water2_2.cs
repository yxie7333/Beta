using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Proyecto26;

public class Water2_2 : MonoBehaviour
{
    public GameObject player;
    public Vector3 position;
    public Level1Player playerScript;
    private Rigidbody2D rb;
    public float jumpForce;
    private bool isCollide = false;

    private void Awake()
    {
        //player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        // 如果玩家激活了回溯
        if (playerScript.RecallActivated == 1)
        {
            //Debug.Log("Recall activated");
            if (isCollide == true && player.transform.position.y <= position.y)
            {
                //rb.gravityScale = 0;
                //player.transform.position = Vector3.MoveTowards(player.transform.position, position1,
                //      waterSpeed * Time.deltaTime);
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                //Debug.Log("AddForce activated");
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isCollide = true;
        }
        else
        {
            isCollide = false;
        }
        //Debug.Log("isCollide = " + isCollide);
    }
}