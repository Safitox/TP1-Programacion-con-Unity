using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exploder : MonoBehaviour
{

    private void OnEnable()
    {
        gameObject.GetComponent<AudioSource>().Play();

    }


    public void Desactivar()
    {
        this.gameObject.SetActive(false);
    }
}
