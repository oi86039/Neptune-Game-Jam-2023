using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    Vector3 position;
    void Update()
    {
        if (player.position.x > 6)
        {
            transform.position = new Vector3(player.position.x, player.position.y, -10);
        }
        else
            transform.position = new Vector3(6, player.position.y, -10);
    }
}
