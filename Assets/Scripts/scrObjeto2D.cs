using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrObjeto2D : MonoBehaviour
{
    Transform jugador;
    private bool visible = false;
    void Start()
    {
        jugador = GameManager.Instance.jugador;
    }

    private void FixedUpdate()
    {

        Visibilidad((transform.position - jugador.position).sqrMagnitude <= 100);
        if (visible)
        {
            Vector3 targetPostition = new Vector3(jugador.position.x,
                        this.transform.position.y,
                        jugador.position.z);
            this.transform.LookAt(targetPostition);

        }
    }

    void Visibilidad(bool estado)
    {
        if (visible !=estado)
            transform.GetChild(0).GetComponent<Animator>().SetBool("aparecer", estado);
        visible = estado;

    }
}
