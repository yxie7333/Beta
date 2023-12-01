using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public GameObject myPlayer;
    public GameObject beamHint;
    public Level1Player playerScript;
    private Vector3 beamOriginalPosition;
    public Vector3 beamNewPosition;
    public float beamSpeed;
    private bool beamActivated = false;
    private bool firstFall = true;
    private float timeInsideBeamArea = 0.0f;

    // Start is called before the first frame update
    private void Awake()
    {
        beamOriginalPosition = transform.position; 
    }

    void Update()
    {
        // 记录玩家解密时间
        if (myPlayer.transform.position.x >= 193.43f && myPlayer.transform.position.x <= 200.53f &&
            myPlayer.transform.position.y >= -78.49f && myPlayer.transform.position.y <= -71.52f)
        {
            timeInsideBeamArea += Time.deltaTime;
        }

        // 如果玩家解密时间超过1min
        if (timeInsideBeamArea >= 3 && myPlayer.transform.position.y >= -78.49f)
        {
            if (myPlayer.transform.position.x >= 193.43f && myPlayer.transform.position.x <= 200.53f &&
    myPlayer.transform.position.y >= -78.49f && myPlayer.transform.position.y <= -71.52f)
            {
                // 显示hint
                beamHint.SetActive(true);
                //Debug.Log("BeamHint Activated!");
            }
        }

        // 当玩家成功解密，隐藏hint
        if (myPlayer.transform.position.y < -78.49f)
        {
            beamHint.SetActive(false);
        }

        if (playerScript.playerFirstCollideBeam == true && beamActivated == false)
        {

            transform.position = Vector3.MoveTowards(transform.position, beamNewPosition,
                            beamSpeed * Time.deltaTime);
            
            //Debug.Log("beam activated");
        }

        if(transform.position == beamNewPosition)
        {
            beamActivated = true;
        }

        if (beamActivated == true)
        {
            if (playerScript.RecallActivated == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, beamOriginalPosition,
                            beamSpeed * Time.deltaTime);
                //Debug.Log("Beam is recalling!");
                //Debug.Log("Beam original position: " + beamOriginalPosition);
            }
            /*
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, beamNewPosition,
                            beamSpeed * Time.deltaTime);
                firstFall = false;
                //Debug.Log("Beam is falling!");
            } 
            */
        }
    }
}