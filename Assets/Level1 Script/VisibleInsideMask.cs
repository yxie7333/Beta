using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;




public class VisibleInsideMask : MonoBehaviour
{
    public Transform player;
    private TilemapRenderer tilemapRenderer;
    private int enterSoundZone = 0;
    // Start is called before the first frame update
    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.x > 71f && player.position.x < 115f && player.position.y > -53f && player.position.y < -22f)
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
            if(enterSoundZone == 0)
            {

            }
            if (enterSoundZone == 1)
            {
                enterSoundZone = 2;
                tilemapRenderer.maskInteraction = SpriteMaskInteraction.None;
            }
        }
            
    }
}
