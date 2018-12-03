using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestionEvolution : MonoBehaviour {

    public List<int> RessourcePiece = new List<int>();
    public int T;

    public void initDico()
    {

    }
    public void MAJameliorationPiece()
    {
        //ressourceOrga = nvPieceOrga;
    } 
	// Use this for initialization
	void Start () {
        T = 0;
        RessourcePiece.Add(200);
        RessourcePiece.Add(150);
    }
	
	// Update is called once per frame
	void Update () {
        T++;
        print(" T : " + T);
	}
}
