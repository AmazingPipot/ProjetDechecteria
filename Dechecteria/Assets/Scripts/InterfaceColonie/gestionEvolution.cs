using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestionEvolution : MonoBehaviour {

    public List<int> RessourcePiece = new List<int>();
    public int T;

    public int nbPieceReserve = 7;//variable indiquant le nombre de piece de type de reserve
    public int nbRessource = 7;
    public void MAJameliorationPiece()
    {
        //ressourceOrga = nvPieceOrga;
    } 
	// Use this for initialization
	void Start () {
        T = 0;
        RessourcePiece.Add(200);//Base d'orga pour le nv1
        //RessourcePiece.Add(150);
    }
	
	// Update is called once per frame
	void Update () {
        T++;
        //print(" T : " + T);
	}
}
