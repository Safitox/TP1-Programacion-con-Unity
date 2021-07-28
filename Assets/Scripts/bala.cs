using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bala : MonoBehaviour
{
    float fusibleVida = 0;
    [SerializeField] float VidaMaxima = 7f;
    [SerializeField] bool blExplosiva = false;
    [SerializeField] float volumenDisparo;
    [SerializeField] AudioClip sndDisparo;
    private void OnEnable()
    {
        fusibleVida = Time.timeSinceLevelLoad + VidaMaxima;
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().PlayOneShot(sndDisparo, volumenDisparo);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //     Debug.Log(collision.gameObject.name);
        if (blExplosiva)
            GameManager.GM.Explosion(transform.position);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (Time.timeSinceLevelLoad > fusibleVida)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }

    }



}
