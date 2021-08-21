using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrScorch : MonoBehaviour
{

    [SerializeField] GameObject pf_Scorch;
    MecaShrek MS;
    // Start is called before the first frame update

    private void Awake()
    {
        MS = GameObject.Find("MecaShrek").GetComponent<MecaShrek>();   
    }
    private void OnEnable()
    {

        MS.onScorch += Scorch;

    }

    private void OnDisable()
    {
        MS.onScorch -= Scorch;
    }
    void Scorch(Vector3 pos)
    {
        GameObject go = Instantiate(pf_Scorch, new Vector3(pos.x,-3f,pos.z), pf_Scorch.transform.rotation);
        Destroy(go, 40f);

    }

}
