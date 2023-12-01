using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public Level1Player playerScript;
    private Vector3 beamOriginalPosition;
    public Vector3 beamNewPosition;
    public float beamSpeed;
    private bool beamActivated = false;

    // Start is called before the first frame update
    private void Awake()
    {
        beamOriginalPosition = transform.position; 
    }

    void Update()
    {
        if (playerScript.playerFirstCollideBeam == true && beamActivated == false)
        {

            transform.position = Vector3.MoveTowards(transform.position, beamNewPosition,
                            beamSpeed * Time.deltaTime);
            beamActivated = true;
            //Debug.Log("beam activated");
        }

        if (beamActivated == true)
        {
            if (playerScript.RecallActivated == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, beamOriginalPosition,
                            beamSpeed * Time.deltaTime);
                //Debug.Log("Beam is recalling!");
                Debug.Log("Beam original position: " + beamOriginalPosition);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, beamNewPosition,
                            beamSpeed * Time.deltaTime);
                //Debug.Log("Beam is falling!");
            }
        }

    }
}