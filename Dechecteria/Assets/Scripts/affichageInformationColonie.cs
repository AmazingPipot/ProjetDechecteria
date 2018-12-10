using Dechecteria;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class affichageInformationColonie : MonoBehaviour {

    public Text textInfo;
    public GameObject Colonie;
	// Use this for initialization
	void Start () {
        textInfo.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        textInfo.text = " Produit Organique : " + Colonie.GetComponent<Colonie>().listReserve[0].ToString()+"/"+ Colonie.GetComponent<Colonie>().reserveMax[0].ToString()
            +" Minéral : " + Colonie.GetComponent<Colonie>().listReserve[1].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[1].ToString()
            +" Métal : " + Colonie.GetComponent<Colonie>().listReserve[2].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[2].ToString()
            +" Pétrole : " + Colonie.GetComponent<Colonie>().listReserve[3].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[3].ToString()
            +" Chimique : " + Colonie.GetComponent<Colonie>().listReserve[4].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[4].ToString()
            +" Nucléaire : " + Colonie.GetComponent<Colonie>().listReserve[5].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[5].ToString()
            /*+" Minéral : " + Colonie.GetComponent<Colonie>().listReserve[1].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[1].ToString()*/;
    }
}
