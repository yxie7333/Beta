using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Proyecto26;

public class MagnetController2 : MonoBehaviour
{
    private bool isEffectorActivated = false;
    private PointEffector2D pointEffector;
    private SpriteRenderer spriteRenderer;  // 新增：磁铁的SpriteRenderer组件

    private string playerID = System.Guid.NewGuid().ToString();
    private int Magnet1Count = 0;

    [System.Serializable]
    public class AnalyticMagnet
    {
        public string Magnet1Count;
    }

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
        spriteRenderer.color = Color.grey;  // 初始时设置磁铁为灰色 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isEffectorActivated = !isEffectorActivated; // 切换Point Effector 2D的状态

            if (isEffectorActivated)
            {
                pointEffector.forceMagnitude = -20; // 根据你的需要调整此值
                spriteRenderer.color = Color.blue;  // 当磁铁被激活时，设置为蓝色或其他亮色来高亮
            }
            else
            {
                pointEffector.forceMagnitude = 0;
                spriteRenderer.color = Color.grey;  // 当磁铁被关闭时，设置为灰色或其他原色来取消高亮

                // 每次关闭磁铁时，累加Magnet1Count的值并发送数据至Firebase
                Magnet1Count++;
                SendDataToFirebase();
            }
        }
    }

    private void SendDataToFirebase()
    {
        string levelInf = "2";
        string stageInf = "3";
        AnalyticMagnet analyticmagnet = new AnalyticMagnet();
        analyticmagnet.Magnet1Count = Magnet1Count.ToString();
        string analyticJson = JsonUtility.ToJson(analyticmagnet);
        string DBurl = "https://yanjungu-unity-analytics-default-rtdb.firebaseio.com/" + "levels/" + levelInf + "/stages/" + stageInf +
        "/players/" + playerID + ".json";
        RestClient.Post(DBurl, analyticJson);
    }
}
