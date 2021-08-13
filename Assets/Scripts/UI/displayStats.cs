using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayStats : Singleton<displayStats>
{
    [SerializeField] TMPro.TextMeshProUGUI txtVidas;
    [SerializeField] Image imgVida;
    [SerializeField] TMPro.TextMeshProUGUI txtMisiles;
    [SerializeField] GameObject UIMuerto;
    [SerializeField] Animator UIDamaged;


    private void Start()
    {
        UIMuerto.SetActive(false);            
        txtMisiles.gameObject.SetActive(false);

    }
    private void OnEnable()
    {
        GameManager.Instance.OnCambioVidaJugador += RefrescarVida;
        GameManager.Instance.OnCambioVidas += RefrescarVidas;
        GameManager.Instance.OnCambioMisiles += RefrescarVidas;
        GameManager.Instance.onRestart += killMe;
        GameManager.Instance.onPlayerdied += MurioElJugador;
        GameManager.Instance.onRespawn += RespawnJugador;
        GameManager.Instance.onPlayerDamaged += Damaged;


    }

    private void OnDisable()
    {
        GameManager.Instance.OnCambioVidaJugador -= RefrescarVida;
        GameManager.Instance.OnCambioVidas -= RefrescarVidas;
        GameManager.Instance.OnCambioMisiles -= RefrescarVidas;
        GameManager.Instance.onRestart -= killMe;
        GameManager.Instance.onPlayerdied -= MurioElJugador;
        GameManager.Instance.onRespawn -= RespawnJugador;
        GameManager.Instance.onPlayerDamaged -= Damaged;

    }

    void RefrescarVida(float valor)
    {
        imgVida.fillAmount = valor;
    }
    void RefrescarVidas(int valor)
    {
        txtVidas.text = valor.ToString();
    }
    void RefrescarMisiles(int valor)
    {
        if (valor > 0)
        {
            txtMisiles.gameObject.SetActive(true);
            txtMisiles.text = valor.ToString();
        }
    }
    void MurioElJugador()
    {
        UIMuerto.SetActive(true);
    }
    void RespawnJugador()
    {
        UIMuerto.SetActive(false);
    }

    void killMe()
    {
        Destroy(this.gameObject);
    }

    void Damaged()
    {
        UIDamaged.SetTrigger("ouch");
    }

}
