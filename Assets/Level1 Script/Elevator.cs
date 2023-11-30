// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// [RequireComponent(typeof(Rigidbody2D))]
// public class ElevatorController : MonoBehaviour
// {
//     private Rigidbody2D rb;
//     public float requiredMass = 3f;  // 你所需要的玩家质量来激活电梯
//     public Text TextForElevator;
//     private static bool hasShownMessage = false;  // 静态变量，用于记录是否已经显示过消息

//     private void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         rb.isKinematic = true;  // 使电梯开始时为静止状态
//         if (TextForElevator == null)
//         {
//             Debug.LogWarning("TextForElevator is null. Please assign a Text object to this variable.");
//         }
//         else
//         {
//             TextForElevator.gameObject.SetActive(false);  // 开始时设置Text为不可见
//         }
//     }

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         Debug.Log("Collision detected with: " + collision.gameObject.name); // 输出碰撞对象的名称
//         if (collision.gameObject.CompareTag("Player"))
//         {
//             Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
//             if (playerRb != null)
//             {
//                 if (playerRb.mass >= requiredMass)
//                 {
//                     rb.isKinematic = false;
//                     Debug.Log("Elevator should fall now."); // 输出调试信息
//                 }
//                 else if (!hasShownMessage && TextForElevator != null)
//                 {
//                     TextForElevator.gameObject.SetActive(true);  // 如果玩家质量不足，显示Text
//                     hasShownMessage = true;  // 更新静态变量的值
//                 }
//             }
//         }
//     }
// }
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class ElevatorController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float requiredMass = 3f;  // 你所需要的玩家质量来激活电梯
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
        // 确保只在与玩家或石头碰撞时才执行
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Stone"))
        {
            // 获取电梯上所有具有 Rigidbody2D 组件的物体的总质量
            float totalMass = 0f;
            // 获取玩家的质量
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null && collision.gameObject.CompareTag("Player"))
            {
                totalMass += playerRb.mass;
            }
            
            // 累加电梯上子物体的质量
            foreach (Transform child in transform)
            {
                // 确保子物体带有“Stone”标签
                if (child.CompareTag("Stone"))
                {
                    Rigidbody2D childRb = child.GetComponent<Rigidbody2D>();
                    if (childRb != null)
                    {
                        totalMass += childRb.mass;
                    }
                }
            }
            
            // 输出电梯上所有物体的总质量
            Debug.Log("Total mass on elevator: " + totalMass);

            // 如果总质量超过了所需质量，激活电梯
            if (totalMass >= requiredMass)
            {
                rb.isKinematic = false;
                Debug.Log("Elevator is activated.");
            }
            else if (!hasShownMessage && TextForElevator != null)
            {
                TextForElevator.gameObject.SetActive(true);  // 如果质量不足，显示Text
                hasShownMessage = true;  // 更新静态变量的值
            }
        }
    }
}
