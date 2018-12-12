using Dechecteria;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffichageInformationColonie : MonoBehaviour
{

    public Text textInfo0;
    public Text textInfo1;
    public Text textInfo2;
    public Text textInfo3;

	void Start ()
    {
        textInfo0.text = "";
        textInfo1.text = "";
        textInfo2.text = "";
        textInfo3.text = "";
    }
	
	// Update is called once per frame
	void Update () {

        textInfo0.text = "\nEnergie : " + Colonie.Instance.energie+" / "+ Colonie.Instance.energieMax;

        textInfo1.text = "Produit Organique : " + Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.ORGA].Resources.ToString() + "/" + Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.ORGA].MaxCapacity.ToString() + "\n"
            + " Minéral : " + Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.MINERAL].Resources.ToString() + "/" + Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.MINERAL].MaxCapacity.ToString() + "\n"
            + " Métal : " + Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.METAL].Resources.ToString() + "/" + Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.METAL].MaxCapacity.ToString() + "\n";

        textInfo2.text = " Pétrole : " + Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.PETROL].Resources.ToString() + "/" + Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.PETROL].MaxCapacity.ToString() + "\n"
            + " Chimique : " + Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.CHIMIC].Resources.ToString() + "/" + Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.CHIMIC].MaxCapacity.ToString() + "\n"
            + " Nucléaire : " + Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.NUCLEAR].Resources.ToString() + "/" + Colonie.Instance.ListeGestionRooms[(int)GameConstants.GestionRoomType.NUCLEAR].MaxCapacity.ToString() + "\n";

        textInfo3.text = "Vitesse : "+Colonie.Instance.listCapaciteCreature[0].ToString()+"\n"+
            "Attaque : " + Colonie.Instance.listCapaciteCreature[1].ToString() + "\n"+
            "Défense : " + Colonie.Instance.listCapaciteCreature[2].ToString();
        /*+" Minéral : " + Colonie.Instance.listReserve[1].ToString() + "/" + Colonie.GetComponent<Colonie>().reserveMax[1].ToString()*/;
    }
}
