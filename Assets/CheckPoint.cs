using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool ischecked = false;
    private SpriteRenderer spriteRenderer;
    public Color savedColor = Color.green;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ischecked = true;
            spriteRenderer.color = savedColor;
            Debug.Log("Change Color to Green!");
        }
    }
}
