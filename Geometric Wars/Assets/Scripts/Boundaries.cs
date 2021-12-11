using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public Transform playField;
    public float offset = .4f;
    private float playerWidth;
    private float playerHeight;

    private float playFieldWidth;
    private float playFieldHeight;

    // Use this for initialization
    void Start()
    {
        //screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        playerWidth = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        playerHeight = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2

        playFieldWidth = playField.transform.GetComponent<SpriteRenderer>().bounds.extents.x - offset; //extents = size of width / 2
        playFieldHeight = playField.transform.GetComponent<SpriteRenderer>().bounds.extents.y - offset; //extents = size of height / 2
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, playFieldWidth * -1 + playerWidth, playFieldWidth - playerWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, playFieldHeight * -1 + playerHeight, playFieldHeight - playerHeight);
        transform.position = viewPos;
    }
}
