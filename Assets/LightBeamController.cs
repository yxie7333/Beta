using UnityEngine;

public class LightBeamController : MonoBehaviour
{
    public Transform primaryLightBeam;  // Assign the primary light beam object here in the inspector.
    public Transform reflectedLightBeam;  // Assign the reflected light beam object here in the inspector.

    public LayerMask mirrorLayer;  // Assign the layer of the mirror object here in the inspector.
    public float maxBeamLength = 5f;  // You can adjust this for how far the light beam should go.

    public LayerMask grassLayer;   // Assign the layer of the grass object here in the inspector.
    public GameObject fireObject;  // Assign the fire prefab or game object here in the inspector.
    private GameObject spawnedFire;

    public void Start()
    {
        reflectedLightBeam.gameObject.SetActive(false);
        fireObject.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Check if primary light beam hits the mirror.
        RaycastHit2D hit = Physics2D.Raycast(primaryLightBeam.position, primaryLightBeam.up, maxBeamLength, mirrorLayer);

        if (hit.collider != null && hit.collider.gameObject.name == "Mirror")
        {
            // Debug.Log("Primary light beam collided with: " + hit.collider.name);
            // Reflect the beam based on the mirror's orientation.
            Vector2 reflectedDirection = Vector2.Reflect(primaryLightBeam.up, hit.normal);
            UpdateReflectedBeam(reflectedLightBeam, hit.point, reflectedDirection);

            // Activate the reflected light (if it was deactivated).
            reflectedLightBeam.transform.position = new Vector3(7.17f, -0.03f, 0);
            reflectedLightBeam.gameObject.SetActive(true);

            RaycastHit2D grassHit = Physics2D.Raycast(reflectedLightBeam.position, reflectedLightBeam.up, maxBeamLength, grassLayer);

            if (grassHit.collider != null)
            {
                // Activate the fire at the grass's position.
                fireObject.SetActive(true);

                // Schedule to deactivate the fire and destroy the grass.
                Invoke("DeactivateFireAndDestroyGrass", 1f);
            }

        }
        else
        {
            // Deactivate the reflected light if it doesn't hit a mirror.
            reflectedLightBeam.gameObject.SetActive(false);
        }
    }


    void UpdateReflectedBeam(Transform lightBeam, Vector2 rayOrigin, Vector2 direction)
    {
        lightBeam.position = rayOrigin;
        lightBeam.up = direction;
    }

    void DeactivateFireAndDestroyGrass()
    {
        fireObject.SetActive(false);
        RaycastHit2D grassHit = Physics2D.Raycast(reflectedLightBeam.position, reflectedLightBeam.up, maxBeamLength, grassLayer);
        if (grassHit.collider != null)
        {
            Destroy(grassHit.collider.gameObject);
        }
    }
}