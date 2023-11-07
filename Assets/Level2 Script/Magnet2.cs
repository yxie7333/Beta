using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet2 : MonoBehaviour
{
    private bool isEffectorActivated = false;
    private PointEffector2D pointEffector;
    private SpriteRenderer spriteRenderer;  // 新增：磁铁的SpriteRenderer组件

    private void Start()
    {
        pointEffector = GetComponent<PointEffector2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // 获取SpriteRenderer组件
        if (pointEffector == null)
        {
            Debug.LogError("No PointEffector2D found on this object!");
            return;
        }

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this object!");
            return;
        }

        // 初始时设置Point Effector 2D的Force Magnitude为0
        pointEffector.forceMagnitude = 0;
        spriteRenderer.color = Color.black;  // 初始时设置磁铁为黑色
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isEffectorActivated = !isEffectorActivated; // 切换Point Effector 2D的状态

            if (isEffectorActivated)
            {
                pointEffector.forceMagnitude = 200; // 根据你的需要调整此值
                spriteRenderer.color = Color.red;  // 当磁铁被激活时，设置为红色或其他亮色来高亮
            }
            else
            {
                pointEffector.forceMagnitude = 0;
                spriteRenderer.color = Color.black; // 当磁铁被关闭时，设置为黑色或其他原色来取消高亮
            }
        }
    }
}
