using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{
    class SousBoutonsCapacite : MonoBehaviour {
        public GameObject Source;
        AudioSource son;
        List<int> listNecessaire = new List<int>();//Quelle type de ressources avons nous besoins
        List<int> listRessource = new List<int>();//En quelle quantité

        Color C10 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color C11 = new Color(0.95f, 0.95f, 0.95f, 1.0f);
        Color C12 = new Color(0.65f, 0.65f, 0.65f, 1.0f);

        Color C20 = new Color(0.5f, 0.5f, 0.5f, 1.0f);

        /*public int organique = 0;// = Stocke matiere organique
        public int mineral = 0;// = Stocke verre
        public int metal = 0;// = Stocke métaux
        public int chimique = 0;// = Stocke produit chimique
        public int petrole = 0;// = Stocke petrole
        public int nucleaire = 0;// = Stocke nucléaire*/
        public Button m_button;
        int m_capacite;

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
            if (!son.isPlaying)//(Colonie.Instance.listAmelioration[(int)Convert.ToInt32(Type)] > 0)
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
                Colonie.Instance.listAmeliorationCreature[Convert.ToInt32(Type)] -= 1;

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
                m_capacite = Colonie.Instance.listAmeliorationCreature[Convert.ToInt32(Type)];
                var cb = this.GetComponent<Button>().colors;
                
                if (m_capacite > 0 && !son.isPlaying)
                {
                    cb.normalColor = C10;
                    cb.highlightedColor = C11;
                    cb.pressedColor = C12;

                    this.transform.GetComponent<Button>().colors = cb;
                }
                else
                {
                    cb.normalColor = C20;
                    cb.highlightedColor = C20;
                    cb.pressedColor = C20;
                    this.transform.GetComponent<Button>().colors = cb;
                }
            }
        }
            // Use this for initialization
        void Start() {
            son = Source.GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update() {
            son = Source.GetComponent<AudioSource>();
            VerificationAmelioration();
        }
    }
}
