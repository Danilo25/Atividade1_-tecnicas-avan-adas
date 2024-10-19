using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public float springForce = 1.5f; 
    public Transform point; 
    public int pelletCount = 10; 
    public float rpm = 2f; 
    private bool isShooting = false; 

    private void Start()
    {
        weaponType = "Shotgun";
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

        for (int i = 0; i < pelletCount; i++)
        {
            GameObject bb = Instantiate(magazine.GetComponent<MagazineController>().bbAmmo, point.position, point.rotation);
            Rigidbody rbBB = bb.GetComponent<Rigidbody>();
            float velocity = CalculateInitialVelocity(springForce, rbBB.mass);

            float spread = Random.Range(-0.1f, 0.1f); 
            Vector3 direction = point.forward + new Vector3(spread, spread, 0);
            rbBB.velocity = direction.normalized * velocity;
        }

        magazine.GetComponent<MagazineController>().currentAmmo--;
        Debug.Log("Shotgun fired: " + pelletCount + " pellets, Ammo left: " + magazine.GetComponent<MagazineController>().currentAmmo);
        
        yield return new WaitForSeconds(1f / rpm);

        isShooting = false;
    }

    public override void AutomaticFire()
    {
        // Não implementado pois a shotgun é semiautomática
    }

    public override void StopAutomaticFire()
    {
        StopAllCoroutines();
        isShooting = false; 
    }
}
