using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Colonie : MonoBehaviour {

    float[,] dataColonie = new float[40, 10];
    public float time = 0.0f;

    /* 
     * Propriétés des colonies
    */
    public static int energieMax = 1000; // = energie max de la colonie
    public int energie = energieMax / 2;// = energie de la colonie

    public float vitesse = 0.3f;// = Vitesse colonie
    public int attaque = 0;// = Attaque colonie
    public int defense = 0;// = Defense colonie

    /*
     * Quantité de déchets stockés en fonction des types
    */
    //public int Rorganique;// = Stocke matiere organique
    public int Rmineral;// = Stocke verre
    public int Rmetal;// = Stocke métaux
    public int Rchimique;// = Stocke produit chimique
    public int Rpetrole;// = Stocke petrole
    public int Rnucleaire;// = Stocke nucléaire

    public int Rpapier;// = Stocke de papier
    public int Rplastique;// = Stocke de plastique
    public int Rcomplexe;// = Stocke de dechet complexe (voiture, ....)

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
    public int Vorganique;// = vitesse absorption matiere organique
    public int Vmineral;// = vitesse absorption mineraux
    public int Vmetal;// = vitesse absorption metaux
    public int Vchimique;// = vitesse absorption produit chimique
    public int Vpetrole;// = vitesse absorption petrole
    public int Vnucleaire;// = vitesse absorption nucleaire

    public int Vpapier;// = vitesse absorption papier
    public int Vplastique;// = vitesse absorption plastique
    public int Vcomplexe;// = vitesse absorption complexe


    // Use this for initialization
    void Start () {
        StartCoroutine(TimerTick());
    }
	
	// Update is called once per frame
	void Update () {
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
            Debug.Log("Time " + time.ToString() + " energie " + energie); ;
        }
    }

    void RecupereDechets()
    {
        // Si on est sur une Tile contenant dechets organiques

    }
}
