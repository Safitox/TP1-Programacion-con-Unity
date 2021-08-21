using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] int addVida = 0;
    [SerializeField] bool ExtraLife = false;
    [SerializeField] int dmgBoost = 0;
//    [SerializeField] float tpoVida = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (ExtraLife)
                GameManager.Instance.Vidas++;
            else if (addVida != 0)
                GameManager.Instance.AddVida(addVida);
            else if (dmgBoost != 0)
                GameManager.Instance.AddPlusDmg();
            GameManager.Instance.RecogerPowerUp();
            Destroy(this.gameObject);
        }
    }

}
