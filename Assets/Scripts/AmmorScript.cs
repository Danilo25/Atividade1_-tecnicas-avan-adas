using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineScript : MonoBehaviour
{
    public GameObject bbAmmo;
    public float bbMass;
    public int maxAmmo = 30;
    public int currentAmmo;
    // Start is called before the first frame update
    void Start()
    {
        bbAmmo.GetComponent<Rigidbody>().mass = bbMass;
        currentAmmo = maxAmmo;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
