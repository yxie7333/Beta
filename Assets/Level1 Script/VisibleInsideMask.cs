using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class VisibleInsideMask : MonoBehaviour
{
    public Transform player;
    private TilemapRenderer tilemapRenderer;

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
            tilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        else
        {
            tilemapRenderer.maskInteraction = SpriteMaskInteraction.None;
        }
    }
}
