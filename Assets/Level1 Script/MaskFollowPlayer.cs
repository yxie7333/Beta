using UnityEngine;

public class MaskFollowPlayer : MonoBehaviour
{
    public Transform player; // 玩家的Transform组件

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
    }
}

