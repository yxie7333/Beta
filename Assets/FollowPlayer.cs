using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FollowPlayer : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;

    // Vsion effect: camera change
    public int levelNum;
    private Camera cam;

    public Image enterArrow;
    public Image exitArrow;

    private int enterSoundZone = 0;
    public TextMeshProUGUI soundDisplayText;



    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam != null && cam.orthographic)
        {
            if(levelNum == 0)
            {
                cam.orthographicSize = 8;
            }
            else if(levelNum == 1)
            {
                cam.orthographicSize = 5;
            }
            else if (levelNum == 2)
            {
                cam.orthographicSize = 12;
            }
        }
        if (enterArrow != null)
        {
            enterArrow.enabled = false;
        }
        if (exitArrow != null)
        {
            exitArrow.enabled = false;
        }
        soundDisplayText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (levelNum == 0)
        {
            if (player.position.x > 71f && player.position.x < 115f && player.position.y > -53f && player.position.y < -22f)
            {
                transform.position = new Vector3(93, -37, offset.z);
                if (cam != null && cam.orthographic)
                {
                    cam.orthographicSize = 20;
                }
                if (enterArrow != null)
                {
                    enterArrow.enabled = true;
                }
                if (exitArrow != null)
                {
                    exitArrow.enabled = true;
                }

                if (enterSoundZone == 0)
                {
                    soundDisplayText.gameObject.SetActive(true);
                    StartCoroutine(HideTextAfterSeconds(5f));
                    enterSoundZone = 1;
                }
            }
            else
            {
                transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
                if (cam != null && cam.orthographic)
                {
                    cam.orthographicSize = 8;
                }
                if (enterArrow != null)
                {
                    enterArrow.enabled = false;
                }
                if (exitArrow != null)
                {
                    exitArrow.enabled = false;
                }
            }

        }
        else if (levelNum == 1)
        {
            if (player.position.x > 137f && player.position.x < 180f && player.position.y > -52f && player.position.y < -17f)
            {
                transform.position = new Vector3(160, -37, offset.z);
                if (cam != null && cam.orthographic)
                {
                    cam.orthographicSize = 17;
                }
                if (enterSoundZone == 0)
                {
                    soundDisplayText.gameObject.SetActive(true);
                    StartCoroutine(HideTextAfterSeconds(5f));
                    enterSoundZone = 1;
                }
            }
            else
            {
                transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
                if (cam != null && cam.orthographic)
                {
                    cam.orthographicSize = 5;
                }
            }
        }
        else if (levelNum == 2)
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
        }
        else
        {
            ;
        }

    }

    private IEnumerator HideTextAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        soundDisplayText.gameObject.SetActive(false);
    }
}
