using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{
    class AffichageCapaciteExplicatif : MonoBehaviour
    {

        List<string> nom = new List<string>();
        List<int> listNecessaire = new List<int>();

        public Text txtExplicatif;
        public Text txtAffichage;
        public GameConstants.CapaciteCreature Type;
        public Button bt;

        int coeff2, coeff1;
        int ress;
        int necessaire0, necessaire1, necessaire2, necessaire3, necessaire4;

        string result;
        int level;

        void CalculCout()
        {
            List<int> Necessaire = new List<int>();
            print("Taille " + Necessaire.Count);
            if (Type == GameConstants.CapaciteCreature.SPEED)
            {
                level = Colonie.Instance.LevelSpeed;
                Necessaire.Add(0);
                for (int i = 0; i <= level; i++)
                {
                    if (Colonie.Instance.LevelSpeed <= 4)
                    {
                        Necessaire.Add(i + 1);
                    }
                }
            }
            else if (Type == GameConstants.CapaciteCreature.ATK)
            {
                level = Colonie.Instance.LevelAttaque;
                Necessaire.Add(0);
                for (int i = 0; i <= level; i++)
                {
                    if (Colonie.Instance.LevelAttaque <= 4)
                    {
                        Necessaire.Add(i + 1);
                    }
                }
            }
            else if (Type == GameConstants.CapaciteCreature.DEF)
            {
                level = Colonie.Instance.LevelDefense;
                Necessaire.Add(0);
                for (int i = 0; i <= level; i++)
                {
                    if (Colonie.Instance.LevelDefense <= 4)
                    {
                        Necessaire.Add(i + 1);
                    }
                }
            }
            result = "";
            for (int i = 0; i < Necessaire.Count; i++)
            {
                ress = 0;

                coeff1 = GestionEvolution.Instance.baseRessourceAmelioration[i];

                print("Ress "+level+" "+coeff1+" "+ress+" "+Necessaire[i]);
                ress = (level + 1) * coeff1;
                print("JE SAIS pas ou je plante");
                result += " " + nom[Necessaire[i]].ToString() + " : " + ress.ToString();
                Necessaire.Clear();

            }
        }
        // Use this for initialization
        void Start()
        {
            txtAffichage.text = "";
            necessaire0 = bt.GetComponent<SousBoutonsCapacite>().necessaire0;
            necessaire1 = bt.GetComponent<SousBoutonsCapacite>().necessaire1;
            necessaire2 = bt.GetComponent<SousBoutonsCapacite>().necessaire2;
            necessaire3 = bt.GetComponent<SousBoutonsCapacite>().necessaire3;
            necessaire4 = bt.GetComponent<SousBoutonsCapacite>().necessaire4;

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
            txtAffichage.text = txtExplicatif.text.ToString() + "\n\n" + "Coût évolution : \n" + result;


        }
    }
}
