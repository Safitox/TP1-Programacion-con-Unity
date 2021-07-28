using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombaSalvadora : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.Find("SceneManager").GetComponent<SceneManager2>().ActivarSalida();
            gameObject.SetActive(false);

        }
        else if (collision.gameObject.CompareTag("piso"))
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        
    }
}
