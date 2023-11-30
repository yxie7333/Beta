using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControllerLevel1Second : MonoBehaviour
{

    public Collider2D mirrorCollider; // Assign in the inspector
    public GameObject light;          // Assign first fire object
    public GameObject hiddenLight;     // Assign the hidden fire object
    public GameObject hiddenPath;
    public GameObject lightbox;

    public void Start()
    {
        hiddenLight.gameObject.SetActive(false);
    }

    private void Update()
    {
        CheckFireMirrorCollision(light);

        if (lightbox.transform.position.x >= 143.5f)
        {
            hiddenLight.gameObject.SetActive(false);
        }
    }

    private void CheckFireMirrorCollision(GameObject light)
    {
        if (light != null && mirrorCollider.bounds.Intersects(light.GetComponent<Collider2D>().bounds))
        {
            hiddenLight.gameObject.SetActive(true);
            DestroyHiddenPath();
        }
    }

    private void DestroyHiddenPath()
    {
        if (hiddenPath != null)
        {
            Destroy(hiddenPath, 0.5f);
            // Debug.Log("Grass object destroyed!");
        }
    }

}
