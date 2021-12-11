using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(10f, 30f)]
    public float speed = 20f;
    [Range(5f, 15f)]
    public float fireRate = 13f;
    [Range(0f, 1f)]
    public float offset = .4f;
    [Range(0f, 2f)]
    public float floatBulletOffset;

    private float nextTimeToFire = 0f;
    private Quaternion bulletOffset;
    private Quaternion bulletOffset2;

    public GameObject bullet;
    public Transform firePoint;
    public Transform firePointAxis;

    public Joystick firingJoystick;
    public Joystick movingJoystick;


    //Bounds Stuff
    public Transform playField;

    private float charWidth;
    private float charHeight;

    private float playFieldWidth;
    private float playFieldHeight;

    void Start()
    {
        playFieldWidth = playField.transform.localScale.x * 10 / 2; //extents = size of width / 2
        playFieldHeight = playField.transform.localScale.z * 10 / 2; //extents = size of height / 2

        charWidth = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        charHeight = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2

        bulletOffset = Quaternion.AngleAxis(floatBulletOffset, Vector3.forward);
        bulletOffset2 = Quaternion.AngleAxis(-floatBulletOffset, Vector3.forward);
    }

    void Update()
    {
        var firingDir = firingJoystick.Direction;
        var firingAngle = Mathf.Atan2(firingDir.y, firingDir.x) * Mathf.Rad2Deg;
        firePointAxis.rotation = Quaternion.AngleAxis(firingAngle, Vector3.forward);

        var movingDir = movingJoystick.Direction;
        var movingAngle = Mathf.Atan2(movingDir.y, movingDir.x) * Mathf.Rad2Deg;

        if (movingJoystick.Direction.magnitude > .3f)
        {
            transform.rotation = Quaternion.AngleAxis(movingAngle, Vector3.forward);
            transform.Translate(movingDir * speed * Time.deltaTime, Space.World);
        }
        else
            transform.rotation = transform.rotation;


        if (firingJoystick.Direction.magnitude > .3f && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            Instantiate(bullet, new Vector3(firePoint.position.x - .8f, firePoint.position.y, firePoint.position.z), firePoint.rotation * bulletOffset);
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            Instantiate(bullet, new Vector3(firePoint.position.x - .8f, firePoint.position.y, firePoint.position.z), firePoint.rotation * bulletOffset2);
        }
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, playFieldWidth * -1 + charWidth + offset, playFieldWidth - charWidth - offset);
        viewPos.y = Mathf.Clamp(viewPos.y, playFieldHeight * -1 + charHeight + offset, playFieldHeight - charHeight - offset);
        transform.position = viewPos;
    }
}
