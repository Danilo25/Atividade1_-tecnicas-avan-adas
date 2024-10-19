using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool isAutomatic;
    public bool isPicked;
    public MagazineController magazine;
    public Transform loaderPosition;
    public string weaponType;
    public bool isAutomaticMode = false;
    public float despawnTime = 30f;
    public abstract void Shoot(); 

    public abstract void AutomaticFire();

    public abstract void StopAutomaticFire();

    protected float CalculateInitialVelocity(float energy, float massInGrams)
    {
        float massInKilograms = massInGrams / 1000f;
        return Mathf.Sqrt(2 * energy / massInKilograms);
    }

    private void OnEnable()
    {
        StartCoroutine(DespawnIfNotEquipped());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator DespawnIfNotEquipped()
    {
        yield return new WaitForSeconds(despawnTime);

        if (!isPicked)
        {
            Destroy(gameObject); 
        }
    }
}
