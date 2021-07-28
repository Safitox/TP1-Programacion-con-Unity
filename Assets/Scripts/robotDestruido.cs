using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotDestruido : MonoBehaviour
{
    // Start is called before the first frame update

    public void Destruir()
    {
        gameObject.GetComponent<Animator>().enabled = false;
        Invoke("Desaparecer", 15f);
    }


    void Desaparecer()
    {
        gameObject.GetComponent<Animator>().enabled = true;
        gameObject.SetActive(false);


    }


}
