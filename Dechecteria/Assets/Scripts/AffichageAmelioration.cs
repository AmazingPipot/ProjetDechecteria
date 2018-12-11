using Dechecteria;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffichageAmelioration : MonoBehaviour {
    public GameObject Colonie;
    public Transform amelioration;
    private List<int> listA;
    List<int> listControl = new List<int>();
    public GestionRoom Spr;
    
    void AffichageButton()
    {
        Colonie ColComp = Colonie.GetComponent<Colonie>();
        listA = ColComp.listAmelioration;
        for (int i = 0; i < listA.Count; i++)
        {
            if (listA[i] > 0 && (i < ColComp.listPieces.Count && ColComp.listPieces[i] > 0))
            {
                if (listControl[i] == 0)
                {
                    Spr = Colonie.GetComponent<Colonie>().listeSprites[i];
                    /*if (i < 7)
                    {
                        Spr = Colonie.GetComponent<Colonie>().listeSprites[i];
                    }
                    else if (i > 9)
                    {
                        Spr = Colonie.GetComponent<Colonie>().listeSprites[i - 3];
                    }*/
                    //listControl[i] = 1;
                    //Transform bt = Instantiate(amelioration);
                    //Vector3 pspr = Spr.GetComponent<Transform>().position;
                    //bt.transform.GetComponent<RectTransform>().position = new Vector3(115, -20, 0);//Vector3(pspr.x, pspr.y, pspr.z);
                }
            }
            else
            {
                listControl[i] = 0;
            }
        }
       

    }
	// Use this for initialization
	void Start () {
		for (int i = 0; i < 17/*Colonie.GetComponent<Colonie>().listAmelioration.Count*/; i++)
        {
            listControl.Add(0);
        }
	}
	
	// Update is called once per frame
	void Update () {
        AffichageButton();
	}
}
