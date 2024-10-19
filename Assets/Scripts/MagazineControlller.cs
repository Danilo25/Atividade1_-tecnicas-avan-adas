using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineController: MonoBehaviour
{
    public GameObject bbAmmo;
    public bool isPicked;
    public float bbMass;
    public int maxAmmo = 30;
    public int currentAmmo;
    public string weaponType;
    public float despawnTime = 30f;
    void Start()
    {
        if (bbAmmo != null)
        {
            bbAmmo.GetComponent<Rigidbody>().mass = bbMass;
        }
        currentAmmo = maxAmmo;
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
