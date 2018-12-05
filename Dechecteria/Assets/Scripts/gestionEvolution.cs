using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestionEvolution : MonoBehaviour {

    public List<int> baseRessourcePiece = new List<int>();
    public List<int> baseRessourceAmelioration = new List<int>();

    
    public int T;

    public int nbPieceReserve = 7;//variable indiquant le nombre de piece de type de reserve (0-6)
    public int nbRessource = 7;//nombre de ressources traitables;
    public int reserveMax = 2000; //base quantite max de skockage
    public int nbAmelioration = 10;//Nombre de parametres améliorables (7-9)
    //public List<int> necessaireAmelioration = new List<int>;


    public void MAJameliorationPiece()
    {
        //ressourceOrga = nvPieceOrga;
    } 
	// Use this for initialization
	void Start () {
        T = 0;
        baseRessourcePiece.Add(100);//Base d'orga pour le nv1
        baseRessourcePiece.Add(90);
        baseRessourcePiece.Add(80);
        baseRessourcePiece.Add(70);
        baseRessourcePiece.Add(60);
        baseRessourcePiece.Add(50);
        baseRessourcePiece.Add(40);

        baseRessourceAmelioration.Add(10);
        baseRessourceAmelioration.Add(10);
        baseRessourceAmelioration.Add(10);
        baseRessourceAmelioration.Add(10);
        baseRessourceAmelioration.Add(10);
        baseRessourceAmelioration.Add(10);
        baseRessourceAmelioration.Add(10);
    }
	
	// Update is called once per frame
	void Update () {
        T++;
        //print(" T : " + T);
	}
}
