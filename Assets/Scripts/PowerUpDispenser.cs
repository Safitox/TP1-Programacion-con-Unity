using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDispenser:MonoBehaviour
{
    [SerializeField]  POWERUPS[] PowerUps;
    private int totalChance;
    private void Start()
    {
        for (int i =0; i<PowerUps.Length;i++)     
        {
            totalChance += PowerUps[i].chancePonderado;
        }
        
    }

    public void dropPowerUp(Vector3 pos)
    {
        int valor = Random.Range(0, totalChance);
        int acc = 0;
        for (int i = 0; i < PowerUps.Length; i++)
        {
            acc += PowerUps[i].chancePonderado;
            if (valor < acc)
            {
                valor = i;
                break;
            }

        }
        GameObject.Instantiate(PowerUps[valor].goPowerUp, pos, Quaternion.identity);
    }
    
}

[System.Serializable]
public struct POWERUPS
{
    public GameObject goPowerUp;
    public int chancePonderado;
}