using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCollisionBullet_Script : MonoBehaviour
{
    public Animation door;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            door.enabled = true;
            Destroy(this.gameObject);
        }
    }
}
