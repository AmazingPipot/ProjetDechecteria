using System.Collections.Generic;
using UnityEngine;
using System.Collections;

//enum spriteR { SPorga, SPmineral, SPmetal}

namespace Dechecteria {
    public class Colonie : MonoBehaviour {

        public GameObject Controller;

        float[,] dataColonie = new float[40, 10];
        public float time = 0.0f, tempsReserve = 0.0f;
        int baseEnergie = 500;
        public List<GestionRoom> listeSprites;
        public List<int> reserveMax;

        public float energie; // = energie de la colonie
        public float energieMax;

        int critic = 0, maxCritic = 6;
        float timeBeforeGainEnergy; // = 2.0f;

        /* 
         * Propriétés des colonies
        */
        
        
        //valeur des quantites organique (0) et mineral (1) metal(2)... nucleaire(6) 
        public List<int> listReserve/* = new List<int>()*/;

        //nombre de pieces de type reservoir orga(0) mineral(1)
        public List<int> listPieceReserve = new List<int>();

        //liste des pièces dédiées au recyclage des ressources
        public List<int> listPieceRecyclage = new List<int>();

        //Liste des pièces dédiées aux déchets complexes
        public List<int> listPieceSeparation = new List<int>();

        //Liste dédiée à la vitesse d'absorption des différents déchets
        public List<int> listVitesseAbsorption = new List<int>();

        //attack, defence, vitesse
        public List<int> listCapaciteCreature = new List<int>();//0 vitesse, 1 attaque, 2 defense

        public List<int> listAmelioration = new List<int>();

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
        public int PrecyclagePetrole;
        public int PrecyclageChimique;
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
            //print(" test "+Controller.GetComponent<gestionEvolution>().nbRessource+" "+ listReserve.Count);
            testPresence();
            ConsommeDechets();
            timeBeforeGainEnergy -= Time.deltaTime;
            MAJReserveMax();
        }

        public void initialisationReserve()
    	{
            int b = Controller.GetComponent<gestionEvolution>().reserveMax;
            // INITIALISATION RESSOURCE
            for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbPieceReserve; i++)
	        {
	            listPieceReserve.Add(0);
                reserveMax.Add((int)(b * (1.0 - (0.05 * i))));
	        }

            //Initialisation des ressources max
            //MAJReserveMax();

            // INITIALISATION VOLUME RESERVE
            for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbRessource; i++)
	        {
                print("ajout fait");
	            listReserve.Add(10000);
	        }
            // INITIALISATION PIECE RECYCLAGE
            for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbPieceRecyclage; i++)
            {
                listPieceRecyclage.Add(0);
            }
            // INITIALISATION CAPACITE CREATURE
            for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbAmelioration-7; i++)
	        {
	            listCapaciteCreature.Add(0);
        	}

            //Initialisation des amélioriations disponible
            for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbPieceComplexe; i++)
            {
                listAmelioration.Add(5);
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
                if (listReserve[0] > reserveMax[0] * 20.0f / 100.0f || (critic == maxCritic && listReserve[0] > 0))//si la reserve organique + de 20%
                {
                    if (timeBeforeGainEnergy <= 0.0f)
                    {
                        // gain energy
                        timeBeforeGainEnergy = 0.5f;
                        listReserve[0]--;
                        energie++;
                        Debug.Log("Reserve orga " + listReserve[0] + " et energie " + energie);
                    }
                    if (listReserve[0] > reserveMax[0] * 20.0f / 100.0f)
                    {
                        critic = 1;
                    }
                }
                else if (listReserve[1] > reserveMax[1] * 20.0f / 100.0f || (critic == maxCritic && listReserve[1] > 0))
                {
                    if (timeBeforeGainEnergy <= 0.0f)
                    {
                        // gain energy
                        timeBeforeGainEnergy = 1.0f;
                        listReserve[1]--;
                        energie += 2;
                        Debug.Log("Reserve mineral " + listReserve[1] + " et energie " + energie);
                    }
                    if (listReserve[1] > reserveMax[1] * 20.0f / 100.0f)
                    {
                        critic = 1;
                    }
                }
                else if (listReserve[2] > reserveMax[2] * 20.0f / 100.0f || (critic == maxCritic && listReserve[2] > 0))
                {
                    if (timeBeforeGainEnergy <= 0.0f)
                    {
                        timeBeforeGainEnergy = 2.0f;
                        listReserve[2]--;
                        energie += 4;
                    }
                    if (listReserve[2] > reserveMax[2] * 20.0f / 100.0f)
                    {
                        critic = 1;
                    }
                }
                else if (listReserve[3] > reserveMax[3] * 20.0f / 100.0f || (critic == maxCritic && listReserve[3] > 0))
                {
                    if (timeBeforeGainEnergy <= 0.0f)
                    {
                        timeBeforeGainEnergy = 4.0f;
                        listReserve[3]--;
                        energie += 8;
                    }
                    if (listReserve[3] > reserveMax[3] * 20.0f / 100.0f)
                    {
                        critic = 1;
                    }
                }
                else if (listReserve[4] > reserveMax[4] * 20.0f / 100.0f || (critic == maxCritic && listReserve[4] > 0))
                {
                    if (timeBeforeGainEnergy <= 0.0f)
                    {
                        timeBeforeGainEnergy = 8.0f;
                        listReserve[4]--;
                        energie += 16;
                    }
                    if (listReserve[4] > reserveMax[4] * 20.0f / 100.0f)
                    {
                        critic = 1;
                    }
                }
                else if (listReserve[5] > reserveMax[5] * 20.0f / 100.0f || (critic == maxCritic && listReserve[5] > 0))
                {
                    if (timeBeforeGainEnergy <= 0.0f)
                    {
                        timeBeforeGainEnergy = 16.0f;
                        listReserve[5]--;
                        energie += 32;
                    }
                    if (listReserve[5] > reserveMax[5] * 20.0f / 100.0f)
                    {
                        critic = 1;
                    }
                }
                else
                    critic = maxCritic;
                /*else
                {
                    /*if (listReserve[6] > reserveMax[6] * 20.0f / 100.0f)
                    {
                        listReserve[6]--;
                        energie += 4;
                    }

                    ConsommeFinal();
                }*/
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

        void MAJReserveMax()
        {
            int b = Controller.GetComponent<gestionEvolution>().reserveMax;
            for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbPieceReserve; i++)
            {
                reserveMax[i] = (int)((b * (listPieceReserve[i] + 1)) * (1.0 - (0.05 * i)));
            }
        }

    }

}
