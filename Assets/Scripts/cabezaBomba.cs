using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cabezaBomba : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro txtTimer;
    public float tiempoBoom = 5f;
    [SerializeField] AudioClip beep;
    GameManager GM;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    

    public void Iniciar(float tiempo)
    {
        tiempoBoom = tiempo + Time.timeSinceLevelLoad;
        Invoke("Autodestruccion", 1f);

    }

    void Autodestruccion()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(beep, 0.5f);
        float quedan = tiempoBoom - Time.timeSinceLevelLoad;
        if (quedan<=0)
        {
            GM.Explosion(transform.position);
            gameObject.SetActive(false);
        }
        else if (quedan <=  2f)
        {
            txtTimer.text = quedan.ToString("F1");
            Invoke("Autodestruccion", 0.2f);
        }
        else
        {
            txtTimer.text = ((int)quedan).ToString();
            Invoke("Autodestruccion", 1f);
        }


    }
}
