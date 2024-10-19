using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class GameController : MonoBehaviour
{
    public TMP_Text lifeText;
    public TMP_Text ammoText;
    public TMP_Text timeText;
    public GameObject player;
    public List<ZoneController> zones;

    private const float activationDistance = 100f;
    public HashSet<ZoneController> activeZones = new HashSet<ZoneController>();

    private const float totalGameTime = 300f; 
    private float timeRemaining;

    void Start()
    {
        ammoText.gameObject.SetActive(false);
        lifeText.SetText("Vida: " + player.GetComponent<PlayerController>().currentLife);
        timeRemaining = totalGameTime;
        StartCoroutine(TimerRoutine());
    }

    void Update()
    {
        lifeText.SetText("Vida: " + player.GetComponent<PlayerController>().currentLife);
        UpdateAmmoText();

    }

    private void UpdateAmmoText()
    {
        var weaponController = player.GetComponent<WeaponController>();
        if (weaponController != null && weaponController.currentWeapon != null)
        {
            var magazineController = weaponController.currentWeapon.magazine.GetComponent<MagazineController>();
            if (magazineController != null)
            {
                ammoText.SetText(magazineController.currentAmmo + "|" + magazineController.maxAmmo);
                ammoText.gameObject.SetActive(true);
            }
            else
            {
                ammoText.gameObject.SetActive(false);
            }


        }
        else
        {
            ammoText.gameObject.SetActive(false);
        }
    }

    private IEnumerator TimerRoutine()
    {
        while (timeRemaining > 0)
        {
           
            timeText.SetText("Tempo: " + Mathf.Ceil(timeRemaining) + "s");
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        timeText.SetText("Fim de Jogo");
    }

}
