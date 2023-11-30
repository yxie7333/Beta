using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaForLevel1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object colliding is water
        if (collision.gameObject.tag == "Water")
        {
            // Destroy or disable the water object
            Destroy(collision.gameObject);
            Destroy(gameObject);

            // Additional effects like steam or sizzling sounds can be played here
        }
    }
}
