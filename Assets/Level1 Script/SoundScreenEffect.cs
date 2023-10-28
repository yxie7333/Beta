using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


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
    public TextMeshProUGUI leftWallText;
    public TextMeshProUGUI rightWallText;


    public float flashDuration = 0.6f;

    private SoundScreenEffect soundScreenEffect;
    private float greenDotWidth;
    private float greenDotHeight;

    private Vector3 lastPlayerPosition;

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

    private void Start()
    {
        if (leftWallText != null && rightWallText != null)
        {
            leftWallText.gameObject.SetActive(false);
            rightWallText.gameObject.SetActive(false);
        }

        lastPlayerPosition = player.transform.position;
    }

    private void Update()
    {
        greenDotWidth = greenDot.rectTransform.rect.width;
        greenDotHeight = greenDot.rectTransform.rect.height;

        if (gem1 == null && gem2 == null && gem3 == null)
        {
            greenDot.enabled = false;
        }

        if (gem1 != null)
        {
            Vector2 intersection = FindIntersection(player.position, gem1.position);
            Vector3 directionToGem = gem1.position - player.position;
            RotateArrowTowards(directionToGem, greenDot.rectTransform);

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
            Vector3 directionToGem = gem2.position - player.position;
            RotateArrowTowards(directionToGem, greenDot.rectTransform);

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
            Vector3 directionToGem = gem3.position - player.position;
            RotateArrowTowards(directionToGem, greenDot.rectTransform);

            //Convert the screen point to a local point within the canvas RectTransform
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, intersection, null, out localPoint))
            {
                greenDot.rectTransform.anchoredPosition = localPoint;
            }
        }

        if (player.transform.position != lastPlayerPosition)
        {
            Color originalColor = greenDot.color;
            Color flashColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
            greenDot.color = flashColor;

            lastPlayerPosition = player.transform.position;
        } else
        {
            Color originalColor = greenDot.color;
            Color flashColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            greenDot.color = flashColor;
        }

    }


    public void FlashLeft()
    {
        if (leftWallText != null && rightWallText != null)
        {
            StartCoroutine(FlashAndText(leftFlash, leftWallText));
        } else
        {
            Debug.Log("111");
            StartCoroutine(Flash(leftFlash));
        }
    }

    public void FlashRight()
    {
        if (leftWallText != null && rightWallText != null)
        {
            StartCoroutine(FlashAndText(rightFlash, rightWallText));
        }
        else
        {
            StartCoroutine(Flash(rightFlash));
        }
    }

    private IEnumerator Flash(Image img)
    {
        Color originalColor = img.color;
        Color flashColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        img.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        flashColor.a = 0f; // Alpha设为0
        img.color = flashColor;
    }

    private IEnumerator FlashAndText(Image img, TextMeshProUGUI txt)
    {
        Color originalColor = img.color;
        Color flashColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        img.color = flashColor;
        txt.gameObject.SetActive(true);


        yield return new WaitForSeconds(flashDuration);

        flashColor.a = 0f; // Alpha设为0
        img.color = flashColor;

        yield return new WaitForSeconds(3-flashDuration);
        txt.gameObject.SetActive(false);

    }

    private void RotateArrowTowards(Vector3 direction, RectTransform arrowTransform)
    {
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        arrowTransform.eulerAngles = new Vector3(0, 0, angle);
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
                {
                    return new Vector2(Screen.width - greenDotWidth / 2, lightboxScreenPos.y); // 向左偏移
                }
                else
                {
                    return new Vector2(greenDotWidth / 2, lightboxScreenPos.y); // 向右偏移
                }
            }

            // Calculate the intersection of the line segment with screen bounds
            float screenLeft = 0;
            float screenRight = Screen.width;
            float screenBottom = 0;
            float screenTop = Screen.height;

            if (LineSegmentsIntersection(playerScreenPos, lightboxScreenPos, new Vector2(screenLeft, screenBottom), new Vector2(screenLeft, screenTop), out Vector2 intersectionLeft))
            {
                intersectionLeft.x += greenDotWidth / 2;  // 向右偏移
                return intersectionLeft;
            }
            if (LineSegmentsIntersection(playerScreenPos, lightboxScreenPos, new Vector2(screenRight, screenBottom), new Vector2(screenRight, screenTop), out Vector2 intersectionRight))
            {
                intersectionRight.x -= greenDotWidth / 2;  // 向左偏移
                return intersectionRight;
            }
            if (LineSegmentsIntersection(playerScreenPos, lightboxScreenPos, new Vector2(screenLeft, screenBottom), new Vector2(screenRight, screenBottom), out Vector2 intersectionBottom))
            {
                intersectionBottom.y += greenDotHeight / 2;  // 向上偏移
                return intersectionBottom;
            }
            if (LineSegmentsIntersection(playerScreenPos, lightboxScreenPos, new Vector2(screenLeft, screenTop), new Vector2(screenRight, screenTop), out Vector2 intersectionTop))
            {
                intersectionTop.y -= greenDotHeight / 2;  // 向下偏移
                return intersectionTop;
            }


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