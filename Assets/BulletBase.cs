using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletBase : MonoBehaviour {

    public int BulletSpeed = 1;
    public int BulletDamage = 5;
    public int WeaponFireRate = 3;
    public int BulletLifetime = 3;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(BulletDamage);
        }

        Destroy(gameObject);
    }
}
