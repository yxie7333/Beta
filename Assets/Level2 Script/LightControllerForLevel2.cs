using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControllerForLevel2 : MonoBehaviour
{
    public Transform primaryLightBeam;
    public Transform reflectedLightBeam;

    public LayerMask mirrorLayer;
    //public LayerMask iceLayer;  // Add a LayerMask for the water objects.
    public float maxBeamLength = 5f;
    public GameObject ice;
    //public GameObject iceCube;

    void Start()
    {
        reflectedLightBeam.gameObject.SetActive(false);
        //iceCube.gameObject.SetActive(false);
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(primaryLightBeam.position, primaryLightBeam.up, maxBeamLength, mirrorLayer);

        if (hit.collider != null && hit.collider.gameObject.name == "Mirror")
        {
            if (hit.collider != null)
            {
            }
            //Vector2 reflectedDirection = Vector2.Reflect(primaryLightBeam.up, hit.normal);
            //UpdateReflectedBeam(reflectedLightBeam, hit.point, reflectedDirection);

            //reflectedLightBeam.transform.position = new Vector3(21.97f, -0.11f, 0);
            reflectedLightBeam.gameObject.SetActive(true);
            //Destroy(water);
            //iceCube.SetActive(true);
            Destroy(ice);


        }
        else
        {
            reflectedLightBeam.gameObject.SetActive(false);
        }
    }

    void UpdateReflectedBeam(Transform lightBeam, Vector2 rayOrigin, Vector2 direction)
    {
        lightBeam.position = rayOrigin;
        lightBeam.up = direction;
    }
}