using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTorchController : MonoBehaviour
{
    public Transform player;
    public GameObject enterArrow;
    public GameObject enterMask;
    public GameObject exitArrow;
    public GameObject exitMask;

    public GameObject torch1;
    public GameObject torch2;
    public GameObject torch3;
    public GameObject torch4;
    public GameObject torch5;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.x > 72f && player.position.x < 96f && player.position.y > -31f && player.position.y < -10f)
        {
            torch1.SetActive(true);
            enterArrow.SetActive(true);
            enterMask.SetActive(true);
            torch2.SetActive(true);
            torch3.SetActive(false);
            torch4.SetActive(false);
            torch5.SetActive(false);
            exitArrow.SetActive(false);
            exitMask.SetActive(false);
        }
        else if (player.position.x > 96f && player.position.x < 114f && player.position.y > -31f && player.position.y < -10f)
        {
            torch1.SetActive(false);
            enterArrow.SetActive(false);
            enterMask.SetActive(false);
            torch2.SetActive(true);
            torch3.SetActive(true);
            torch4.SetActive(false);
            torch5.SetActive(false);
            exitArrow.SetActive(false);
            exitMask.SetActive(false);
        }
        else if (player.position.x > 72f && player.position.x < 114f && player.position.y > -42f && player.position.y < -31f)
        {
            torch1.SetActive(false);
            enterArrow.SetActive(false);
            enterMask.SetActive(false);
            torch2.SetActive(false);
            torch3.SetActive(true);
            torch4.SetActive(true);
            torch5.SetActive(false);
            exitArrow.SetActive(false);
            exitMask.SetActive(false);
        }
        else if (player.position.x > 72f && player.position.x < 114f && player.position.y > -53f && player.position.y < -42f)
        {
            torch1.SetActive(false);
            enterArrow.SetActive(false);
            enterMask.SetActive(false);
            torch2.SetActive(false);
            torch3.SetActive(false);
            torch4.SetActive(true);
            torch5.SetActive(true);
            exitArrow.SetActive(true);
            exitMask.SetActive(true);
        }


    }
}
