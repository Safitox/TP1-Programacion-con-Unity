using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayStats : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI txtVidas;
    [SerializeField] Image imgVida;
    [SerializeField] TMPro.TextMeshProUGUI txtMisiles;
    [SerializeField] GameObject UIMuerto;
    [SerializeField] GameObject UIPausa;
    [SerializeField] Animator UIDamaged;

    private void Start()
    {
        UIMuerto.SetActive(false);
        UIPausa.SetActive(false);
        txtMisiles.gameObject.SetActive(false);
    }

    private void Awake() => DontDestroyOnLoad(gameObject);

    private void OnEnable()
    {
        GameManager.Instance.OnCambioVidaJugador += RefrescarVida;
        GameManager.Instance.OnCambioVidas += RefrescarVidas;
        GameManager.Instance.OnCambioMisiles += RefrescarMisiles;
        GameManager.Instance.onRestart += killMe;
        GameManager.Instance.onPlayerdied += MurioElJugador;
        GameManager.Instance.onRespawn += RespawnJugador;
        GameManager.Instance.onPlayerDamaged += Damaged;
        GameManager.Instance.OnPressEscape += Pausa;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCambioVidaJugador -= RefrescarVida;
        GameManager.Instance.OnCambioVidas -= RefrescarVidas;
        GameManager.Instance.OnCambioMisiles -= RefrescarMisiles;
        GameManager.Instance.onRestart -= killMe;
        GameManager.Instance.onPlayerdied -= MurioElJugador;
        GameManager.Instance.onRespawn -= RespawnJugador;
        GameManager.Instance.onPlayerDamaged -= Damaged;
        GameManager.Instance.OnPressEscape -= Pausa;
    }

    void RefrescarVida(float valor) => imgVida.fillAmount = valor;
    void RefrescarVidas(int valor) => txtVidas.text = valor.ToString();
    void RefrescarMisiles(int valor)
    {
        if (valor > 0)
        {
            txtMisiles.gameObject.SetActive(true);
            txtMisiles.text = valor.ToString();
        }
    }
    void MurioElJugador() => UIMuerto.SetActive(true);
    void RespawnJugador() => UIMuerto.SetActive(false);
    void killMe() => Destroy(this.gameObject);
    void Damaged() => UIDamaged.SetTrigger("ouch");
    public void butSalir() => GameManager.Instance.Quit();
    public void butMenu() => GameManager.Instance.Restart();
    public void butContinue()
    {
        UIPausa.SetActive(false);
        GameManager.Instance.Resume();
    }

    void Pausa() => UIPausa.SetActive(true);

}
