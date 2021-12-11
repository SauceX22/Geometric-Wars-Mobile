using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

public class BlueDiamond : MonoBehaviour
{
    Collider2D diamondCollider;
    public Collider2D DiamondCollider { get { return diamondCollider; } }

    BlueDiamondController blueDiamondController;
    public BlueDiamondController BlueDiamondController { get { return blueDiamondController; } }


    public void Initialize(BlueDiamondController controller)
    {
        blueDiamondController = controller;
    }

        void Start()
    {
        diamondCollider = GetComponent<Collider2D>();
    }

}