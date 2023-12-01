using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class Level1_VisibleInsideMask: MonoBehaviour
{
    public Transform player;
    private TilemapRenderer tilemapRenderer;
    private int enterSoundZone = 0;

    public GameObject torch1;
    public GameObject torch2;
    public GameObject torch3;



    // Start is called before the first frame update
    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // handle ground
        if (player.position.x > 137f && player.position.x < 181f && player.position.y > -53f && player.position.y < -19f)
        {
            if (enterSoundZone == 0)
            {
                tilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                enterSoundZone = 1;
            }
            else
            {
                ;
            }

        }
        else
        {
            if (enterSoundZone == 0)
            {

            }
            if (enterSoundZone == 1)
            {
                enterSoundZone = 2;
                tilemapRenderer.maskInteraction = SpriteMaskInteraction.None;
            }
        }


        // handle torch
        if (player.position.x > 137f && player.position.x < 181f && player.position.y > -27f && player.position.y < -19f)
        {
            torch1.SetActive(true);
            torch2.SetActive(true);
            torch3.SetActive(false);

        }
        else if (player.position.x > 137f && player.position.x < 169f && player.position.y > -35f && player.position.y < -27f)
        {
            torch1.SetActive(true);
            torch2.SetActive(true);
            torch3.SetActive(false);

        }
        else if (player.position.x > 169f && player.position.x < 181f && player.position.y > -35f && player.position.y < -27f)
        {
            torch1.SetActive(false);
            torch2.SetActive(true);
            torch3.SetActive(true);

        }
        else if (player.position.x > 137f && player.position.x < 181f && player.position.y > -53f && player.position.y < -35f)
        {
            torch1.SetActive(false);
            torch2.SetActive(true);
            torch3.SetActive(true);
        }

    }
}
