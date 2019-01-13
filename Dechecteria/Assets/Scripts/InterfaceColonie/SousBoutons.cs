using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{
    class SousBoutons : MonoBehaviour
    {
        public GameObject source;
        AudioSource son;
        List<int> listNecessaire = new List<int>();//Quelle type de ressources avons nous besoins
        List<int> listRessource = new List<int>();//En quelle quantité

        Color C10 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color C11 = new Color(0.95f, 0.95f, 0.95f, 1.0f);
        Color C12 = new Color(0.65f, 0.65f, 0.65f, 1.0f);

        Color C20 = new Color(0.5f, 0.5f, 0.5f, 1.0f);

        public Button m_button;
        public GameConstants.GestionRoomType Type;//0 à n caractérise les pièces, n+1 a nn les pièces de traitements, nn+1 a nnn les améliorations
        //public GameConstants.CapaciteCreature Type2;
        /*
         * Chaque amélioraton nécessite au max n éléments
         * 1 = orga;
         * 2 = ...
        */
        GestionRoom m_room;

        public int necessaire0;//
        public int necessaire1;//
        public int necessaire2;//
        public int necessaire3;//
        public int necessaire4;//

        int coeff1 = 0;
        int coeff2 = 0;
        int coeff3 = 0;

        int CorrectVal = 0;

        private int ClicksCount = 0;

        Vector2 scaleRoom;
        float tempsConstructionRoom = 8.0f;

        public void OnMouseDown()
        {
            if (!son.isPlaying)
            {
                if (Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Amelioration > 0)//(Colonie.Instance.listAmelioration[(int)Convert.ToInt32(Type)] > 0)
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
                    if (VerificationList(Necessaire) == true)
                    {
                        ClicksCount++;
                        Construction();
                        if (ClicksCount == 1 && Type == GameConstants.GestionRoomType.ORGA)
                        {
                            CameraController.Instance.DisplayBubble("La flèche verte signifie que tu récoltes des ressources", TooltipBubble.TipPosition.TOP_LEFT, 270.0f, 940.0f, 500.0f);
                            CameraController.Instance.DisplayBubble("Cliques ici pour augmenter la capacité de la pièce et vitesse d'absorption", TooltipBubble.TipPosition.LEFT, InterfaceColonie.Instance.BoutonTraitement.transform.position.x + 190.0f, InterfaceColonie.Instance.BoutonTraitement.transform.position.y, 500.0f);

                        }
                    }
                }
            }

        }

        public void OnMouseEnter()
        {
            print("Je suis dedans");
        }
        public void OnMouseExit()
        {
            print("Je suis dehors");
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
                float en1 = Colonie.Instance.energieMax;
                Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].MaxCapacity = /*(Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level + 1) */(int)(2.5 * Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].MaxCapacity);
                float en2 = en1 + Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].MaxCapacity;
                Colonie.Instance.energieMax = en2;
                Colonie.Instance.energie = (int)(en2/en1 * Colonie.Instance.energie);
                print("de tout "+en2+" "+ en1+" "+ Colonie.Instance.energie);
            }
            else //Amelioration de la vitesse d'absorption des déchets
            {
                Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)-7].vitesseAbsorption *= 4;//(Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].vitesseAbsorption + 1);
            }
            tempsConstructionRoom = 8.0f;
            Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Amelioration -= 1;
            Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level += 1;
            Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].RoomLevelText.text = "Niveau " + Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level;

            Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].constructionEnCours = true;
            if(Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level == 1)
            {
                scaleRoom.x = (Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].RoomDisplay.rectTransform.localScale.x + 1) * Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level;
                scaleRoom.y = (Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].RoomDisplay.rectTransform.localScale.y + 1) * Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level;
            }
            else
            {
                scaleRoom.x = Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].RoomDisplay.rectTransform.localScale.x * Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level / (1.01f*(Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level-1));
                scaleRoom.y = Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].RoomDisplay.rectTransform.localScale.y * Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level / (1.01f*(Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level - 1));
            }
            

            StartCoroutine(TempsConstruction());
            Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].son.GetComponent<AudioSource>().PlayDelayed(1.4f);
            

            listRessource.Clear();
            listNecessaire.Clear();
        }

        IEnumerator TempsConstruction()
        {
            
            while (tempsConstructionRoom > 0)
            {
                yield return new WaitForSeconds(0.2f);
                Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].AnimationRoom(scaleRoom);
                tempsConstructionRoom -= 0.2f;
            }
            Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].constructionEnCours = false;
        }

        public bool VerificationList(List<int> nec)
        {
            int ress = 0;//Calcul du nombre de ressource nécessaire
            bool res = true;

            coeff1 = 0;
            coeff2 = 0;
            coeff3 = 0;

            if (Convert.ToInt32(Type) < GestionEvolution.Instance.nbPieceReserve)
            {
                print(Convert.ToInt32(Type) + " " + GestionEvolution.Instance.nbPieceReserve);
                //coeff2 = Colonie.Instance.listPieceReserve[Convert.ToInt32(Type)] + 1;
                coeff2 = Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level + 1;

                for (int i = 0; i < nec.Count; i++)
                {
                    ress = 0;

                    if (Convert.ToInt32(Type) < GestionEvolution.Instance.nbPieceReserve)
                    {
                        if (nec[i] != -1)
                        {
                            coeff1 = GestionEvolution.Instance.baseRessourcePieceReserve[i];
                            ress = (int)(2.5 * coeff2 * coeff1);//coeff2 * coeff1 + (Convert.ToInt32(Type) + 1) * coeff1 * coeff2 + GestionEvolution.Instance.nbRessource / (nec[i] + 1) * coeff1 * coeff2;
                            if (ress > Colonie.Instance.ListeGestionRooms[nec[i]].Resources)
                            {
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
            return res;
        }

        void VerificationAmelioration()
        {
            //var cb = this.GetComponent<Button>().colors;

            //foreach (GestionRoom room in Colonie.Instance.ListeGestionRooms)
            if (m_button != null)
            {
                m_room = Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)];
                var cb = this.GetComponent<Button>().colors;
                if (m_room.Amelioration > 0 && son.isPlaying == false)
                {
                    cb.normalColor = C10;
                    //this.transform.GetComponent<Button>().colors = cb;

                    cb.highlightedColor = C11;
                    //this.transform.GetComponent<Button>().colors = cb;

                    cb.pressedColor = C12;
                    this.transform.GetComponent<Button>().colors = cb;
                }
                else
                {
                    cb.normalColor = C20;
                    //this.transform.GetComponent<Button>().colors = cb;

                    cb.highlightedColor = C20;
                    //this.transform.GetComponent<Button>().colors = cb;

                    cb.pressedColor = C20;
                    this.transform.GetComponent<Button>().colors = cb;
                }
            }
        }
        
        // Use this for initialization
        void Start () {
            if (source != null)
            {
                son = source.GetComponent<AudioSource>();
            }
        }
	
	    // Update is called once per frame
	    void Update () {
            VerificationAmelioration();
        }
    }
}
