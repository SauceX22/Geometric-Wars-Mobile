using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class JoinedPinkCube : MonoBehaviour
{
    JoinedPinkCubesController pinkCubeController;
    public JoinedPinkCubesController PinkCubeController { get { return pinkCubeController; } }
    Collider2D pinkCubeCollider;
    public Collider2D PinkCubeCollider { get { return pinkCubeCollider; } }

    private int[] directions = new int[] { 90, 180, 270, 360 };

    private float step;
    private float movingTime;
    [SerializeField]
    [Range(0, 10)]
    private float flipTimer;

    private int flipTimerHolder;
    private int chosenDir;
    private bool timerSet = false;
    private bool dirSet = false;
    private Vector3 moveDirection;

    void Start()
    {
        pinkCubeCollider = GetComponent<Collider2D>();
    }

    public void Initialize(JoinedPinkCubesController controller)
    {
        pinkCubeController = controller;
        flipTimerHolder = controller.flipTimer;
        movingTime = controller.movingTime;
        step = controller.step;
    }

    void Update()
    {
        if (timerSet)
        {
            //Resetting The Timer
            flipTimer = flipTimerHolder;
            timerSet = false;
        }
    }

    private void FixedUpdate()
    {
        flipTimer -= Time.deltaTime;
        if (flipTimer <= 0)
        {
            if (!dirSet)
            {
                chosenDir = directions[Random.Range(0, 3)]; 
                dirSet = true;
            }
            //UP
            if (chosenDir == 90)
            {
                //North
                //UP
                chosenDir = 90;
                moveDirection = Vector3.up;

                StartCoroutine(Move(moveDirection));
            }
            else
            //DOWN
            if (chosenDir == 270)
            {
                //South
                //Down
                chosenDir = 270;
                moveDirection = Vector3.down;

                StartCoroutine(Move(moveDirection));
            }
            else
            //LEFT
            if (chosenDir == 180)
            {
                //West
                //Left
                chosenDir = 180;
                moveDirection = Vector3.left;

                StartCoroutine(Move(moveDirection));
            }
            else
            //RIGHT
            if (chosenDir == 360)
            {
                //East
                //Right
                chosenDir = 360;
                moveDirection = Vector3.right;

                StartCoroutine(Move(moveDirection));
            }
        }
    }

    IEnumerator Move(Vector3 direction)
    {
        //transform.position = Vector3.SmoothDamp(transform.position, direction * step, ref velocity, smoothTime);
        transform.position += (direction * step) / 10;
        yield return new WaitForSeconds(movingTime);
        timerSet = true;
        dirSet = false;
    }
}