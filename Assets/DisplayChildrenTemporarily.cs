using UnityEngine;
using System.Collections;

public class DisplayChildrenTemporarily : MonoBehaviour
{
    public GameObject parentObject; // 父 GameObject，它的子对象将被临时显示
    private bool hasDisplayed = false; // 标记以确保子对象只显示一次

    void Update()
    {
        // 当玩家的 x 坐标首次大于 156 时
        if (transform.position.x > 156f && !hasDisplayed)
        {
            // 设置标记，避免重复显示
            hasDisplayed = true;
            // 显示子对象 10 秒
            StartCoroutine(DisplayChildrenForTime(10f));
        }
    }

    private IEnumerator DisplayChildrenForTime(float time)
    {
        // 激活所有子对象
        SetChildrenActive(true);
        // 等待指定的时间
        yield return new WaitForSeconds(time);
        // 隐藏所有子对象
        SetChildrenActive(false);
    }

    // 激活或隐藏父 GameObject 的所有子对象
    private void SetChildrenActive(bool state)
    {
        foreach (Transform child in parentObject.transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}
