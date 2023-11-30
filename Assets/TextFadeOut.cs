using UnityEngine;
using System.Collections;
using TMPro; // Add this to access TextMeshPro types

public class TextFadeOut : MonoBehaviour
{
    public CanvasGroup uiGroup; // Assign this in the inspector
    public float delayBeforeFade = 8f; // Time in seconds before the fade starts
    public float fadeDuration = 2f; // Duration of the fade

    void Start()
    {
        if (uiGroup != null)
        {
            StartCoroutine(FadeOut(delayBeforeFade, fadeDuration));
        }
    }

    private IEnumerator FadeOut(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        float startAlpha = uiGroup.alpha;
        float endAlpha = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            uiGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }

        uiGroup.alpha = endAlpha; // Ensure the UI is completely invisible
        uiGroup.interactable = false; // Optionally, disable interaction
        uiGroup.blocksRaycasts = false; // Optionally, prevent the UI from blocking raycasts
    }
}

