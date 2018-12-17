using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{
    class SousBoutonsCapacite : MonoBehaviour {

        List<int> listNecessaire = new List<int>();//Quelle type de ressources avons nous besoins
        List<int> listRessource = new List<int>();//En quelle quantité

        /*public int organique = 0;// = Stocke matiere organique
        public int mineral = 0;// = Stocke verre
        public int metal = 0;// = Stocke métaux
        public int chimique = 0;// = Stocke produit chimique
        public int petrole = 0;// = Stocke petrole
        public int nucleaire = 0;// = Stocke nucléaire*/

        public GameConstants.CapaciteCreature Type;

        public int necessaire0;//
        public int necessaire1;//
        public int necessaire2;//
        public int necessaire3;//
        public int necessaire4;//

        int coeff1 = 0;
        int coeff2 = 0;
        int coeff3 = 0;

        int CorrectVal = 0;
        int level = 0;

        public void OnMouseDown()
        {
            //if (Colonie.Instance.listeAmeliorationCapaciteCreature[Convert.ToInt32(Type)] > 0)//(Colonie.Instance.listAmelioration[(int)Convert.ToInt32(Type)] > 0)
            {

                List<int> Necessaire = new List<int>();

                if (Type == GameConstants.CapaciteCreature.SPEED)
                {
                    level = Colonie.Instance.LevelSpeed;
                    Necessaire.Add(0);
                    for (int i = 0; i < Colonie.Instance.LevelSpeed; i++)
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
                    for (int i = 0; i < Colonie.Instance.LevelAttaque; i++)
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
                    for (int i = 0; i < Colonie.Instance.LevelDefense; i++)
                    {
                        if (Colonie.Instance.LevelDefense <= 4)
                        {
                            Necessaire.Add(i + 1);
                        }
                    }
                }

                if (VerificationList(Necessaire) == true)
                {
                    print("Construction possible : \n");
                    Construction();

                }
                //Colonie.Instance.listeAmeliorationCapaciteCreature[Convert.ToInt32(Type)] -= 1;
            }
        }

        public void Construction()
        {
            for (int i = 0; i < listNecessaire.Count; i++)
            {
                Colonie.Instance.ListeGestionRooms[i].Resources -= listRessource[i];
            }

            //if (Convert.ToInt32(Type) < GestionEvolution.Instance.nbCapacite)
            {
                print("JE SUIS L EVOLUTION D UNE AMELIORATION");
                Colonie.Instance.listCapaciteCreature[Convert.ToInt32(Type)] += 1;

                if (Type == GameConstants.CapaciteCreature.SPEED)
                {
                    Colonie.Instance.LevelSpeed += 1;
                }
                else if (Type == GameConstants.CapaciteCreature.ATK)
                {
                    Colonie.Instance.LevelAttaque += 1;
                }
                else if (Type == GameConstants.CapaciteCreature.DEF)
                {
                    Colonie.Instance.LevelDefense += 1;
                }
            }

            listRessource.Clear();
            listNecessaire.Clear();
        }

        public bool VerificationList(List<int> nec)
        {
            int ress = 0;//Calcul du nombre de ressource nécessaire
            bool res = true;

            coeff1 = 0;
            coeff2 = 0;
            coeff3 = 0;

            if (Convert.ToInt32(Type) < Convert.ToInt32(GameConstants.CapaciteCreature.COUNT)/*GestionEvolution.Instance.nbCapacite*/)
            {
                for (int i = 0; i < nec.Count; i++)
                {
                    ress = 0;

                    coeff1 = GestionEvolution.Instance.baseRessourceAmelioration[i];
                    ress = (level + 1) * coeff1;

                    if (ress > Colonie.Instance.ListeGestionRooms[i].Resources)
                    {
                        res = false;
                    }
                    else
                    {
                        listNecessaire.Add(i);
                        listRessource.Add(ress);
                    }
                    /*CorrectVal = GestionEvolution.Instance.nbPieceRecyclage;
                    coeff3 = Colonie.Instance.listCapaciteCreature[Convert.ToInt32(Type)] + 1;

                    res = true;

                    for (int j = 0; j < coeff3; j++)
                    {
                        coeff1 = GestionEvolution.Instance.baseRessourceAmelioration[j];
                        ress = coeff1 * coeff3;

                        if (ress > Colonie.Instance.ListeGestionRooms[j].Resources)
                        {
                            res = false;
                        }
                        else
                        {
                            listNecessaire.Add(j);
                            listRessource.Add(ress);
                        }
                    }*/
                }
                //return res;
            }
            return res;
        }
        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
