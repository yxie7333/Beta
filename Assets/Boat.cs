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
    GameObject myPlayer;

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

            if (Vector3.Distance(transform.position, myPlayer.transform.position) <= 20f)
            {

                transform.position = Vector3.MoveTowards(transform.position, targetPosition,
                    mySpeed * Time.deltaTime);
                Debug.Log("Boat is coming!");
            }
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, originPosition,
                    mySpeed * Time.deltaTime);
            Debug.Log("RecallActivated = 1");
        }
    }

    public void Interact()
    {
        Debug.Log("Interacting");
    }
}
