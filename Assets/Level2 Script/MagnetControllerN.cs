using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MagnetControllerN : MonoBehaviour
{
    private bool isEffectorActivated = false;
    private PointEffector2D pointEffector;

    private void Start()
    {
        pointEffector = GetComponent<PointEffector2D>();
        if (pointEffector == null)
        {
            Debug.LogError("No PointEffector2D found on this object!");
            return;
        }

        // 初始时设置Point Effector 2D的Force Magnitude为0
        pointEffector.forceMagnitude = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isEffectorActivated = !isEffectorActivated; // 切换Point Effector 2D的状态

            if (isEffectorActivated)
            {
                pointEffector.forceMagnitude = 20; // 根据你的需要调整此值
            }
            else
            {
                pointEffector.forceMagnitude = 0;
            }
        }
    }
}
