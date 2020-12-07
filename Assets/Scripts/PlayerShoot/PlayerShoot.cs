using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerShoot : MonoBehaviour
{
    public Transform playerCamera;
    public int ammunition = 100;
    public float shootingDistance = 1000;
    //public int decalLimit = 50;
    public bool readyToFire = true;
    public ParticleSystem muzzleFlashPistolCamera;
    public ParticleSystem muzzleFlashPistolLocal;
    public ParticleSystem muzzleFlashPistolGlobal;
    public GameObject gunDecal;
    public LayerMask canShootAt;

    public PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

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
        muzzleFlashPistolLocal.Play();
        muzzleFlashPistolGlobal.Play();

        ammunition--;

        RaycastHit hit;

        //Shoots from playerCamera, change to weaponMuzzle for VR/third-person
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, shootingDistance, canShootAt))
        {
            if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Target") {
                enemyHealth = (EnemyHealth)hit.collider.gameObject.GetComponent("EnemyHealth");
                enemyHealth.Damage(1);
            } else {
                GameObject particle = Instantiate(gunDecal, hit.point, Quaternion.LookRotation(hit.normal));
                particle.SetActive(true);
                Destroy(particle, 60f);
            }
        }
    }
}
