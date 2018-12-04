using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SousBoutons : MonoBehaviour {

    public GameObject Colonie;
    public GameObject Controller;
    public GameObject Target;

    List<int> listNecessaire = new List<int>();//Quelle type de ressources avons nous besoins
    List<int> listRessource = new List<int>();//En quelle quantité

    /*public int organique = 0;// = Stocke matiere organique
    public int mineral = 0;// = Stocke verre
    public int metal = 0;// = Stocke métaux
    public int chimique = 0;// = Stocke produit chimique
    public int petrole = 0;// = Stocke petrole
    public int nucleaire = 0;// = Stocke nucléaire*/

    public int Type;//0 à n caractérise les pièces, n+1 a nn les pièces de traitements, nn+1 a nnn les améliorations
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

        List<int> Necessaire = new List<int>();
        if (necessaire0 != -1)
        {
            Necessaire.Add(necessaire0);
            print("JE VALIDE 0");
            if (necessaire1 != -1)
            {
                Necessaire.Add(necessaire1);
                print("JE VALIDE 1");
                if (necessaire2 != -1)
                {
                    Necessaire.Add(necessaire2);
                    print("JE VALIDE 2");
                    if (necessaire3 != -1)
                    {
                        Necessaire.Add(necessaire3);
                        print("JE VALIDE 3");
                        if (necessaire4 != -1)
                        {
                            Necessaire.Add(necessaire4);
                            print("JE VALIDE 4");

                        }
                    }
                }
            }
        }
        print("Liste prête");
        if (verificationList(Necessaire) == true)
        {
            print("Test de construction : \n");
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
        for (int i = 0; i < listNecessaire.Count; i++)
        {
            Colonie.transform.GetComponent<Colonie>().listReserve[i] -= listRessource[i];
        }

        if (Type < Controller.GetComponent<gestionEvolution>().nbPieceReserve)
        {
            Colonie.transform.GetComponent<Colonie>().listPieceReserve[Type] += 1;
        }
        listRessource.Clear();
        listNecessaire.Clear();
    }

    public bool verificationList(List<int> nec)
    {
        int ress = 0;//Calcul du nombre de ressource nécessaire
        print("Debut du test");
        for (int i = 0; i < nec.Count; i++)
        {
            print("JE suis dans la boucle");
            ress = 0;
            if (Type < Controller.GetComponent<gestionEvolution>().nbPieceReserve)
            {
                print("Je suis bien du bon type");
                if (nec[i] != -1)
                {
                    print("calcul des ressources");
                    ress = Colonie.GetComponent<Colonie>().listPieceReserve[Type] * 100 + (Controller.GetComponent<gestionEvolution>().nbPieceReserve / (nec[i]+1)) * 100;
                    print("calcul des ressources 2nd "+ress+ " "+ Controller.GetComponent<gestionEvolution>().nbPieceReserve+" "+ (nec[i]+1)+" "+ Controller.GetComponent<gestionEvolution>().nbPieceReserve / (nec[i] + 1));
                    if (ress > Colonie.GetComponent<Colonie>().listReserve[nec[i]])
                    {
                        print("Construction impossible : \n");
                        return false;
                    }
                    else
                    {
                        listNecessaire.Add(nec[i]);
                        listRessource.Add(ress);
                    }
                }
            }
        }
        /*if (Type < Controller.GetComponent<gestionEvolution>().nbPieceReserve)
        {
            //Target = Colonie.GetComponent<Colonie>().listPieceReserve;

            if (necessaire0 != -1)
            {
                if (100 > Colonie.GetComponent<Colonie>().listReserve[necessaire0])
                {
                    r = false;
                }
                else
                {
                    listNecessaire.Add(necessaire0);
                    listRessource.Add(Colonie.GetComponent<Colonie>().listPieceReserve[Type] * 100 + (Controller.GetComponent<gestionEvolution>().nbPieceReserve) / necessaire0 * 200);
                }
            }
        }*/
        print("Construction possible : \n");
        return true;
    }


    // Use this for initialization
    void Start () {
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000, 1000);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
