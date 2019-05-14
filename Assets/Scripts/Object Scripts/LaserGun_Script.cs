using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun_Script : MonoBehaviour
{
    private GameObject _bullet;
    private GameObject _bulletPrefab;
    public AudioClip laserGunSounds;
    public float defaultVolume = 10;

    private float _cooldownFireTime = 1.0f;
    private float _cooldownCurrentTime = 0.0f;

    void Fire()
    {
        if (_bulletPrefab == null)
        {
            _bulletPrefab = (GameObject)Resources.Load("Prefabs/Bullet", typeof(GameObject));
        }
        Player_Controller.animator.Play("Shoot");

        _bullet = Instantiate(_bulletPrefab, transform.position + transform.forward * 0.6f,transform.rotation);
        _bullet.GetComponent<Rigidbody>().velocity = _bullet.transform.forward * 70;
        //_bullet.GetComponent<Rigidbody>().AddForce(_bullet.transform.forward * 70);
    }

    private void Update()
    {
        _cooldownCurrentTime += Time.deltaTime;

        if (_cooldownCurrentTime >= _cooldownFireTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _cooldownCurrentTime = 0.0f;

                if (laserGunSounds != null)
                {
                    AudioSource.PlayClipAtPoint(laserGunSounds, transform.position, defaultVolume);
                }
                Fire();
            }
        }

        Destroy(_bullet, 3.0f);
    }
}
