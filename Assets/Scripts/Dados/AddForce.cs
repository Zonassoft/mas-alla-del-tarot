using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    [SerializeField]
    public float maxForce;
    
    [SerializeField]
    public float minForce;
    
    [SerializeField]
    public Vector3 forceDirectionVector;

    public GameObject dadoAzul;
    public GameObject dadoLila;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DadoAzul"))
        {
            float forceMagnitud = Random.Range(minForce, maxForce);
            Vector3 force = Vector3.Normalize(forceDirectionVector) * forceMagnitud;
            dadoAzul.GetComponent<Rigidbody>().AddForce(force);
            Debug.Log(force);
        }
        
        if (other.gameObject.CompareTag("DadoLila"))
        {
            float forceMagnitud = Random.Range(minForce, maxForce);
            Vector3 force = Vector3.Normalize(forceDirectionVector) * forceMagnitud;
            dadoLila.GetComponent<Rigidbody>().AddForce(force);
        }
    }
}
