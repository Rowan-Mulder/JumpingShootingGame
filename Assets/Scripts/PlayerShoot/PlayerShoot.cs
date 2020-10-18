using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerShoot : MonoBehaviour
{
    public Transform weaponMuzzle;
    public Transform playerCamera;
    public int ammunition = 100;
    public bool readyToFire = true;
    public ParticleSystem muzzleFlashPistolCamera;
    public ParticleSystem muzzleFlashPistolGlobal;
    public GameObject gunDecal;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (ammunition > 0) {
                Shoot();
            }
            readyToFire = false;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            readyToFire = true;
        }
    }

    void Shoot()
    {
        muzzleFlashPistolCamera.Play();
        muzzleFlashPistolGlobal.Play();

        ammunition--;

        RaycastHit hit;

        //Shoots from playerCamera, change to weaponMuzzle for VR
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit))
        {
            GameObject particle = Instantiate(gunDecal, hit.point, Quaternion.LookRotation(hit.normal));
            particle.SetActive(true);
            Destroy(particle, 60f);
        }
    }
}
