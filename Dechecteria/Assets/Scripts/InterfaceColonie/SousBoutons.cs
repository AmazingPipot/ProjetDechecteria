using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SousBoutons : MonoBehaviour {

    public GameObject Colonie;
    public GameObject Controller;
    public GameObject Target;

    int[] TabNecessaire = new int[7];//Quelle type de ressources avons nous besoins
    List<int> listRessource = new List<int>();//En quelle quantité

    /*public int organique = 0;// = Stocke matiere organique
    public int mineral = 0;// = Stocke verre
    public int metal = 0;// = Stocke métaux
    public int chimique = 0;// = Stocke produit chimique
    public int petrole = 0;// = Stocke petrole
    public int nucleaire = 0;// = Stocke nucléaire*/

    public int Type;//0 caractérise les pièces, 1 les pièces de traitements, 2 les améliorations
    /*
     * Chaque amélioraton nécessite au max n éléments
     * 1 = orga;
     * 2 = ...
    */
    public int necessaire0;//
    public int necessaire1;//
    public int necessaire2;//
    public int necessaire3;//
    public int necessaire4;//

    public void OnMouseDown()
    {
        print("JE SUIS CLIQUE");
        if (verificationList() == true)
        {
            print("construction possible");
            construction();
        }
    }

    public void ajoutList()
    {
        //listNecessaire.Add(a);
        //TabNecessaire[necessaire] = necessaire+1;
    }

    public void construction()
    {

    }

    public bool verificationList()
    {
        bool r = true;
        
        if (necessaire0 != 0)
        {
            //if ()
        }
        /*for (int i = 0; i < TabNecessaire.Length; i++)
        {
            if (TabNecessaire[i] != 0)
            {
                listRessource[i] = Colonie.GetComponent<Colonie>().listReserve[i];

                if (listRessource[i] > Colonie.GetComponent<Colonie>().listReserve[TabNecessaire[i]])
                {
                    r = false;
                }
            }
        }*/
        return r;
    }

    // Use this for initialization
    void Start () {
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000, 1000);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
