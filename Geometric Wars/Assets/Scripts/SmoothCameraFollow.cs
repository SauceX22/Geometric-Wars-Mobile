using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform player;
    public Transform playField;
    public Camera camera;

    private Vector3 velocity = Vector3.zero;
    private Vector3 pos;

    public float a = 0.017f;
    public float smoothTime = 0.125f;
    public float distance = -30f;
    public float offset = 3f;

    void LateUpdate()
    {
        if (player.position.x >= 1)
            pos.x = player.position.x + offset;
        else if (player.position.x <= -1)
            pos.x = player.position.x - offset;

        if (player.position.y >= 1)
            pos.y = player.position.y + offset;
        else if (player.position.y <= -1)
            pos.y = player.position.y - offset;
        pos.z = distance;


        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smoothTime);
        transform.LookAt(a * new Vector3(-player.position.x, -player.position.y, player.position.z));
       
        //Vector3 point = camera.WorldToViewportPoint(target);
        //Vector3 delta = target - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        //Vector3 destination = transform.position + delta;

        //transform.localRotation = rot;
    }
}
