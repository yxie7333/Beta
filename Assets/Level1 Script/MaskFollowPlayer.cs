using UnityEngine;

public class MaskFollowPlayer : MonoBehaviour
{
    public Transform player; // ��ҵ�Transform���

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
    }
}

