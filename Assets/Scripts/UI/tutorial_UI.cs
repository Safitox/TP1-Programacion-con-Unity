using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial_UI : MonoBehaviour
{
    [SerializeField] GameObject panelTutorial;

    private void Start()
    {
        Show(GameManager.Instance.blTutorial);
    }
    private void OnEnable()
    {
        GameManager.Instance.OnCambioEstadoTutorial += Show;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCambioEstadoTutorial -= Show;
    }


    private void Show(bool OnOff)
    {
        panelTutorial.SetActive(OnOff);
    }
}
