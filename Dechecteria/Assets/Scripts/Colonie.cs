using System.Collections.Generic;
using UnityEngine;
using System.Collections;

//enum spriteR { SPorga, SPmineral, SPmetal}

namespace Dechecteria {
    public class Colonie : MonoBehaviour {

        public GameObject Controller;

        float[,] dataColonie = new float[40, 10];
        public float time = 0.0f;
        int baseEnergie = 500;
        public List<GestionRoom> listeSprites;
        public List<int> reserveMax;
        public float energieMax;

        /* 
         * Propriétés des colonies
        */
        public float energie; // = energie de la colonie
        
        //valeur des quantites organique (0) et mineral (1) metal(2)... nucleaire(6) 
        public List<int> listReserve = new List<int>();

        //nombre de pieces de type reservoir orga(0) mineral(1)
        public List<int> listPieceReserve = new List<int>();
        //attack, defence, vitesse
    	public List<int> listCapaciteCreature = new List<int>();

        public float vitesse;// = Vitesse creature

        /*
         * Quantité de déchets stockés en fonction des types
        */
        //public int Rpapier;// = Stocke de papier
        //public int Rplastique;// = Stocke de plastique
        //public int Rcomplexe;// = Stocke de dechet complexe (voiture, ....)

        /*
         * Les déchets composés sont tous stockés dans une unique pièce avant leur décomposition
         * en déchets élémentaires dans les pièces dédiées
        */
        public ComplexRoom Pcomplexe;// = piece de stockage dechet complexe, papier, plastique, ...

        public int PseparationComplexe;//piece de transformation en déchets complexes (voiture --> metal+plastique+petrole+matiere organique)
        public int PseparationPapier;//Pièce transformation papier --> produit chimique, matière organique
        public int PseparationPlastique;//Pièce separation plastique --> petrole, produit chimique, matiere organique

        /*
         * Pièces permettant de faire évoluer la créature en fonction des déchets utilisés 
        */
        public int PrecyclageOrganique;
        public int PrecyclageMineral;
        public int PrecyclageMetal;
        public int PrecyclageChimique;
        public int PrecyclagePetrole;
        public int PrecyclageNucleaire;

        /*
         * La vitesse d'absorbtion de chaque déchet est proportionnelle au type de déchet et 
         * à la taille de la créature
        */
        //public int Vpapier;// = vitesse absorption papier
        //public int Vplastique;// = vitesse absorption plastique
        //public int Vcomplexe;// = vitesse absorption complexe


        // Use this for initialization
        void Start() {
            initialisationReserve();
            energieMax = baseEnergie + SommeReserve();
            energie = energieMax / 2.0f; // = energie de la colonie
            StartCoroutine(TimerTick());
        }

        // Update is called once per frame
        void Update() {
            vitesse = listCapaciteCreature[2] / 2.0f;
            testPresence();
            //ConsommeDechets();
        }

        public void initialisationReserve()
    	{
            int b = Controller.GetComponent<gestionEvolution>().reserveMax;

            for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbPieceReserve; i++)
	        {
	            listPieceReserve.Add(0);
                reserveMax.Add((int)(b * (1.0 - (0.05 * i))));
	        }

	        for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbRessource; i++)
	        {
	            listReserve.Add(1000);
	        }

	        for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbAmelioration-7; i++)
	        {
	            listCapaciteCreature.Add(0);
        	}
    	}

        // Par défaut, à toutes les secondes, energie decremente de 1
        IEnumerator TimerTick()
        {
            while (time >= 0)
            {
                //attendre 1 seconde
                yield return new WaitForSeconds(1.0f);
                time++;
                energie--;
                Debug.Log("Time " + time.ToString() + " energie " + energie);
            }
        }

        void testPresence()
        {
            for(int i = 0; i < listPieceReserve.Count; i++)
            {
                if(listPieceReserve[i] > 0)
                {
                    listeSprites[i].transform.GetComponent<GestionRoom>().visible = true;
                }
                else
                    listeSprites[i].transform.GetComponent<GestionRoom>().visible = false;
            }
        }

        void ConsommeDechets()
        {
            if (energie < energieMax)
            {
                if (listReserve[0] > reserveMax[0] * 20.0f / 100.0f)//si la reserve organique + de 20%
                {
                    listReserve[0]--;
                    energie++;
                    //Debug.Log("Reserve orga " + listReserve[0] + " et energie " + energie);
                }
                else
                {
                    if (listReserve[1] > reserveMax[1] * 20.0f / 100.0f)
                    {
                        listReserve[1]--;
                        energie++;
                    }
                    else
                    {
                        if (listReserve[2] > reserveMax[2] * 20.0f / 100.0f)
                        {
                            //yield return new WaitForSeconds(3.0f);
                            listReserve[2]--;
                            energie += 2;
                            Debug.Log("Attente energie " + energie);
                        }
                        else
                        {
                            if (listReserve[3] > reserveMax[3] * 20.0f / 100.0f)
                            {
                                listReserve[3]--;
                                energie += 4;
                            }
                            else
                            {
                                if (listReserve[4] > reserveMax[4] * 20.0f / 100.0f)
                                {
                                    listReserve[4]--;
                                    energie += 5;
                                }
                                else
                                {
                                    if (listReserve[5] > reserveMax[5] * 20.0f / 100.0f)
                                    {
                                        listReserve[5]--;
                                        energie += 7;
                                    }
                                    else
                                    {
                                        /*if (listReserve[6] > reserveMax[6] * 20.0f / 100.0f)
                                        {
                                            listReserve[6]--;
                                            energie += 4;
                                        }*/

                                        ConsommeFinal();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void ConsommeFinal()
        {
            if (listReserve[0] != 0)//si la reserve organique n'a pas encore atteint 0
            {
                listReserve[0]--;
                energie++;
            }
            else
            {
                if (listReserve[1] != 0)
                {
                    listReserve[1]--;
                    energie++;
                }
                else
                {
                    if (listReserve[2] != 0)
                    {
                        listReserve[2]--;
                        energie += 2;
                    }
                    else
                    {
                        if (listReserve[3] != 0)
                        {
                            listReserve[3]--;
                            energie += 4;
                        }
                        else
                        {
                            if (listReserve[4] != 0)
                            {
                                listReserve[4]--;
                                energie += 5;
                            }
                            else
                            {
                                if (listReserve[5] != 0)
                                {
                                    listReserve[5]--;
                                    energie += 7;
                                }
                                else
                                {
                                    /*if (listReserve[6] > reserveMax[6] * 20.0f / 100.0f)
                                    {
                                        listReserve[6]--;
                                        energie += 4;
                                    }*/

                                    Debug.Log("Plus de ressources. Créature meurt");
                                }
                            }
                        }
                    }
                }
            }
        }

        int SommeReserve()
        {
            int somme = 0;
            for(int i = 0; i < reserveMax.Count; i++)
            {
                somme += reserveMax[i];
            }

            return somme;
        }

    }
}
