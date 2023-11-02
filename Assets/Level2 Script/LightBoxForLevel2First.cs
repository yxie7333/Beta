using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Proyecto26;
public class LightBoxForLevel2First : MonoBehaviour
{
    public float minX = -181f;
    public float maxX = -175f;

    private GameObject player;
    private Rigidbody2D rb;
    private Rigidbody2D playerRb;
    private bool isCollidingWithPlayer = false;

    private float collisionStartTime = 0f;
    private float totalCollisionDuration = 0f;

    private string playerID = System.Guid.NewGuid().ToString();

    [System.Serializable]
    public class CollisionData
    {
        public string collisionDuration;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        playerRb = player.GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == player)
        {
            isCollidingWithPlayer = true;

            collisionStartTime = Time.time;

        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject == player)
        {
            isCollidingWithPlayer = false;
            rb.velocity = new Vector2(0, rb.velocity.y);

            totalCollisionDuration += Time.time - collisionStartTime;

            // Debug.Log("Total collision duration: " + totalCollisionDuration + " seconds");
            CollisionData collisionData = new CollisionData();
            collisionData.collisionDuration = totalCollisionDuration.ToString();

            // Convert the data to JSON and POST to Firebase
            string dataJson = JsonUtility.ToJson(collisionData);
            string DBurl = "https://yanjungu-unity-analytics-default-rtdb.firebaseio.com/" +
                           "levels/" + "2" +
                           "/stages/" + "1" +
                           "/players/" + playerID + ".json";
            RestClient.Post(DBurl, dataJson);
        }
    }
}
