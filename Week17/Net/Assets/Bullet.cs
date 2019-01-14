using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        Debug.Log("hit");
        var combat = hit.GetComponent<Combat>();
        if (combat != null) { 
            combat.TakeDamage(30);
        }
        Destroy(gameObject, 0.01f);
        //DestroyImmediate(gameObject);

    }
}
