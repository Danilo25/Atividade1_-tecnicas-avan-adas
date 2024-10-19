using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    
    public float springForce = 1.49f;
    public Transform point;
    public float rpm = 4.5f;
    public bool isShooting = false;
    
    private void Start()
    {
        weaponType = "Rifle";
        point = transform.GetChild(0);
        loaderPosition = transform.GetChild(1);
        isAutomatic = true; 
    }


    public override void Shoot()
    {
        if (magazine.GetComponent<MagazineController>().currentAmmo > 0 && !isShooting)
        {
            StartCoroutine(ShootRF());
        }
    }

    public override void AutomaticFire()
    {
        if (!isShooting && magazine.GetComponent<MagazineController>().currentAmmo > 0)
        {
            StartCoroutine(AutomaticFireCoroutine());
        }
    }

    public override void StopAutomaticFire()
    {
        StopAllCoroutines();
        isShooting = false; 
    }

    public IEnumerator ShootRF()
    {
        isShooting=true;

        GameObject bb = Instantiate(magazine.GetComponent<MagazineController>().bbAmmo, point.position, point.rotation);
        Rigidbody rbBB = bb.GetComponent<Rigidbody>();
        float velocity = CalculateInitialVelocity(springForce, rbBB.mass);
        rbBB.velocity = point.forward * velocity;
        magazine.GetComponent<MagazineController>().currentAmmo--;

        Debug.Log("Rifle shot: " + velocity + " m/s, Ammo left: " + magazine.GetComponent<MagazineController>().currentAmmo);

        
        yield return new WaitForSeconds(1f/rpm);

        isShooting = false;
    }

    private IEnumerator AutomaticFireCoroutine()
    {
        while (magazine.GetComponent<MagazineController>().currentAmmo > 0)
        {
            Shoot(); 
            yield return new WaitForSeconds(1f / rpm); 
        }

        StopAutomaticFire(); 
    }
}

