using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class CycleSkyBox : MonoBehaviour
{
    public List<Material> skys;
    [Range(0f, 20f)]
    public float changeSkyTimer = 20f;
    [Range(0f, 30f)]
    public float skyRotSpeed = 1f;
    [Range(1f, 10f)]
    public float skyRotMultiplier = 3f;
    [Range(0f, 2f)]
    public float speed = 0.125f;

    float val = 0f;
    float rot = 0f;

    private float skyTimerHolder;
    private bool setRot = false;

    void Start()
    {
        skyTimerHolder = changeSkyTimer;
        RenderSettings.skybox.SetFloat("_Rotation", 0f);
    }

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", rot += skyRotSpeed * Time.deltaTime * skyRotMultiplier);
        //changeSkyTimer -= Time.deltaTime;
        //if (changeSkyTimer <= 0.00f)
        //{
        //    //StartCoroutine(WaitToChange());
        //    changeSkyTimer = 0f;
        //    RenderSettings.skybox.SetFloat("_Blend", Mathf.Sin(val += speed * Time.deltaTime));
        //}
    }
    IEnumerator WaitToChange()
    {
        changeSkyTimer = 0f;
        yield return new WaitForSeconds(3f);
        changeSkyTimer = skyTimerHolder;
    }
}
