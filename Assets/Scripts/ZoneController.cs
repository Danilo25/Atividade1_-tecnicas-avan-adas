using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    public bool isActive = false;
    public int idZone = 0;
    public GameObject[] enemies; 
    public GameObject[] magazines; 
    public GameObject[] weapons; 

    private Vector3 zoneSize = new Vector3(150, 1, 150);
    private const int maxWeapons = 5;
    private const int maxMagazines = 20;

    private float spawnInterval = 60f; 
    private float spawnTimer = 0f;
    private int enemyCount = 2; 

    private float weaponSpawnInterval = 30f; 

    void Start()
    {
        
        StartCoroutine(SpawnEnemiesRoutine());
        StartCoroutine(SpawnWeaponsRoutine()); 
    }

    void Update()
    {
        
        if (isActive)
        {
            
            if (Time.time % 60 < Time.deltaTime) 
            {
                enemyCount += 2;
                Debug.Log("Inimigos aumentados: " + enemyCount);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeactivateZone();
        }
    }

    private void ActivateZone()
    {
        if (!isActive)
        {
            isActive = true;
            SpawnRandomItems(); 
            Debug.Log("Zona ativada: " + gameObject.name);
        }
    }

    private void DeactivateZone()
    {
        if (isActive)
        {
            isActive = false;
            Debug.Log("Zona desativada: " + gameObject.name);
            
        }
    }

    public void SpawnRandomItems()
    {
        if (!isActive) return;

       
        for (int i = 0; i < maxWeapons; i++)
        {
            SpawnItem(weapons);
        }

        
        for (int i = 0; i < maxMagazines; i++)
        {
            SpawnItem(magazines);
        }
    }

    private void SpawnItem(GameObject[] itemArray)
    {
        if (itemArray.Length == 0) return;

        Vector3 zonePosition = transform.position;
        Vector3 randomPosition = new Vector3(
            Random.Range(zonePosition.x - zoneSize.x / 2, zonePosition.x + zoneSize.x / 2),
            zonePosition.y, 
            Random.Range(zonePosition.z - zoneSize.z / 2, zonePosition.z + zoneSize.z / 2)
        );

        int itemIndex = Random.Range(0, itemArray.Length);
        Instantiate(itemArray[itemIndex], randomPosition, Quaternion.identity);
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (isActive)
            {
                for (int i = 0; i < enemyCount; i++)
                {
                    SpawnEnemy();
                }
            }
        }
    }

    private IEnumerator SpawnWeaponsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(weaponSpawnInterval);

            if (isActive)
            {
                SpawnRandomItems(); 
            }
        }
    }

    private void SpawnEnemy()
    {
        if (enemies.Length == 0) return;

        Vector3 zonePosition = transform.position;
        Vector3 randomPosition = new Vector3(
            Random.Range(zonePosition.x - zoneSize.x / 2, zonePosition.x + zoneSize.x / 2),
            zonePosition.y, 
            Random.Range(zonePosition.z - zoneSize.z / 2, zonePosition.z + zoneSize.z / 2)
        );

        int enemyIndex = Random.Range(0, enemies.Length);
        Instantiate(enemies[enemyIndex], randomPosition, Quaternion.identity);
    }
}