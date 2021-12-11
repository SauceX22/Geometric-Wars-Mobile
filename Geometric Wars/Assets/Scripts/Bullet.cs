using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Bullet : MonoBehaviour
{

    public float bulletSpeed = 5f; 
    void Update()
    {
        transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        string tag = obj.gameObject.tag;

        switch (tag)
        {
            case "BlueDiamondEnemy": //Enemy
                Destroy(this.gameObject);
                var diamond = obj.GetComponent<BlueDiamond>();
                diamond.BlueDiamondController.myDiamonds.Remove(diamond);
                Destroy(obj.transform.gameObject);
                break;
            case "PinkCubeEnemy":
                Destroy(this.gameObject);
                var pinkCube = obj.GetComponent<JoinedPinkCube>();
                pinkCube.PinkCubeController.myCubes.Remove(pinkCube);
                Destroy(obj.transform.gameObject);
                break;

            case "Wall":
                Destroy(this.gameObject);
                break;
            default:
                return;
                break;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyMe());
    }

    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
