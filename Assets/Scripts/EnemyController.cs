using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.5f; 
    public int life = 50;
    public int damage = 10;
    private Transform player;
    public ZoneController currentZone;
    private ZoneController nextZone;
    private bool isChasing = true; 
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        currentZone = GetComponentInParent<ZoneController>(); 
    }

    void Update()
    {
        
    }

    private void ChasePlayer()
    {
        if (!isChasing) return; 

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("BB"))
        {
            TakeDamage(collision.gameObject.GetComponent<BBController>().damage);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            StopChasing(1f); 
        }
    }

    private void TakeDamage(int amount)
    {
        life -= amount;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            ZoneController newZone = other.GetComponent<ZoneController>();
            if (newZone != null && newZone.isActive)
            {
                nextZone = newZone;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            ZoneController exitedZone = other.GetComponent<ZoneController>();
            if (exitedZone != null && exitedZone == currentZone)
            {
                currentZone = nextZone;
                nextZone = null;
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentZone != null && currentZone.isActive)
        {
            ChasePlayer();
        }

        if (nextZone != null)
        {
            currentZone = nextZone;
            nextZone = null;
        }
    }

    private void StopChasing(float pauseDuration)
    {
        isChasing = false; 
        Invoke("ResumeChasing", pauseDuration); 
    }

    private void ResumeChasing()
    {
        isChasing = true; 
    }
}
