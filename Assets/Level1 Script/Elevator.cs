using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class ElevatorController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float requiredMass = 4f;  // 你所需要的玩家质量来激活电梯
    public Text TextForElevator;
    private static bool hasShownMessage = false;  // 静态变量，用于记录是否已经显示过消息

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;  // 使电梯开始时为静止状态
        if (TextForElevator == null)
        {
            Debug.LogWarning("TextForElevator is null. Please assign a Text object to this variable.");
        }
        else
        {
            TextForElevator.gameObject.SetActive(false);  // 开始时设置Text为不可见
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name); // 输出碰撞对象的名称
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                if (playerRb.mass >= requiredMass)
                {
                    rb.isKinematic = false;
                    Debug.Log("Elevator should fall now."); // 输出调试信息
                }
                else if (!hasShownMessage && TextForElevator != null)
                {
                    TextForElevator.gameObject.SetActive(true);  // 如果玩家质量不足，显示Text
                    hasShownMessage = true;  // 更新静态变量的值
                }
            }
        }
    }
}
