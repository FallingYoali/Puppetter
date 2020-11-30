using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComportamientoBarraHP : MonoBehaviour
{

    public int vidaMax;
    public float vidaActual;
    public Image imagenBarraHp;



    // Start is called before the first frame update
    void Start()
    {
        
        
        if(gameObject.GetComponent<Bobby>())
        {
            vidaMax = GetComponent<Bobby>().hp;
        }
        else
        {
            vidaMax = GetComponent<Enemy>().hp;
        }

        vidaActual = vidaMax;
        Debug.Log(vidaMax);
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarVida();
        
    }


    public void ActualizarVida()
    {
        imagenBarraHp.fillAmount = vidaActual / vidaMax;
    }
}
