using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximaBomba : MonoBehaviour
{
    [SerializeField]
    SceneManager2 SCMAN;
    // Start is called before the first frame update

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("bombarotadora"))
        {
            //if (!SCMAN.proximabomba = null)
                SCMAN.HayBomba(collision.gameObject);

        }
        else if (collision.gameObject.CompareTag("salvadora"))
        {
            SCMAN.HayBomba(collision.gameObject, true);

        }
    }


}
