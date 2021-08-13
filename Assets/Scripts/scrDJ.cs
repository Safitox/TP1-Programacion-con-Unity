﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrDJ : MonoBehaviour
{
    [SerializeField] AudioClip Musica;
    private AudioSource _audio;
    private void Start()
    {
        if (!GetComponent<AudioSource>())
            _audio = gameObject.AddComponent<AudioSource>() as AudioSource;
        else
            _audio = GetComponent<AudioSource>();

        _audio.volume = GameManager.Instance.volMusica;
        _audio.loop = true;
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
            _audio.clip = GameManager.Instance.MusicaTutorial;
            _audio.Play();
        }
    }
    void EstadoGame(bool OnOff)
    {
        if (OnOff)
        {
            _audio.clip = Musica;
            _audio.Play();
        }   
    }
}
