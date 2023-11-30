using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LightControllerLevel1First : MonoBehaviour
{
    public Collider2D mirrorCollider; // Assign in the inspector
    public GameObject light;          // Assign first fire object
    public GameObject hiddenLight1;     // Assign the hidden fire object
    public GameObject hiddenLight2;
    public GameObject ice;
    public GameObject water;
    public GameObject lightbox;


    public void Start()
    {
        hiddenLight1.gameObject.SetActive(false);
        hiddenLight2.gameObject.SetActive(false);
        water.SetActive(false);

    }

    private void Update()
    {
        CheckFireMirrorCollision(light);

        if (lightbox.transform.position.x >= 106.5f)
        {
            hiddenLight1.gameObject.SetActive(false);
            hiddenLight2.gameObject.SetActive(false);
        }
    }

    private void CheckFireMirrorCollision(GameObject light)
    {
        if (light != null && mirrorCollider.bounds.Intersects(light.GetComponent<Collider2D>().bounds))
        {
            hiddenLight1.gameObject.SetActive(true);
            hiddenLight2.gameObject.SetActive(true);
            DestroyIce();
        }
    }

    private void DestroyIce()
    {
        if (ice != null)
        {
            Destroy(ice, 0.5f);
            // Debug.Log("Grass object destroyed!");
            Invoke(nameof(ActivateWater), 0.5f);
        }
    }

    private void ActivateWater()
    {
        water.SetActive(true);

        // Get the Rigidbody2D component of the water object
        Rigidbody2D waterRigidbody = water.GetComponent<Rigidbody2D>();

        // Check if the Rigidbody2D component exists
        if (waterRigidbody != null)
        {
            // Change the Rigidbody type to Dynamic
            waterRigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            Debug.LogError("Water object does not have a Rigidbody2D component.");
        }

    }

}
