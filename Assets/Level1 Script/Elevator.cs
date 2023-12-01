using System.Collections;
<<<<<<< Updated upstream
using System.Collections.Generic;
=======
>>>>>>> Stashed changes
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class ElevatorController : MonoBehaviour
{
    private Rigidbody2D rb;
<<<<<<< Updated upstream
    public float requiredMass = 4f;  // 你所需要的玩家质量来激活电梯
=======
    public float requiredMass = 3f;
>>>>>>> Stashed changes
    public Text TextForElevator;
    private static bool hasShownMessage = false;

    public float detectionRadius = 5f;
    public Transform detectionPoint;

    private float totalMass; // 声明一个变量来存储总质量

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        if (TextForElevator == null)
        {
            Debug.LogWarning("TextForElevator is null. Please assign a Text object to this variable.");
        }
        else
        {
            TextForElevator.gameObject.SetActive(false);
        }

        // 初始化时计算一次总质量
        totalMass = CalculateTotalMassInRange();
        Debug.Log("Initial total mass: " + totalMass); // 输出初始总质量
    }

    private void Update()
    {
        totalMass = CalculateTotalMassInRange(); // 每帧更新总质量

        if (totalMass >= requiredMass && rb.isKinematic)
        {
            rb.isKinematic = false;
            Debug.Log("Elevator is activated.");
            if (TextForElevator != null)
            {
                TextForElevator.gameObject.SetActive(false);
            }
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
<<<<<<< Updated upstream
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
=======
        // 检查电梯是否与地面碰撞
        if (collision.gameObject.CompareTag("ground"))
        {
            rb.isKinematic = true;
            Debug.Log("Elevator collided with ground and is now Kinematic");
            // 调用方法以在3秒后使电梯消失
            Invoke("DisappearElevator", 1f);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (totalMass < requiredMass)
            {
                if (!hasShownMessage && TextForElevator != null)
                {
                    TextForElevator.gameObject.SetActive(true);
                    hasShownMessage = true;
                    StartCoroutine(HideTextAfterTime(3f)); // 启动协程，参数为3秒
>>>>>>> Stashed changes
                }
            }
        }
    }

    private void DisappearElevator()
    {
        // 使电梯消失的逻辑
        //Debug.Log("Elevator will now disappear.");
        Destroy(gameObject); // 销毁电梯对象
    }


    private float CalculateTotalMassInRange()
    {
        float totalMass = 0f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionPoint.position, detectionRadius);
        foreach (var collider in colliders)
        {
            // 确保只计算特定标签的对象
            if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Stone"))
            {
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    totalMass += rb.mass;
                }
            }
        }

        //Debug.Log("Total mass in range: " + totalMass);
        return totalMass;
    }


    private void OnDrawGizmos()
    {
        if (detectionPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(detectionPoint.position, detectionRadius);
        }
    }

    IEnumerator HideTextAfterTime(float time)
    {
        yield return new WaitForSeconds(time); // 等待指定的时间
        TextForElevator.gameObject.SetActive(false);
        hasShownMessage = false;
    }


}
