using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Proyecto26;

public class Boat : MonoBehaviour
{
    // 引用PlayerRecall脚本
    public CombinedPlayer1 playerScript;

    public Vector3 targetPosition;
    Vector3 originPosition;
    public int mySpeed;
    public GameObject myPlayer;
    int onBoat = 0;

    // Analytic
    private string playerID = System.Guid.NewGuid().ToString();
    private int boatComeCount = 0;
    private int boatRecallCount = 0;
    private int playerComeCount = 0;
    private int playerRecallCount = 0;
    private bool sendToDB = false; // 判断是否已经将数据发送给DB

    [System.Serializable]
    public class BoatAnalyticData
    {
        public string boatComeNoPlayer;
        public string boatRecallNoPlayer;
        public string boatComeWithPlayer;
        public string boatRecallWithPlayer;
    }

    private void Awake()
    {
        myPlayer = GameObject.Find("Player");
        originPosition = new Vector3(transform.position.x, transform.position.y,
            transform.position.z);
    }

    private void Update()
    {
        if (playerScript.RecallActivated == 0)
        {
            if (Vector3.Distance(transform.position, myPlayer.transform.position) <= 15f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition,
                    mySpeed * Time.deltaTime);
                boatComeCount += 1;
                if (onBoat == 1)
                {
                    myPlayer.transform.position = Vector3.MoveTowards(myPlayer.transform.position, targetPosition,
                        mySpeed * Time.deltaTime);
                    playerComeCount += 1;
                }
                //Debug.Log("Boat is coming!");
            }
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, originPosition,
                    mySpeed * Time.deltaTime);
            boatRecallCount += 1;
            if (onBoat == 1)
            {
                myPlayer.transform.position = Vector3.MoveTowards(myPlayer.transform.position, originPosition,
                    mySpeed * Time.deltaTime);
                playerRecallCount += 1;
            }
            //Debug.Log("RecallActivated = 1");
        }

        // 当player通过这关时，发送收集到的数据
        if (myPlayer.transform.position.x >= 140.0f && !sendToDB)
        {
            sendToDB = true;
            BoatAnalyticData boatAnalyticData = new BoatAnalyticData();

            boatAnalyticData.boatComeWithPlayer = playerComeCount.ToString();
            boatAnalyticData.boatRecallWithPlayer = playerRecallCount.ToString();
            boatAnalyticData.boatComeNoPlayer = (boatComeCount - playerComeCount).ToString();
            boatAnalyticData.boatRecallNoPlayer = (boatRecallCount - playerRecallCount).ToString();

            // Convert the data to JSON and POST to Firebase
            string dataJson = JsonUtility.ToJson(boatAnalyticData);
            string DBurl = "https://yanjungu-unity-analytics-default-rtdb.firebaseio.com/" +
                           "levels/" + "0" +
                           "/stages/" + "3" +
                           "/players/" + playerID + ".json";
            RestClient.Post(DBurl, dataJson);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onBoat = 1;
            //Debug.Log("Player is on the boat!");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onBoat = 0;
            //Debug.Log("Player is not on the boat!");
        }
    }

    public void Interact()
    {
        //Debug.Log("Interacting");
    }
}