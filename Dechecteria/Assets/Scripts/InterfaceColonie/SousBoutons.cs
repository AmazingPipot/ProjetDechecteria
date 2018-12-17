using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dechecteria
{
    class SousBoutons : MonoBehaviour
    {
        List<int> listNecessaire = new List<int>();//Quelle type de ressources avons nous besoins
        List<int> listRessource = new List<int>();//En quelle quantité

        /*public int organique = 0;// = Stocke matiere organique
        public int mineral = 0;// = Stocke verre
        public int metal = 0;// = Stocke métaux
        public int chimique = 0;// = Stocke produit chimique
        public int petrole = 0;// = Stocke petrole
        public int nucleaire = 0;// = Stocke nucléaire*/

        public GameConstants.GestionRoomType Type;//0 à n caractérise les pièces, n+1 a nn les pièces de traitements, nn+1 a nnn les améliorations
        //public GameConstants.CapaciteCreature Type2;
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

        int CorrectVal = 0;

        Vector2 scaleRoom;
        float tempsConstructionRoom = 8.0f;

        public void OnMouseDown()
        {
            print("DEBUT DES EVOLUTIONS DES PIECES "+ Convert.ToInt32(Type));
            if (Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Amelioration > 0)//(Colonie.Instance.listAmelioration[(int)Convert.ToInt32(Type)] > 0)
            {
                print("DEBUT DES EVOLUTIONS DES PIECES2");
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
                if (VerificationList(Necessaire) == true)
                {
                    print("Construction possible : \n");
                    Construction();
                }
                //Colonie.Instance.listAmelioration[(int)Convert.ToInt32(Type)] -= 1;
            }
        }

        public void Construction()
        {
            for (int i = 0; i < listNecessaire.Count; i++)
            {
                Colonie.Instance.ListeGestionRooms[i].Resources -= listRessource[i];
            }

            //Amelioration de la capacité des reserves
            if  (Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].isRecyclageRoom == false)
            {
                Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].MaxCapacity = /*(Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level + 1) */(int)(2.5 * Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].MaxCapacity);
            }
            else //Amelioration de la vitesse d'absorption des déchets
            {
                Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].vitesseAbsorption *= 2;//(Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].vitesseAbsorption + 1);
            }

            Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Amelioration -= 1;
            Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level += 1;

            Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].RoomDisplay.rectTransform.localScale = new Vector2(0, 0);
            StartCoroutine(TempsConstruction());

            listRessource.Clear();
            listNecessaire.Clear();
        }

        IEnumerator TempsConstruction()
        {
            while (tempsConstructionRoom > 0)
            {
                //attendre 1 seconde
                yield return new WaitForSeconds(1.0f);
                Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].AnimationRoom(scaleRoom);
                tempsConstructionRoom--;
            }
        }

        public bool VerificationList(List<int> nec)
        {
            int ress = 0;//Calcul du nombre de ressource nécessaire
            bool res = true;

            coeff1 = 0;
            coeff2 = 0;
            coeff3 = 0;


            print("COOUCOUU");
            if (Convert.ToInt32(Type) < GestionEvolution.Instance.nbPieceReserve)
            {
                print(Convert.ToInt32(Type) + " " + GestionEvolution.Instance.nbPieceReserve);
                //coeff2 = Colonie.Instance.listPieceReserve[Convert.ToInt32(Type)] + 1;
                coeff2 = Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level + 1;
                print("Coeff2 " + coeff2);
                for (int i = 0; i < nec.Count; i++)
                {
                    ress = 0;

                    if (Convert.ToInt32(Type) < GestionEvolution.Instance.nbPieceReserve)
                    {
                        if (nec[i] != -1)
                        {
                            print("avant le coeff ");
                            coeff1 = GestionEvolution.Instance.baseRessourcePieceReserve[i];
                            print("coeff1 " + coeff1);
                            print("Coeff " + coeff1 + " " + coeff2);
                            ress = (int)(2.5 * coeff2 * coeff1);//coeff2 * coeff1 + (Convert.ToInt32(Type) + 1) * coeff1 * coeff2 + GestionEvolution.Instance.nbRessource / (nec[i] + 1) * coeff1 * coeff2;
                            print("calcul des ressources " + ress + " " + GestionEvolution.Instance.nbPieceReserve + " " + (nec[i] + 1) + " " + GestionEvolution.Instance.nbPieceReserve / (nec[i] + 1));
                            if (ress > Colonie.Instance.ListeGestionRooms[nec[i]].Resources)
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
            else if (Convert.ToInt32(Type) < GestionEvolution.Instance.nbPieceRecyclage)
            {
                //coeff2 = Colonie.Instance.listPieceRecyclage[Type] + 1;
                CorrectVal = 0;// GestionEvolution.Instance.nbPieceReserve;

                //coeff2 = Colonie.Instance.listPieceRecyclage[Convert.ToInt32(Type) - CorrectVal] + 1;
                coeff2 = Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level + 1;

                for (int i = 0; i < nec.Count; i++)
                {
                    ress = 0;

                    //coeff3 = Colonie.transform.GetComponent<Colonie>().listCapaciteCreature[Convert.ToInt32(Type) - coeff2] + 1;

                    if (nec[i] != -1)
                    {
                        coeff1 = GestionEvolution.Instance.baseRessourcePieceReserve[i];

                        print("Coeff " + coeff1 + " " + coeff2);
                        ress = (int)(2.75 * coeff2 * coeff1);//coeff2 * coeff1 + (Convert.ToInt32(Type) + 1) * coeff1 * coeff2 + GestionEvolution.Instance.nbRessource / (nec[i] + 1) * coeff1 * coeff2;
                        //print("calcul des ressources " + ress + " " + GestionEvolution.Instance.nbPieceReserve + " " + (nec[i] + 1) + " " + GestionEvolution.Instance.nbPieceReserve / (nec[i] + 1));
                        if (ress > Colonie.Instance.ListeGestionRooms[nec[i]].Resources)
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
            /*else if (Convert.ToInt32(Type) < GestionEvolution.Instance.nbCapacite)
            {
                print("No soucy " + Type + " " + GestionEvolution.Instance.nbPieceRecyclage );
                CorrectVal = GestionEvolution.Instance.nbPieceRecyclage;
                coeff3 = Colonie.Instance.listCapaciteCreature[Convert.ToInt32(Type) - CorrectVal] + 1;
                //Colonie.transform.GetComponent<Colonie>().listCapaciteCreature[Type - coeff2] = ress;
                res = true;
                print("Je suis une amelioration");
                for (int j = 0; j < coeff3; j++)
                {
                    print("Je suis dans la boucle");

                    coeff1 = GestionEvolution.Instance.baseRessourceAmelioration[j];

                    ress = coeff1 * coeff3 ;
                    print("Ressource "+ress);
                    if (ress > Colonie.Instance.ListeGestionRooms[j].Resources)
                    {
                        res = false;
                    }
                    else
                    {
                        listNecessaire.Add(j);
                        listRessource.Add(ress);
                    }
                }
                print("Tout c'est bien passé");
                return res;
            }*/
        

            print("Fin de la verification");
            return res;
        }

        void VerificationAmelioration()
        {
            /*Color C10 = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            Color C11 = new Color(0.25f, 0.25f, 0.25f, 1.0f);
            Color C12 = new Color(0.3f, 0.3f, 0.3f, 1.0f);

            Color C20 = new Color(0.55f, 0.55f, 0.55f, 1.0f);
            Color C21 = new Color(0.6f, 0.6f, 0.6f, 1.0f);
            Color C22 = new Color(0.65f, 0.65f, 0.65f, 1.0f);

            var cb = this.GetComponent<Button>().colors;

            for (int i = 0; i < Colonie.Instance.listAmelioration.Count; i++)
            {
                if (Colonie.Instance.listAmelioration[0] > 0)
                {
                    //ColorBlock cb = this.GetComponent<Button>().colors;
                    cb.normalColor = C20;
                    this.transform.GetComponent<Button>().colors = cb;

                    cb.highlightedColor = C21;
                    this.transform.GetComponent<Button>().colors = cb;

                    cb.pressedColor = C22;
                    this.transform.GetComponent<Button>().colors = cb;
                }
                else
                {
                    cb.highlightedColor = C10;
                    this.transform.GetComponent<Button>().colors = cb;

                    cb.highlightedColor = C11;
                    this.transform.GetComponent<Button>().colors = cb;

                    cb.pressedColor = C12;
                    this.transform.GetComponent<Button>().colors = cb;
                }
            }*/
        }
        
        // Use this for initialization
        void Start () {
            scaleRoom = Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].RoomDisplay.rectTransform.localScale;
            //transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000, 1000);
        }
	
	    // Update is called once per frame
	    void Update () {
            VerificationAmelioration();           
        }
    }
}
