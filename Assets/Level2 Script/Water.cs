using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Water : MonoBehaviour
{
    public float waterSpeed = 3.0f;
    GameObject player;
    public Vector3 position1;
    public Vector3 position2;
    public Player playerScript;
    public Vector3 originPosition;
    private Rigidbody2D rb;
    public float jumpForce;
    public GameObject iceLayer;

    private void Awake()
    {
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (iceLayer == null)
        {
            if (playerScript.RecallActivated == 1)
            {
                if (player.transform.position.x >= position1.x && player.transform.position.x <= position1.x + 1.0f &&
                    player.transform.position.y <= position1.y && player.transform.position.y >= originPosition.y)
                {
                    //rb.gravityScale = 0;
                    //player.transform.position = Vector3.MoveTowards(player.transform.position, position1,
                    //      waterSpeed * Time.deltaTime);
                    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                    Debug.Log("Move to Position1!");

                }
                if (player.transform.position.x >= position1.x && player.transform.position.x <= position2.x &&
                    player.transform.position.y >= position2.y - 2.0f && player.transform.position.y <= position2.y - 1.0f)
                {
                    //player.GetComponent<Rigidbody2D>().gravityScale = 1;
                    player.transform.position = Vector3.MoveTowards(player.transform.position, position2,
                            waterSpeed * Time.deltaTime);
                    Debug.Log("Move to Position2!");
                }
            }
            else
            {
                if (player.transform.position.x >= position1.x && player.transform.position.x <= position2.x &&
                    player.transform.position.y >= position2.y - 2.0f && player.transform.position.y <= position2.y - 1.0f)
                {
                    //player.GetComponent<Rigidbody2D>().gravityScale = 1;
                    player.transform.position = Vector3.MoveTowards(player.transform.position, position1,
                            waterSpeed * Time.deltaTime);
                    Debug.Log("Move to Position1!");
                }
            }
        }
    }
}

