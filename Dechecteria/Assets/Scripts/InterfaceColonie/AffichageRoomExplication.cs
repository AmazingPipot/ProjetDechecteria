using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{
    class AffichageRoomExplication : MonoBehaviour
    {
        List<string> nom = new List<string>();
        List<int> listNecessaire = new List<int>();

        public Text txtExplicatif;
        public Text txtAffichage;
        public GameConstants.GestionRoomType Type;
        public Button bt;

        int coeff2, coeff1;
        int ress;
        int necessaire0, necessaire1, necessaire2, necessaire3, necessaire4;

        string result;

        void CalculCout()
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
            
            print(Convert.ToInt32(Type) + " " + GestionEvolution.Instance.nbPieceReserve);
            coeff2 = Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Level + 1;
            result = "";
            for (int i = 0; i < Necessaire.Count; i++)
            {
                ress = 0;

                if (Necessaire[i] != -1)
                {
                    coeff1 = GestionEvolution.Instance.baseRessourcePieceReserve[i];
                    if (Convert.ToInt32(Type) < GestionEvolution.Instance.nbPieceReserve)
                    {
                        
                        ress = (int)(2.5 * coeff2 * coeff1);//coeff2 * coeff1 + (Convert.ToInt32(Type) + 1) * coeff1 * coeff2 + GestionEvolution.Instance.nbRessource / (Necessaire[i] + 1) * coeff1 * coeff2;
                    }
                    else
                    {
                        ress = (int)(2.75 * coeff2 * coeff1);
                    }
                    result += " "+nom[Necessaire[i]].ToString() + " : " + ress.ToString();
                }
                    
            }
        }
        // Use this for initialization
        void Start()
        {
            txtAffichage.text = "";
            necessaire0 = bt.GetComponent<SousBoutons>().necessaire0;
            necessaire1 = bt.GetComponent<SousBoutons>().necessaire1;
            necessaire2 = bt.GetComponent<SousBoutons>().necessaire2;
            necessaire3 = bt.GetComponent<SousBoutons>().necessaire3;
            necessaire4 = bt.GetComponent<SousBoutons>().necessaire4;

            nom.Add("Organique");
            nom.Add("Minéral");
            nom.Add("Métal");
            nom.Add("Pétrole");
            nom.Add("Chimiqie");
            nom.Add("Nucléaire");
        }

        // Update is called once per frame
        void Update()
        {
            CalculCout();
            txtAffichage.text = txtExplicatif.text.ToString() + "\n\n" + "Coût évolution : \n"+result;


        }
    }
}
