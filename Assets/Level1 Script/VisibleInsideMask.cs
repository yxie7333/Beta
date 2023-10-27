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
    public Image greenDot;
    public TextMeshProUGUI soundDisplayText;

    // Start is called before the first frame update
    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        soundDisplayText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.x > 71f && player.position.x < 115f && player.position.y > -53f && player.position.y < -22f)
        {
            greenDot.enabled = true;
            if (enterSoundZone == 0)
            {
                tilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

                soundDisplayText.gameObject.SetActive(true);
                StartCoroutine(HideTextAfterSeconds(5f));

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
                greenDot.enabled = false;

            }
            if (enterSoundZone == 1)
            {
                enterSoundZone = 2;
                tilemapRenderer.maskInteraction = SpriteMaskInteraction.None;
                GameObject objToDestroy = GameObject.Find("FakeGem");
                if (objToDestroy != null)
                {
                    Destroy(objToDestroy);
                }
            }
        }
            
    }

    private IEnumerator HideTextAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        soundDisplayText.gameObject.SetActive(false);
    }
}
