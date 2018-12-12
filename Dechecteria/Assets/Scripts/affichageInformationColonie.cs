using Dechecteria;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class affichageInformationColonie : MonoBehaviour {

    public Text textInfo0;
    public Text textInfo1;
    public Text textInfo2;
    public Text textInfo3;
    public GameObject Colonie;
	// Use this for initialization
	void Start () {
        textInfo0.text = "";
        textInfo1.text = "";
        textInfo2.text = "";
        textInfo3.text = "";
    }
	
	// Update is called once per frame
	void Update () {
        textInfo0.text = "\nEnergie : " + Colonie.GetComponent<Colonie>().energie+" / "+ Colonie.GetComponent<Colonie>().energieMax;

        textInfo1.text = "Produit Organique : " + Colonie.GetComponent<Colonie>().listReserve[0].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[0].ToString() + "\n"
            + " Minéral : " + Colonie.GetComponent<Colonie>().listReserve[1].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[1].ToString() + "\n"
            + " Métal : " + Colonie.GetComponent<Colonie>().listReserve[2].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[2].ToString() + "\n";

        textInfo2.text = " Pétrole : " + Colonie.GetComponent<Colonie>().listReserve[3].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[3].ToString() + "\n"
            + " Chimique : " + Colonie.GetComponent<Colonie>().listReserve[4].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[4].ToString() + "\n"
            + " Nucléaire : " + Colonie.GetComponent<Colonie>().listReserve[5].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[5].ToString() + "\n";

        textInfo3.text = "Vitesse : "+Colonie.GetComponent<Colonie>().listCapaciteCreature[0].ToString()+"\n"+
            "Attaque : " + Colonie.GetComponent<Colonie>().listCapaciteCreature[1].ToString() + "\n"+
            "Défense : " + Colonie.GetComponent<Colonie>().listCapaciteCreature[2].ToString();
        /*+" Minéral : " + Colonie.GetComponent<Colonie>().listReserve[1].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[1].ToString()*/;
    }
}
