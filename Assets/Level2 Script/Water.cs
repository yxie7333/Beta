using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Proyecto26;

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

    // Analytic
    private string playerID = System.Guid.NewGuid().ToString();
    private float timeInsideWaterfallArea = 0.0f;
    private bool sendToDB = false; // 判断是否已经将数据发送给DB
    public class WaterAnalyticData
    {
        public string timeInsideArea;
    }

    private void Awake()
    {
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        // 如果瀑布解冻
        if (iceLayer == null)
        {
            // 记录玩家在瀑布解冻后待在谜题区域内的时间
            if (player.transform.position.x >= -166.3f && player.transform.position.x <= -137.35f)
            {
                timeInsideWaterfallArea += Time.deltaTime;
            }

            // 如果玩家解密成功
            if (player.transform.position.x > -136.0f && !sendToDB)
            {
                sendToDB = true;
                WaterAnalyticData waterAnalyticData = new WaterAnalyticData();
                waterAnalyticData.timeInsideArea = timeInsideWaterfallArea.ToString();

                // Convert the data to JSON and POST to Firebase
                string dataJson = JsonUtility.ToJson(waterAnalyticData);
                string DBurl = "https://yanjungu-unity-analytics-default-rtdb.firebaseio.com/" +
                                "levels/" + "2" +
                                "/stages/" + "4" +
                                "" +
                                "/players/" + playerID + ".json";
                RestClient.Post(DBurl, dataJson);
            }

            // 如果玩家激活了回溯
            if (playerScript.RecallActivated == 1)
            {
                if (player.transform.position.x >= position1.x && player.transform.position.x <= position1.x + 1.0f &&
                    player.transform.position.y <= position1.y && player.transform.position.y >= originPosition.y)
                {
                    //rb.gravityScale = 0;
                    //player.transform.position = Vector3.MoveTowards(player.transform.position, position1,
                    //      waterSpeed * Time.deltaTime);
                    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                    //Debug.Log("Move to Position1!");
                }
                if (player.transform.position.x >= position1.x && player.transform.position.x <= position2.x &&
                    player.transform.position.y >= position2.y - 2.0f && player.transform.position.y <= position2.y - 1.0f)
                {
                    //player.GetComponent<Rigidbody2D>().gravityScale = 1;
                    player.transform.position = Vector3.MoveTowards(player.transform.position, position2,
                            waterSpeed * Time.deltaTime);
                    //Debug.Log("Move to Position2!");
                }
            }

            // 如果玩家未激活回溯
            else
            {
                if (player.transform.position.x >= position1.x && player.transform.position.x <= position2.x &&
                    player.transform.position.y >= position2.y - 2.0f && player.transform.position.y <= position2.y - 1.0f)
                {
                    //player.GetComponent<Rigidbody2D>().gravityScale = 1;
                    player.transform.position = Vector3.MoveTowards(player.transform.position, position1,
                            waterSpeed * Time.deltaTime);
                    //Debug.Log("Move to Position1!");
                }
            }
        }
    }
}