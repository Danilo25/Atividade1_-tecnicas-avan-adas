using System;
using System.Collections.Generic;
using UnityEngine;

public class BBController : MonoBehaviour
{
    public float lifetime = 10f; 
    public float backspinDrag = 0.001f; 
    public Rigidbody rb;
    public int damage = 15;

    private LineRenderer lineRenderer;
    private List<Vector3> positions = new List<Vector3>(); 

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        lineRenderer = GetComponent<LineRenderer>(); 
        lineRenderer.positionCount = 0;
        Destroy(gameObject, lifetime); 
    }

    void FixedUpdate()
    {
        
        positions.Add(transform.position); 
        lineRenderer.positionCount = positions.Count; 
        lineRenderer.SetPositions(positions.ToArray()); 

        float velocidadeXZ = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        float liftForceMagnitude = backspinDrag * Mathf.Sqrt(velocidadeXZ);
        rb.AddForce(liftForceMagnitude * transform.up * Time.fixedDeltaTime, ForceMode.Force);
    }

    private void OnDestroy()
    {
        lineRenderer.positionCount = 0;
    }
}
