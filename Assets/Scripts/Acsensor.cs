using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acsensor : MonoBehaviour
{
    [SerializeField]
    float minY=5000;
    [SerializeField]
    Transform[] Piezas;
    Vector3 iniY=new Vector3(0,-5000,0);
    public float velocidad = 1f;
    Vector3 V;

    private void Start()
    {
        foreach (Transform T in Piezas)
        {
            if (T.position.y < minY)
                minY = T.position.y;
            if (T.position.y > iniY.y)
                iniY = T.position;
        }
    }


    void FixedUpdate()
    {
        if (velocidad != 0f)
        {
            foreach (Transform T in Piezas)
            {
                if (T.position.y < minY)
                    T.position = iniY;
                else
                {
                    V = new Vector3(T.position.x, T.position.y + velocidad * Time.fixedDeltaTime, T.position.z);
                    T.position = V;
                }
            }
        }


    }

}
