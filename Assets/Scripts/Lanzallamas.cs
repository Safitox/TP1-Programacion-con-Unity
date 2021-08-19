using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanzallamas : MonoBehaviour
{

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            if (Random.Range(1, 20) == 1)
                GameManager.Instance.JugadorTocado(1);
        }
    }
}
