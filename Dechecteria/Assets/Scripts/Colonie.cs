using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colonie : MonoBehaviour {
    public GameObject Controller;

    float[,] dataColonie = new float[40, 10];
    
    /* 
     * Propriétés des colonies
    */
    public int energieMax; // = energie max de la colonie
    public int energie;// = energie de la colonie

    public float vitesse;// = Vitesse colonie
    public int attaque;// = Attaque colonie
    public int defense;// = Defense colonie

    /*
     * Quantité de déchets stockés en fonction des types
    */
    public int Rorganique;// = Stocke matiere organique
    public int Rmineral;// = Stocke verre
    public int Rmetal;// = Stocke métaux
    public int Rchimique;// = Stocke produit chimique
    public int Rpetrole;// = Stocke petrole
    public int Rnucleaire;// = Stocke nucléaire

    public int Rpapier;// = Stocke de papier
    public int Rplastique;// = Stocke de plastique
    public int Rcomplexe;// = Stocke de dechet complexe (voiture, ....)

    public List<int> listReserve = new List<int>();

    /*
     * Pièce de stockage des déchets élémentaires
    */
    public int Porganique;// = piece stockage matiere organique
    public int Pmineral;// = piece stockage verre
    public int Pmetal;// = piece stockage métal
    public int Pchimique;// = piece stockage produit chimique
    public int Ppetrole;// = piece stockage petrole
    public int Pnucleaire;// = piece stockage nucleaire

    /*
     * Les déchets composés sont tous stockés dans une unique pièce avant leur décomposition
     * en déchets élémentaires dans les pièces dédiées
    */
    public int Pcomplexe;// = piece de stockage dechet complexe, papier, plastique, ...

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
    public int Vorganique;// = vitesse absorption matiere organique
    public int Vmineral;// = vitesse absorption mineraux
    public int Vmetal;// = vitesse absorption metaux
    public int Vchimique;// = vitesse absorption produit chimique
    public int Vpetrole;// = vitesse absorption petrole
    public int Vnucleaire;// = vitesse absorption nucleaire

    public int Vpapier;// = vitesse absorption papier
    public int Vplastique;// = vitesse absorption plastique
    public int Vcomplexe;// = vitesse absorption complexe

    public List<int> listPieceReserve = new List<int>();

    public void initialisationReserve()
    {
        for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbPieceReserve; i++)
        {
            listPieceReserve.Add(0);
        }

        for (int i = 0; i < Controller.GetComponent<gestionEvolution>().nbRessource; i++)
        {
            listReserve.Add(1000);
        }
    }
    // Use this for initialization
    void Start () {
        initialisationReserve();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
