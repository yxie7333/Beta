using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet2 : MonoBehaviour
{
    // 引用两个不同状态的Sprite
    public Sprite inactiveSprite;
    public Sprite activeSprite;

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
        // 初始时设置为非激活的Sprite
        spriteRenderer.sprite = inactiveSprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isEffectorActivated = !isEffectorActivated; // 切换Point Effector 2D的状态

            if (isEffectorActivated)
            {
                pointEffector.forceMagnitude = 200; // 根据你的需要调整此值
                spriteRenderer.sprite = activeSprite; // 切换到激活状态的Sprite
            }
            else
            {
                pointEffector.forceMagnitude = 0;
                spriteRenderer.sprite = inactiveSprite; // 切换回非激活状态的Sprite
            }
        }
    }
}
