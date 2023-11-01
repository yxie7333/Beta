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

    }

    private void Update()
    {

    }


    public void FlashLeft()
    {
        if (leftWallText != null && rightWallText != null)
        {
            StartCoroutine(FlashAndText(leftFlash, leftWallText));
        } else
        {
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

}