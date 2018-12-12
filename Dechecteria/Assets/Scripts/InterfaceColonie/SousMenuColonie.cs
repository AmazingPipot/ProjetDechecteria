using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SousMenuColonie : MonoBehaviour {
    public GameObject Gestion;
    public GameObject gestionAffichage;

    public int clic;
    public int T = 0;

    public void OnMouseDown()
    {
        clic += 1;
        clic = clic % 2;

        T = Gestion.GetComponent<gestionEvolution>().T;
    }

    public void affichageSousBoutons()
    {
        if (clic == 1)
        {
            gestionAffichage.SetActive(true);
        }
        else
        {
            gestionAffichage.SetActive(false);
        }
    }

    void Start () {
        clic = 0;
	}


	// Update is called once per frame
	void Update () {
        affichageSousBoutons();
    }
}
