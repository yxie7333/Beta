using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControllerForLevel2Second : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform primaryLightBeam;
    public Transform reflectedLightBeam;

    public LayerMask mirrorLayer;
    public float maxBeamLength = 100f;
    public GameObject water1;
    public GameObject water2;
    public GameObject iceCube;

    void Start()
    {
        reflectedLightBeam.gameObject.SetActive(false);
        iceCube.gameObject.SetActive(false);
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(primaryLightBeam.position, primaryLightBeam.up, maxBeamLength, mirrorLayer);


        if (hit.collider != null && hit.collider.gameObject.name == "SecondMirror")
        {
            reflectedLightBeam.gameObject.SetActive(true);
            Destroy(water1);
            Destroy(water2);
            if (iceCube != null)
            {
                iceCube.SetActive(true);
            }
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
