using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    // ÒýÓÃPlayerRecall½Å±¾
    public CombinedPlayer1 playerScript;

    public Vector3 targetPosition;
    Vector3 originPosition;
    public int mySpeed;
    public GameObject myPlayer;
    int onBoat = 0;

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
                if (onBoat == 1)
                {
                    myPlayer.transform.position = Vector3.MoveTowards(myPlayer.transform.position, targetPosition,
                        mySpeed * Time.deltaTime);
                }
                //Debug.Log("Boat is coming!");
            }
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, originPosition,
                    mySpeed * Time.deltaTime);
            if (onBoat == 1)
            {
                myPlayer.transform.position = Vector3.MoveTowards(myPlayer.transform.position, originPosition,
                    mySpeed * Time.deltaTime);

            }
            //Debug.Log("RecallActivated = 1");
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