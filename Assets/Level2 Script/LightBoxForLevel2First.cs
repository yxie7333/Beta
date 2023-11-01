using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBoxForLevel2First : MonoBehaviour
{
    public float minX = -181f;
    public float maxX = -175f;
    private void Start()
    {
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
        transform.position = currentPosition;

    }

}
