using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrMenu : MonoBehaviour

{ 
    public void Iniciar()
    {
        GameManager.Instance.PASEDENIVEL(1);
    }
}
