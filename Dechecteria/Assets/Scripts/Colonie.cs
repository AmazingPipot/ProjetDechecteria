using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

//enum spriteR { SPorga, SPmineral, SPmetal}
namespace Dechecteria
{
    class Colonie : MonoBehaviour
    {
        public static Colonie Instance;

        float[,] dataColonie = new float[40, 10];
        public float time = 0.0f, tempsReserve = 0.0f;
        public List<GestionRoom> ListeGestionRooms;

        public GameObject ColonieUI;
        public GameObject MapUI;

        public int baseEnergie;
        public float energie; // = energie de la colonie
        public float energieMax;

        int SommeReserve = 0;
        int critic = 0, maxCritic = 6;

        float timeAmelioration = 2.0f;
        public bool textEnCours = false;
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
        public List<int> listAmeliorationCreature = new List<int>();

        //public List<int> listCapaciteCreature = new List<int>();

        public int LevelSpeed;
        public int LevelAttaque;
        public int LevelDefense;

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
            energieMax = baseEnergie;// + SommeReserve();
            //energie = energieMax / 2.0f; // = energie de la colonie
            StartCoroutine(TimerTick());
        }

        void Update()
        {
            energie = Mathf.Clamp(energie, 0.0f, energieMax);
            vitesse = listCapaciteCreature[Convert.ToInt32(GameConstants.CapaciteCreature.SPEED)] / 2.0f;
            //print(" test "+Controller.GetComponent<gestionEvolution>().nbRessource+" "+ listReserve.Count);
            TestPresence();
            ConsommeDechets();

            SommeReserve = 0;
            maxCritic = 0;
            foreach (GestionRoom room in ListeGestionRooms)
            {

                if (!room.isRecyclageRoom)
                {
                    room.TimeBeforeGainEnergy -= Time.deltaTime;
                    if (room.Level > 0)
                    {
                        SommeReserve += room.MaxCapacity;//Verification de la taille max des reserves
                        maxCritic++;
                    }

                }
            }
            //Mise a jour de l'energie max ! proportionelle à la capacité des rooms reserves
            //energieMax = baseEnergie + SommeReserve;

            timeAmelioration -= Time.deltaTime;

            if (timeAmelioration < 0.0f)
            {
                foreach(GestionRoom room in ListeGestionRooms)
                {
                    if (room.isRecyclageRoom == false)
                    {
                        GestionAmeliorations(Convert.ToInt32(room.Type));
                    }
                }
                timeAmelioration = 2.0f;
            }

        }

        public void InitialisationReserve()
    	{
            //Initialisation des ressources max
            //MAJReserveMax();

            // INITIALISATION VOLUME RESERVE
	        foreach(GestionRoom room in ListeGestionRooms)
            {
                if (!room.isRecyclageRoom && (room.Level > 0 || room.Type == 0))
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
            for (int i = 0; i < Convert.ToInt32(GameConstants.CapaciteCreature.COUNT); i++)
	        {
                listAmeliorationCreature.Add(1);
            }

            //Initialisation des amélioriations disponible
            /*for (int i = 0; i < Convert.ToInt32(GameConstants.CapaciteCreature.COUNT); i++)
            {
                if (i == 0)
                {
                    listCapaciteCreature.Add(1);
                }
                else
                {
                    listCapaciteCreature.Add(0);
                }
                listCapaciteCreature.Add(1);
                //listPieces.Add(1);
            }*/
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
            }
        }

        void TestPresence()
        {
            foreach(GestionRoom room in ListeGestionRooms)
            {
                if (room.Level > 0)
                {
                    room.Visible = true;
                }
            }
        }

