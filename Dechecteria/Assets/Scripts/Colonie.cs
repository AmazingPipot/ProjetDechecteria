using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Dechecteria {
    public class Colonie : MonoBehaviour {

        float[,] dataColonie = new float[40, 10];
        public GameObject Controller;
        public float time = 0.0f;

        /* 
         * Propriétés des colonies
        */
        public float energie = GameConstants.MAX_ENERGY / 2.0f; // = energie de la colonie
        
        //valeur des quantites organique (0) et mineral (1) metal(2)... nucleaire(6) 
        public List<int> listReserve = new List<int>();

        //nombre de pieces de type reservoir orga(0) mineral(1)
        public List<int> listPieceReserve = new List<int>();
        //attack, defence, vitesse
    	public List<int> listCapaciteCreature = new List<int>();

        public float vitesse = 0.3f;// = Vitesse colonie
        public int attaque = 0;// = Attaque colonie
        public int defense = 0;// = Defense colonie

        /*
         * Quantité de déchets stockés en fonction des types
        */
        //public int Rpapier;// = Stocke de papier
        //public int Rplastique;// = Stocke de plastique
        //public int Rcomplexe;// = Stocke de dechet complexe (voiture, ....)

        /*
         * Pièce de stockage des déchets élémentaires
        */
        public OrganiqueRoom Porganique;// = piece stockage matiere organique
        public MineralRoom Pmineral;// = piece stockage verre
        public MetalRoom Pmetal;// = piece stockage métal
        public ChimiqueRoom Pchimique;// = piece stockage produit chimique
        public PetrolRoom Ppetrole;// = piece stockage petrole
        public NuclearRoom Pnucleaire;// = piece stockage nucleaire

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
            energie = GameConstants.MAX_ENERGY / 2.0f; // = energie de la colonie
            StartCoroutine(TimerTick());
            initialisationReserve();
        }

        // Update is called once per frame
        void Update() {
            //ConsommeDechets();
        }

        public void initialisationReserve()
    	{
	        for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbPieceReserve; i++)
	        {
	            listPieceReserve.Add(0);
	        }

	        for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbRessource; i++)
	        {
	            listReserve.Add(10000);
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

        /*void ConsommeDechets()
        {
            //if (energie <= (energie * 25.0f) / 100.0f) //energie de la creature inferieur ou egale à 25 %
            if (energie <= 125)
            {
                Debug.Log("Energie " + energie + " a 25%");
            }
        }*/
    }
}