using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundScreenEffect : MonoBehaviour
{
    public static SoundScreenEffect Instance; // 单例模式
    public Image leftFlash;
    public Image rightFlash;
    public Image greenDot;

    public Transform player;
    public Transform gem1;
    public Transform gem2;
    public Transform gem3;

    public RectTransform canvasRect;
    public Camera mainCamera;

    public float flashDuration = 0.6f;

    private SoundScreenEffect soundScreenEffect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (gem1 != null)
        {
            Vector2 intersection = FindIntersection(player.position, gem1.position);

            //Convert the screen point to a local point within the canvas RectTransform
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, intersection, null, out localPoint))
            {
                greenDot.rectTransform.anchoredPosition = localPoint;
            }
        }  
        else if (gem2 != null)
        {
            Vector2 intersection = FindIntersection(player.position, gem2.position);

            //Convert the screen point to a local point within the canvas RectTransform
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, intersection, null, out localPoint))
            {
                greenDot.rectTransform.anchoredPosition = localPoint;
            }
        }  
        else if(gem3 != null)
        {
            Vector2 intersection = FindIntersection(player.position, gem3.position);

            //Convert the screen point to a local point within the canvas RectTransform
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, intersection, null, out localPoint))
            {
                greenDot.rectTransform.anchoredPosition = localPoint;
            }
        }
        else
        {
            greenDot.enabled = false;
        }
    }


    public void FlashLeft()
    {
        StartCoroutine(Flash(leftFlash));
    }

    public void FlashRight()
    {
        StartCoroutine(Flash(rightFlash));
    }

    private IEnumerator Flash(Image img)
    {
        Color originalColor = img.color;
        Color flashColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f); // Alpha设为1
        img.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        flashColor.a = 0f; // Alpha设为0
        img.color = flashColor;
    }

        Vector2 FindIntersection(Vector3 playerPos, Vector3 lightboxPos)
        {
            // Convert to screen coordinates
            Vector2 playerScreenPos = mainCamera.WorldToScreenPoint(playerPos);
            Vector2 lightboxScreenPos = mainCamera.WorldToScreenPoint(lightboxPos);

            // Check if both points are on the screen
            if (IsPointOnScreen(playerScreenPos) && IsPointOnScreen(lightboxScreenPos))
            {
                if (lightboxScreenPos.x > playerScreenPos.x)
                    return new Vector2(Screen.width, lightboxScreenPos.y);
                else
                    return new Vector2(0, lightboxScreenPos.y);
            }

            // Calculate the intersection of the line segment with screen bounds
            float screenLeft = 0;
            float screenRight = Screen.width;
            float screenBottom = 0;
            float screenTop = Screen.height;

            if (LineSegmentsIntersection(playerScreenPos, lightboxScreenPos, new Vector2(screenLeft, screenBottom), new Vector2(screenLeft, screenTop), out Vector2 intersectionLeft))
                return intersectionLeft;
            if (LineSegmentsIntersection(playerScreenPos, lightboxScreenPos, new Vector2(screenRight, screenBottom), new Vector2(screenRight, screenTop), out Vector2 intersectionRight))
                return intersectionRight;
            if (LineSegmentsIntersection(playerScreenPos, lightboxScreenPos, new Vector2(screenLeft, screenBottom), new Vector2(screenRight, screenBottom), out Vector2 intersectionBottom))
                return intersectionBottom;
            if (LineSegmentsIntersection(playerScreenPos, lightboxScreenPos, new Vector2(screenLeft, screenTop), new Vector2(screenRight, screenTop), out Vector2 intersectionTop))
                return intersectionTop;

            return Vector2.zero;
        }

        bool IsPointOnScreen(Vector2 point)
        {
            return point.x >= 0 && point.x <= Screen.width && point.y >= 0 && point.y <= Screen.height;
        }

        bool LineSegmentsIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            float det = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);
            if (det == 0) return false;

            float lambda = ((p4.y - p3.y) * (p4.x - p1.x) + (p3.x - p4.x) * (p4.y - p1.y)) / det;
            float gamma = ((p1.y - p2.y) * (p4.x - p1.x) + (p2.x - p1.x) * (p4.y - p1.y)) / det;

            if (0 <= lambda && lambda <= 1 && 0 <= gamma && gamma <= 1)
            {
                intersection = new Vector2(p1.x + lambda * (p2.x - p1.x), p1.y + lambda * (p2.y - p1.y));
                return true;
            }

            return false;
        }

}