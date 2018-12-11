using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestionEvolution : MonoBehaviour {

    public List<int> baseRessourcePieceReserve = new List<int>();
    public List<int> baseRessourcePieceRecyclage = new List<int>();
    public List<int> baseRessourceAmelioration = new List<int>();

    
    public int T;

    public int nbPieceReserve = 7;//variable indiquant le nombre de piece de type de reserve (0-6)
    public int nbRessource = 6;//nombre de ressources traitables;
    public int reserveMax = 2000; //base quantite max de skockage
    public int nbAmelioration = 10;//Nombre de parametres améliorables (7-9)
    public int nbPieceRecyclage = 16;//Nombre de piece dédiées au recyclage, de (10 à 15)
    public int nbPieceComplexe = 17;//Nombre de piece dédiées au recyclage, de (16 à )
    //public List<int> necessaireAmelioration = new List<int>;


    public void MAJameliorationPiece()
    {
        //ressourceOrga = nvPieceOrga;
    } 
	// Use this for initialization
	void Start () {
        T = 0;
        /*
         * Base de ressource pour construire une pièce
         * 0 base orga
         * 1 base minéral
         * 2 base métal
         * 3 base pétrole
         * 4 base chimique
         * 5 base nucléaire
        */
        int B = 100;
        for (int i = 0; i < nbRessource; i++)
        {
            baseRessourcePieceReserve.Add((int)(B * (1.0 - (0.05 * i))));
        }
        /*
        baseRessourcePieceReserve.Add(100);
        baseRessourcePieceReserve.Add(90);
        baseRessourcePieceReserve.Add(80);
        baseRessourcePieceReserve.Add(70);
        baseRessourcePieceReserve.Add(60);
        baseRessourcePieceReserve.Add(50);
        baseRessourcePieceReserve.Add(40);
        */

        /*
         *  Ressource nécessaire pour la construction des pièces de transformations
         * 
        */
        B = 200;
        for (int i = 0; i < nbPieceRecyclage - nbAmelioration; i++)
        {
            if (i == 0)
            {
                baseRessourcePieceRecyclage.Add(B);
            }
            else
            {
                baseRessourcePieceRecyclage.Add(baseRessourcePieceRecyclage[i-1]-10);
            } 
        }
        /*
         * Ressource de base nécessaire pour effectuer les amélioation
        */
        B = 300;
        for (int i = 0; i < nbRessource-1; i++)
        {
            baseRessourceAmelioration.Add((int)(B * (1.0-(0.05*i))));
        }
        /*baseRessourceAmelioration.Add(10);
        baseRessourceAmelioration.Add(10);
        baseRessourceAmelioration.Add(10);
        baseRessourceAmelioration.Add(10);
        baseRessourceAmelioration.Add(10);
        baseRessourceAmelioration.Add(10);*/
        //baseRessourceAmelioration.Add(10);
    }
	
	// Update is called once per frame
	void Update () {
        T++;
        //print(" T : " + T);
	}
}
