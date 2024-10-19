using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public float springForce = 1.2f; 
    public Transform point; 
    public float rpm = 4.5f; 
    private bool isShooting = false; 

    private void Start()
    {
        weaponType = "Pistol";
        point = transform.GetChild(0);
        loaderPosition = transform.GetChild(1);
        isAutomatic = false; 
    }

    public override void Shoot()
    {
        if (magazine.GetComponent<MagazineController>().currentAmmo > 0 && !isShooting)
        {
            StartCoroutine(ShootRF());
        }
    }

    public IEnumerator ShootRF()
    {
        isShooting = true;

        GameObject bb = Instantiate(magazine.GetComponent<MagazineController>().bbAmmo, point.position, point.rotation);
        Rigidbody rbBB = bb.GetComponent<Rigidbody>();
        float velocity = CalculateInitialVelocity(springForce, rbBB.mass);
        rbBB.velocity = point.forward * velocity;
        magazine.GetComponent<MagazineController>().currentAmmo--;

        Debug.Log("Pistol shot: " + velocity + " m/s, Ammo left: " + magazine.GetComponent<MagazineController>().currentAmmo);

        
        yield return new WaitForSeconds(1f / rpm);

        isShooting = false;
    }

    public override void AutomaticFire()
    {
        // Não implementado pois a pistola é semiautomática
    }

    public override void StopAutomaticFire()
    {
        StopAllCoroutines();
        isShooting = false; 
    }
}
