using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecaShrek : MonoBehaviour
{
    [SerializeField] GameObject spawner;
    [SerializeField] LineRenderer lr1;
    [SerializeField] LineRenderer lr2;
    [SerializeField] Transform jugador;
    [SerializeField] Animator Boca;
    [SerializeField] AudioClip sndBurro;
    [SerializeField] Material matlaser;
    [SerializeField] Transform posLaser1;
    [SerializeField] Transform posLaser2;
    [SerializeField] Transform exploreja_izq;
    [SerializeField] Transform orejaIzqDesprendible;
    public int VidaOrejaIzq = 1;
    [SerializeField] Transform exploreja_der;
    [SerializeField] Transform orejaDerDesprendible;
    public int VidaOrejaDer = 1;
    public bool blLaserOn = false;
    int explosIzq = 0;
    int explosDer = 0;
    bool blInvulnerable = true;
    Vector3 posAntiguaJugador;
    [SerializeField] AudioClip sndExplosionGrande;
    public System.Action<Vector3> onScorch;

    void Start()
    {
        spawner.SetActive(false);
        StartCoroutine("Laser");
        lr1.enabled = false;
        lr2.enabled = false;
    }

    private void Awake()
    {
        jugador = GameManager.Instance.jugador;

    }

    private void FixedUpdate()
    {
        if (blLaserOn)
        {
            lr1.SetPosition(0, posLaser1.position);
            lr2.SetPosition(0, posLaser2.position);
        }
    }

        public void ActivarLaser()
    {
        blLaserOn = !blLaserOn;
        if (blLaserOn)
            Invoke("ActivarLaser", 7f);
        lr1.enabled = blLaserOn;
        lr2.enabled = blLaserOn;
    }


    public void Burro()
    {
        Boca.SetTrigger("hablar");
        GetComponent<AudioSource>().PlayOneShot(sndBurro);
        Invoke("ActivarLaser", 5f);
        Invoke("Burro", 23f);
    }


    IEnumerator Laser()
    {
        while (true)
        {
            if (blLaserOn)
            {
                lr1.SetPosition(1, posAntiguaJugador);
                lr2.SetPosition(1, posAntiguaJugador);
                if (Random.Range(0, 2) == 1)
                {
                    GameManager.Instance.Explosion(posAntiguaJugador);
                    onScorch?.Invoke(posAntiguaJugador);
                }
            }
                posAntiguaJugador = jugador.position;
            
            yield return new WaitForSeconds(1f);
        }
    }

    // Start is called before the first frame update

    public void IniciarSpawner()
    {
        blInvulnerable = false;
        spawner.SetActive(true);
        Invoke("Burro", 5f);
        SceneManager4.SM.MechaShrekOn = true;

    }

    public void Colisionado(object[] pars)
    {
        if (blInvulnerable)
            return;
        if (pars[0].ToString() == "exploder")
        {
            if (pars[1].ToString() == "20")
            {
                VidaOrejaIzq -= 20;
                if (VidaOrejaIzq <= 0 && explosIzq==0)
                    ExplotarOrejaI();
            }
            else if (pars[1].ToString() == "10")
            {
                VidaOrejaDer -= 20;
                if (VidaOrejaDer <= 0 && explosDer == 0)
                    ExplotarOrejaD();
            }
        }
    }

    void ExplotarOrejaI()
    {
        GameManager.Instance.Explosion(new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-2f, 2f), Random.Range(-0.5f, 0.5f)) + exploreja_izq.position);
        GetComponent<AudioSource>().PlayOneShot(sndExplosionGrande, 1f);
        explosIzq++;
        if (explosIzq!=3)
            Invoke("ExplotarOrejaI", 0.5f);
        else
        {
            orejaIzqDesprendible.parent = null;
            orejaIzqDesprendible.GetComponent<Rigidbody>().isKinematic = false;
            Destroy( orejaIzqDesprendible.GetComponent<capturaImpacto>());
            Invoke("EvaluarVida", 3f);
        }

    }

    void ExplotarOrejaD()
    {
        GameManager.Instance.Explosion(new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-2f, 2f), Random.Range(-0.5f, 0.5f)) + exploreja_der.position);
        GetComponent<AudioSource>().PlayOneShot(sndExplosionGrande, 1f);
        explosDer++;
        if (explosDer != 3)
            Invoke("ExplotarOrejaD", 0.5f);
        else
        {
            orejaDerDesprendible.parent = null;
            orejaDerDesprendible.GetComponent<Rigidbody>().isKinematic = false;
            Destroy(orejaDerDesprendible.GetComponent<capturaImpacto>());
            Invoke("EvaluarVida", 3f);
        }
    }


    void EvaluarVida()
    {
        if (explosDer == 3 && explosIzq == 3)
            SceneManager4.SM.Victorioso();
    }

    public void Temblar()
    {

    }




}
