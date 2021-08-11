using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrDJ : MonoBehaviour
{
    [SerializeField] AudioClip Musica;
    private AudioSource audio;
    private void Start()
    {
        if (!GetComponent<AudioSource>())
            audio = gameObject.AddComponent<AudioSource>() as AudioSource;
        else
            audio = GetComponent<AudioSource>();

        audio.volume = GameManager.Instance.volMusica;
        audio.loop = true;
        EstadoTutorial(GameManager.Instance.blTutorial);
    }
    private void OnEnable()
    {
        GameManager.Instance.OnCambioEstadoTutorial += EstadoTutorial;
        GameManager.Instance.OnCambioEstadoGame += EstadoGame;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCambioEstadoTutorial -= EstadoTutorial;
        GameManager.Instance.OnCambioEstadoGame -= EstadoGame;
    }


    void EstadoTutorial(bool OnOff)
    {
        if (OnOff)
        {
            audio.clip = GameManager.Instance.MusicaTutorial;
            audio.Play();
        }
    }
    void EstadoGame(bool OnOff)
    {
        if (OnOff)
        {
            audio.clip = Musica;
            audio.Play();
        }
    }
}
