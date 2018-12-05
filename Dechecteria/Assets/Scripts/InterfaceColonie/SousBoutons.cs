using Dechecteria;
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

    int coeff1 = 0;
    int coeff2 = 0;
    int coeff3 = 0;

    public void OnMouseDown()
    {
        List<int> Necessaire = new List<int>();
        if (necessaire0 != -1)
        {
            Necessaire.Add(necessaire0);
            if (necessaire1 != -1)
            {
                Necessaire.Add(necessaire1);
                if (necessaire2 != -1)
                {
                    Necessaire.Add(necessaire2);
                    if (necessaire3 != -1)
                    {
                        Necessaire.Add(necessaire3);
                        if (necessaire4 != -1)
                        {
                            Necessaire.Add(necessaire4);
                        }
                    }
                }
            }
        }
        if (verificationList(Necessaire) == true)
        {
            print("Construction possible : \n");
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
            print(Type);
        }
        else if (Type < Controller.GetComponent<gestionEvolution>().nbAmelioration)
        {
            Colonie.transform.GetComponent<Colonie>().listCapaciteCreature[Type-coeff2] += 1;
        }

        listRessource.Clear();
        listNecessaire.Clear();
    }

    public bool verificationList(List<int> nec)
    {
        int ress = 0;//Calcul du nombre de ressource nécessaire
        bool res = true;

        coeff1 = 0;
        coeff2 = 0;
        coeff3 = 0;

        coeff2 = Colonie.GetComponent<Colonie>().listPieceReserve[Type] + 1;

        print("COOUCOUU");
        if (Type < Controller.GetComponent<gestionEvolution>().nbPieceReserve)
        {
            for (int i = 0; i < nec.Count; i++)
            {
                ress = 0;

                //coeff3 = Colonie.transform.GetComponent<Colonie>().listCapaciteCreature[Type - coeff2] + 1;

                if (Type < Controller.GetComponent<gestionEvolution>().nbPieceReserve)
                {
                    if (nec[i] != -1)
                    {
                        coeff1 = Controller.GetComponent<gestionEvolution>().baseRessourcePiece[i];

                        print("Coeff " + coeff1 + " " + coeff2);
                        ress = coeff2 * coeff1 + (Type + 1) * coeff1 * coeff2 + Controller.GetComponent<gestionEvolution>().nbRessource / (nec[i] + 1) * coeff1 * coeff2;
                        print("calcul des ressources " + ress + " " + Controller.GetComponent<gestionEvolution>().nbPieceReserve + " " + (nec[i] + 1) + " " + Controller.GetComponent<gestionEvolution>().nbPieceReserve / (nec[i] + 1));

                        if (ress > Colonie.GetComponent<Colonie>().listReserve[nec[i]])
                        {
                            print("Construction impossible : \n");
                            res = false;
                        }
                        else
                        {
                            listNecessaire.Add(nec[i]);
                            listRessource.Add(ress);
                        }
                    }
                }
            }
        }
        else if (Type < Controller.GetComponent<gestionEvolution>().nbAmelioration)
        {
            print("No soucy " + Type + " " + Controller.GetComponent<gestionEvolution>().nbPieceReserve);
            /*coeff3 = Colonie.transform.GetComponent<Colonie>().listCapaciteCreature[Type - Controller.GetComponent<gestionEvolution>().nbPieceReserve] + 1;
            //Colonie.transform.GetComponent<Colonie>().listCapaciteCreature[Type - coeff2] = ress;
            res = true;
            print("Je suis une amelioration");
            for (int j = 0; j < coeff3; j++)
            {
                print("Je suis dans la boucle");
                coeff1 = Controller.GetComponent<gestionEvolution>().baseRessourceAmelioration[i];
                ress = coeff1 * coeff3 ;
                print("Ressource "+ress);
                if (ress > Colonie.GetComponent<Colonie>().listReserve[j])
                {
                    res = false;
                }
                else
                {
                    listNecessaire.Add(j);
                    listRessource.Add(ress);
                }
            }
            print("Tout c'est bien passé");*/
            return false;
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
        return res;
    }


    // Use this for initialization
    void Start () {
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000, 1000);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
