using System.Collections.Generic;
using UnityEngine;
using System.Collections;

//enum spriteR { SPorga, SPmineral, SPmetal}

namespace Dechecteria
{
    class Colonie : MonoBehaviour
    {
        public static Colonie Instance;

        float[,] dataColonie = new float[40, 10];
        public float time = 0.0f, tempsReserve = 0.0f;
        int baseEnergie = 500;
        public List<GestionRoom> ListeGestionRooms;

        public GameObject ColonieUI;
        public GameObject MapUI;

        public float energie; // = energie de la colonie
        public float energieMax;

        int critic = 0, maxCritic = 6;
        float timeAmelioration = 2.0f;

        /* 
         * Propriétés des colonies
        */
        
        //Ensemble des pieces constructibles
        public List<int> listPieces = new List<int>();
        //nombre de pieces de type reservoir orga(0) mineral(1)
        public List<int> listPieceReserve = new List<int>();

        //liste des pièces dédiées au recyclage des ressources
        public List<int> listPieceRecyclage = new List<int>();

        //Liste des pièces dédiées aux déchets complexes
        public List<int> listPieceSeparation = new List<int>();

        //Liste dédiée à la vitesse d'absorption des différents déchets
        public List<int> listVitesseAbsorption = new List<int>();

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
         * La vitesse d'absorbtion de chaque déchet est proportionnelle au type de déchet et 
         * à la taille de la créature
        */
        //public int Vpapier;// = vitesse absorption papier
        //public int Vplastique;// = vitesse absorption plastique
        //public int Vcomplexe;// = vitesse absorption complexe

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("Une seule instance de Colonie est requise.");
                DestroyImmediate(this.gameObject);
            }
        }

        void Start()
        {
            InitialisationReserve();
            energieMax = baseEnergie + SommeReserve();
            energie = energieMax / 2.0f; // = energie de la colonie
            StartCoroutine(TimerTick());
        }

        void Update()
        {
            vitesse = listCapaciteCreature[0] / 2.0f;
            //print(" test "+Controller.GetComponent<gestionEvolution>().nbRessource+" "+ listReserve.Count);
            TestPresence();
            ConsommeDechets();
            foreach(GestionRoom room in ListeGestionRooms)
            {
                if (!room.isRecyclageRoom)
                {
                    room.TimeBeforeGainEnergy -= Time.deltaTime;
                }
            }
            timeAmelioration -= Time.deltaTime;
            MAJListPieces();

            if (timeAmelioration < 0.0f)
            {
                for (int i = 0; i < listPieceReserve.Count - 1; i++)//-1 car pas de traitement sur les dechets complexes
                {
                    GestionAmeliorations(i);
                }
                timeAmelioration = 2.0f;
            }
            print("TAILLE DES VARIABLES " + GestionEvolution.Instance.nbAmelioration + "  " + GestionEvolution.Instance.nbPieceRecyclage);
        }
        void MAJListPieces()
        {
            for (int i = 0; i < listPieceReserve.Count; i++)
            {
                listPieces[i] = listPieceReserve[i];
            }
            for (int i = 0; i < listPieceRecyclage.Count; i++)
            {
                listPieces[i+ listPieceReserve.Count] = listPieceReserve[i];
            }
        }

        public void InitialisationReserve()
    	{
            //Initialisation des ressources max
            //MAJReserveMax();

            // INITIALISATION VOLUME RESERVE
	        foreach(GestionRoom room in ListeGestionRooms)
            {
                if (!room.isRecyclageRoom)
                {
                    room.Resources = room.MaxCapacity;
                }
            }
            // INITIALISATION PIECE RECYCLAGE
            for (int i = 0; i < GestionEvolution.Instance.nbPieceRecyclage - listPieceReserve.Count; i++)
            {
                listPieceRecyclage.Add(0);
            }
            // INITIALISATION CAPACITE CREATURE
            for (int i = 0; i < GestionEvolution.Instance.nbAmelioration - GestionEvolution.Instance.nbPieceRecyclage; i++)
	        {
	            listCapaciteCreature.Add(0);
        	}

            //Initialisation des amélioriations disponible
            for (int i = 0; i < GestionEvolution.Instance.nbAmelioration; i++)
            {
                listAmelioration.Add(5);
                listPieces.Add(0);
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

        void TestPresence()
        {
            /*for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbPieceRecyclage; i++)
            {
                if (listPieceRecyclage[i] > 0)
                {
                    listeSprites[i + listPieceReserve.Count].transform.GetComponent<GestionRoom>().visible = true;
                }
                else
                    listeSprites[i + listPieceReserve.Count].transform.GetComponent<GestionRoom>().visible = false;

            }*/
            for (int i = 0; i < listPieceReserve.Count; i++)
            {
                if (listPieceReserve[i] > 0)
                {
                    ListeGestionRooms[i].Visible = true;
                }
                else
                    ListeGestionRooms[i].Visible = false;
            }
            
            for (int i = 0; i < listPieceRecyclage.Count; i++)
            {
                if (listPieceRecyclage[i] > 0)
                {
                    ListeGestionRooms[i+listPieceReserve.Count].Visible = true;
                }
                else
                    ListeGestionRooms[i+ listPieceReserve.Count].Visible = false;

            }
        }

        void ConsommeDechets()
        {
            if (energie < energieMax)
            {
                foreach(GestionRoom room in ListeGestionRooms)
                {
                    // Pour l'instant on ignore la room de type Complex
                    if(room.Type == GameConstants.GestionRoomType.COMPLEX || room.isRecyclageRoom)
                    {
                        continue;
                    }

                    // Si la reserve + de 20%
                    if (room.Resources > room.MaxCapacity * 0.2f || (critic == maxCritic && room.Resources > 0))
                    {
                        if (room.TimeBeforeGainEnergy <= 0.0f)
                        {
                            room.TimeBeforeGainEnergy = room.IntervalGainEnergy;
                            room.Resources--;
                            energie += room.EnergyGain;
                        }
                        if (room.Resources > room.MaxCapacity * 0.2f)
                        {
                            critic = 1;
                        }
                        break;
                    }
                }

                /*
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
                else if (listReserve[3] > reserveMax[3] * 0.2f || (critic == maxCritic && listReserve[3] > 0))
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
            int result = 0;
            foreach(GestionRoom room in ListeGestionRooms)
            {
                result += room.MaxCapacity;
            }
            return result;
        }

        /*
        void MAJReserveMax()
        {
            int b = GestionEvolution.Instance.reserveMax;
            for (int i = 0; i < GestionEvolution.Instance.nbPieceReserve; i++)
            {
                reserveMax[i] = (int)((b * (listPieceReserve[i] + 1)) * (1.0 - (0.05 * i)));
            }
        }
        */

        float probMax = 10;
        float ameliorations = 0;

        void GestionAmeliorations(int index)
        {
            float prob = ListeGestionRooms[index].Resources / ListeGestionRooms[index].MaxCapacity * probMax;
            ameliorations = Random.Range(0, 101);
            if(ameliorations < prob)
            {
                if (index == 0)
                {
                    if(ameliorations > 6 * prob / 7)
                    {
                        listAmelioration[0] += 1;
                    }
                    else if(ameliorations > 5 * prob / 7)
                    {
                        listAmelioration[7] += 1; 
                    }
                    else if (ameliorations > 4 * prob / 7)
                    {
                        listAmelioration[15] += 1;
                    }
                    else if (ameliorations > 3 * prob / 7)
                    {
                        listAmelioration[16] += 1;
                    }
                    else if (ameliorations > 2 * prob / 7)
                    {
                        listAmelioration[6] += 1;
                    }
                    else if (ameliorations > prob / 7)
                    {
                        listAmelioration[13] += 1;
                    }
                   /* else
                    {
                        listAmelioration[16] += 1;
                    }*/
                }
                else if(index == 1)
                {
                    if (ameliorations > 5 * prob / 6)
                    {
                        listAmelioration[1] += 1;
                    }
                    else if (ameliorations > 4 * prob / 6)
                    {
                        listAmelioration[8] += 1;
                    }
                    else if (ameliorations > 3 * prob / 6)
                    {
                        listAmelioration[15] += 1;
                    }
                    else if (ameliorations > 2 * prob / 6)
                    {
                        listAmelioration[16] += 1;
                    }
                    else if (ameliorations > prob / 6)
                    {
                        listAmelioration[6] += 1;
                    }
                    else
                    {
                        listAmelioration[13] += 1;
                    }
                }
                else if (index == 2)
                {
                    if (ameliorations > 5 * prob / 6)
                    {
                        listAmelioration[2] += 1;
                    }
                    else if (ameliorations > 4 * prob / 6)
                    {
                        listAmelioration[9] += 1;
                    }
                    else if (ameliorations > 3 * prob / 6)
                    {
                        listAmelioration[15] += 1;
                    }
                    else if (ameliorations > 2 * prob / 6)
                    {
                        listAmelioration[16] += 1;
                    }
                    else if (ameliorations > prob / 6)
                    {
                        listAmelioration[6] += 1;
                    }
                    else
                    {
                        listAmelioration[13] += 1;
                    }
                }
                else if (index == 3)
                {
                    if (ameliorations > 5 * prob / 6)
                    {
                        listAmelioration[3] += 1;
                    }
                    else if (ameliorations > 4 * prob / 6)
                    {
                        listAmelioration[10] += 1;
                    }
                    else if (ameliorations > 3 * prob / 6)
                    {
                        listAmelioration[14] += 1;
                    }
                    else if (ameliorations > 2 * prob / 6)
                    {
                        listAmelioration[15] += 1;
                    }
                    else if (ameliorations > prob / 6)
                    {
                        listAmelioration[6] += 1;
                    }
                    else
                    {
                        listAmelioration[13] += 1;
                    }
                }
                else if (index == 4)
                {
                    if (ameliorations > 4 * prob / 5)
                    {
                        listAmelioration[4] += 1;
                    }
                    else if (ameliorations > 3 * prob / 5)
                    {
                        listAmelioration[11] += 1;
                    }
                    else if (ameliorations > 2 * prob / 5)
                    {
                        listAmelioration[15] += 1;
                    }
                    else if (ameliorations > prob / 5)
                    {
                        listAmelioration[6] += 1;
                    }
                    else
                    {
                        listAmelioration[13] += 1;
                    }
                }
                else if (index == 5)
                {
                    if (ameliorations > 6 * prob / 7)
                    {
                        listAmelioration[5] += 1;
                    }
                    else if (ameliorations > 5 * prob / 7)
                    {
                        listAmelioration[12] += 1;
                    }
                    else if (ameliorations > 4 * prob / 7)
                    {
                        listAmelioration[14] += 1;
                    }
                    else if (ameliorations > 3 * prob / 7)
                    {
                        listAmelioration[15] += 1;
                    }
                    else if (ameliorations > 2 * prob / 7)
                    {
                        listAmelioration[16] += 1;
                    }
                    else if (ameliorations > prob / 7)
                    {
                        listAmelioration[6] += 1;
                    }
                    else
                    {
                        listAmelioration[13] += 1;
                    }
                }
            }
        }
    }
}