        void ConsommeDechets()
        {
            if (energie < energieMax)
            {
                foreach (GestionRoom room in ListeGestionRooms)
                {
                    // Pour l'instant on ignore la room de type Complex
                    if (room.Type == GameConstants.GestionRoomType.COMPLEX || room.isRecyclageRoom)
                    {
                        continue;
                    }

                    // Si la reserve + de 20%
                    if (/*room.Level > 0 &&*/ (room.Resources > room.MaxCapacity * 0.2f || (critic == maxCritic && room.Resources > 0)))
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
            }
        }

        /*int SommeReserve()
        {
            int result = 0;
            foreach(GestionRoom room in ListeGestionRooms)
            {
                result += room.MaxCapacity;
            }
            return result;
        }*/

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

        /*
         * Fonction determinant si des evolutions sont disponibles
         * 
        */
        float probMax = 10;
        float ameliorations = 0;

        void GestionAmeliorations(int index)
        {
            float prob = ListeGestionRooms[index].Resources / ListeGestionRooms[index].MaxCapacity * probMax;
            ameliorations = Random.Range(0, 101);
            if(ameliorations < prob)
            {
                print("Test si amelioration dipo 1");
                if (index == 0)
                {
                    print("Test si amelioration dipo 2");
                    if (ameliorations > 3 * prob / 6 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.ORGA)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.ORGA)].AmeliorationDisp += 1;
                        //listCapaciteCreature[0] += 1;
                    }
                    else if (ameliorations > 2 * prob / 6 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_ORGA)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_ORGA)].AmeliorationDisp += 1;
                        //listCapaciteCreature[7] += 1; 
                    }
                    else if (ameliorations > 1.5 * prob / 6 && listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.ATK)] == 0)
                    {
                        listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.ATK)] += 1;
                    }
                    else if (ameliorations > 1 * prob / 6 && listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.DEF)] == 0)
                    {
                        listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.DEF)] += 1;
                    }
                    else if (ameliorations > 0.5 * prob / 6 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.COMPLEX)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.COMPLEX)].AmeliorationDisp += 1;
                        //listCapaciteCreature[6] += 1;
                    }
                    else if (ameliorations > 0 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_COMPLEX)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_COMPLEX)].AmeliorationDisp += 1;
                        //listCapaciteCreature[13] += 1;
                    }
                   /* else
                    {
                        listCapaciteCreature[16] += 1;
                    }*/
                }
                else if(index == 1)
                {
                    print("Test si amelioration dipo 2");
                    if (ameliorations > 3 * prob / 6 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.MINERAL)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.MINERAL)].AmeliorationDisp += 1;
                    }
                    else if (ameliorations > 2 * prob / 6 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_MINERAL)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_MINERAL)].AmeliorationDisp += 1;
                    }
                    else if (ameliorations > 1.5 * prob / 6 && listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.ATK)] == 0)
                    {
                        listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.ATK)] += 1;
                    }
                    else if (ameliorations > 1 * prob / 6 && listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.DEF)] == 0)
                    {
                        listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.DEF)] += 1;
                    }
                    else if (ameliorations > 0.5 * prob / 6 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.COMPLEX)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.COMPLEX)].AmeliorationDisp += 1;
                    }
                    else if (ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_COMPLEX)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_COMPLEX)].AmeliorationDisp += 1;
                    }
                }
                else if (index == 2)
                {
                    print("Test si amelioration dipo 2");
                    if (ameliorations > 4 * prob / 6 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.METAL)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.METAL)].AmeliorationDisp += 1;
                    }
                    else if (ameliorations > 3 * prob / 6 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_METAL)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_METAL)].AmeliorationDisp += 1;
                    }
                    else if (ameliorations > 2 * prob / 6 && listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.ATK)] == 0)
                    {
                        listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.ATK)] += 1;
                    }
                    else if (ameliorations > 1 * prob / 6 && listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.DEF)] == 0)
                    {
                        listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.DEF)] += 1;
                    }
                    else if (ameliorations > 0.5 * prob / 6 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.COMPLEX)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.COMPLEX)].AmeliorationDisp += 1;
                    }
                    else if (ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_COMPLEX)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_COMPLEX)].AmeliorationDisp += 1;
                    }
                }
                else if (index == 3)
                {
                    print("Test si amelioration dipo 2");
                    if (ameliorations > 4 * prob / 6 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.PETROL)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.PETROL)].AmeliorationDisp += 1;
                    }
                    else if (ameliorations > 3 * prob / 6 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_PETROL)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_PETROL)].AmeliorationDisp += 1;
                    }
                    else if (ameliorations > 2 * prob / 6 && listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.SPEED)] == 0)
                    {
                        listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.SPEED)] += 1;
                    }
                    else if (ameliorations > 1 * prob / 6 && listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.ATK)] == 0)
                    {
                        listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.ATK)] += 1;
                    }
                    else if (ameliorations > 0.5 * prob / 6 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.COMPLEX)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.COMPLEX)].AmeliorationDisp += 1;
                    }
                    else if (ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_COMPLEX)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_COMPLEX)].AmeliorationDisp += 1;
                    }
                }
                else if (index == 4)
                {
                    print("Test si amelioration dipo 2");
                    if (ameliorations > 4 * prob / 5 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.CHIMIC)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.CHIMIC)].AmeliorationDisp += 1;
                    }
                    else if (ameliorations > 3 * prob / 5 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_CHIMIC)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_CHIMIC)].AmeliorationDisp += 1;
                    }
                    else if (ameliorations > 2 * prob / 5 && listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.ATK)] == 0)
                    {
                        listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.ATK)] += 1;
                    }
                    else if (ameliorations > prob / 5 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.COMPLEX)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.COMPLEX)].AmeliorationDisp += 1;
                    }
                    else if (ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_COMPLEX)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_COMPLEX)].AmeliorationDisp += 1;
                    }
                }
                else if (index == 5)
                {
                    print("Test si amelioration dipo 2");
                    if (ameliorations > 6 * prob / 7 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.NUCLEAR)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.NUCLEAR)].AmeliorationDisp += 1;
                    }
                    else if (ameliorations > 5 * prob / 7 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_NUCLEAR)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_NUCLEAR)].AmeliorationDisp += 1;
                    }
                    else if (ameliorations > 4 * prob / 7 && listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.SPEED)] == 0)
                    {
                        listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.SPEED)] += 1;
                    }
                    else if (ameliorations > 3 * prob / 7 && listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.ATK)] == 0)
                    {
                        listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.ATK)] += 1;
                    }
                    else if (ameliorations > 2 * prob / 7 && listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.DEF)] == 0)
                    {
                        listAmeliorationCreature[Convert.ToInt32(GameConstants.CapaciteCreature.DEF)] += 1;
                    }
                    else if (ameliorations > prob / 7 && ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.COMPLEX)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.COMPLEX)].AmeliorationDisp += 1;
                    }
                    else if (ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_COMPLEX)].Amelioration == 0)
                    {
                        ListeGestionRooms[Convert.ToInt32(GameConstants.GestionRoomType.RECYCLAGE_COMPLEX)].AmeliorationDisp += 1;
                    }
                }
            }
        }
    }
}
