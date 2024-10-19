using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class WeaponController : MonoBehaviour
{
    public Weapon currentWeapon;
    public GameObject proximo; 
   
    public TMP_Text interactionText; 
    public Transform hand; 
    public bool isPicked = false; 
    public bool isLoaded = false;
    public Transform cameraTransform;

    private bool isAutomaticMode = false; 

    void Start()
    {
        interactionText.gameObject.SetActive(false); 
    }

    void Update()
    {
        if (isPicked)
        {
            HandleShooting();
            HandleAutomaticMode();

            
            if (Input.GetKeyDown(KeyCode.Q) && currentWeapon != null)
            {
                DropWeapon();
            }
        }

        CheckAndDropMagazine();

       
        if (proximo != null && Input.GetKeyDown(KeyCode.E))
        {
            if (proximo.CompareTag("Weapon"))
            {
                if (isPicked)
                {
                    DropWeapon();
                    PickupWeapon(proximo);

                }
                else
                {
                    PickupWeapon(proximo);
                }
            }else if(proximo.CompareTag("Magazine"))
            {
                PickupLoader(proximo);
            }
        }

        if (currentWeapon?.magazine != null) 
        {
            var bbAmmo = currentWeapon.magazine.GetComponent<MagazineController>()?.bbAmmo.GetComponent<BBController>();

            if (bbAmmo != null) 
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                {
                    bbAmmo.backspinDrag++;
                    Debug.Log("Hop-up: " + bbAmmo.backspinDrag);
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    bbAmmo.backspinDrag--;
                    Debug.Log("Hop-up: " + bbAmmo.backspinDrag);
                }
            }
        }

    }

    private void CheckAndDropMagazine()
    {
        if (currentWeapon?.magazine != null)
        {
            var magazineController = currentWeapon.magazine.GetComponent<MagazineController>();

            
            if (magazineController != null && magazineController.currentAmmo <= 0)
            {
                DropMagazine();
            }
        }
    }

    private void DropMagazine()
    {
        if (currentWeapon.magazine != null)
        {
            
            currentWeapon.magazine.transform.SetParent(null);
            currentWeapon.magazine.isPicked = false;

            Rigidbody rb = currentWeapon.magazine.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; 
                rb.detectCollisions = true; 
            }

            Debug.Log("Dropped magazine: " + currentWeapon.magazine.name);
            currentWeapon.magazine = null; 
        }
    }


    private void HandleShooting()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            if (!currentWeapon.isAutomaticMode)
            {
                currentWeapon.Shoot();
            }
            else
            {
                currentWeapon.AutomaticFire();
            }
            
        }

        if (Input.GetButtonUp("Fire1") && currentWeapon.isAutomaticMode)
        {
            currentWeapon.StopAutomaticFire();
        }
    }

    private void HandleAutomaticMode()
    {
       
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentWeapon.isAutomaticMode = !currentWeapon.isAutomaticMode;
            Debug.Log("Automatic mode: " + isAutomaticMode);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Debug.Log("Colidiu");
            interactionText.text = "Pressione 'E' para Pegar a Arma";
            interactionText.gameObject.SetActive(true);
            proximo = other.gameObject; 
        }
        else if (other.CompareTag("Magazine") && currentWeapon != null)
        {
            interactionText.text = "Pressione 'E' para Pegar o Carregador";
            interactionText.gameObject.SetActive(true);
            proximo = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            interactionText.gameObject.SetActive(false);
            proximo = null; 
        } else if(other.CompareTag("Magazine"))
        {
            interactionText.gameObject.SetActive(false);
            proximo = null;
        }
    }

    void PickupWeapon(GameObject weapon)
    {
        Debug.Log("Tentando pegar a arma");
        currentWeapon = weapon.GetComponent<Weapon>();
        if (currentWeapon != null)
        {
           
            currentWeapon.transform.position = hand.position;
            currentWeapon.transform.rotation = hand.rotation;
            currentWeapon.transform.SetParent(hand);
            currentWeapon.isPicked = true;
            isPicked = true;

            Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; 
                rb.detectCollisions = false; 
            }

            Debug.Log("Picked up weapon: " + currentWeapon.name);
        }
    }
    void PickupLoader(GameObject loader)
    {
        MagazineController currentLoader = loader.GetComponent<MagazineController>();
        
        if (currentLoader.weaponType == currentWeapon.weaponType)
        {
            currentLoader.transform.position = currentWeapon.GetComponent<Weapon>().loaderPosition.position;
            currentLoader.transform.rotation = currentWeapon.GetComponent<Weapon>().loaderPosition.rotation;
            currentLoader.transform.SetParent(currentWeapon.GetComponent<Weapon>().loaderPosition);
            currentLoader.isPicked = true;
            currentWeapon.magazine = currentLoader;
            isLoaded = true;
            Rigidbody rb = currentLoader.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.detectCollisions = false; 
            }
        }
        else
        {
            Debug.Log("Munição da arma errada");
        }
        
    }

    void DropWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.isPicked = false;
            Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; 
                rb.detectCollisions = true; 
                rb.AddForce(Camera.main.transform.forward * 5f, ForceMode.Impulse); 
            }

            isPicked = false; 
            Debug.Log("Dropped weapon: " + currentWeapon.name);
            currentWeapon = null; 
        }
    }

    public void AlignWeaponWithCamera()
    {
        cameraTransform = Camera.main.transform;

        
        transform.position = hand.position; 
        transform.rotation = Quaternion.Euler(0.231f, -15f, 0.297f);
 
        transform.position += cameraTransform.TransformDirection(new Vector3(0.1f, -0.2f, 1.0f)); 
    }
}

