using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class ElevatorController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float requiredMass = 3f;
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
