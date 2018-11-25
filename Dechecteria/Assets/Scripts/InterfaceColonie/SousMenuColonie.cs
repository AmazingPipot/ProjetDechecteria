using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SousMenuColonie : MonoBehaviour {
    //public Button bt;
    public int clic;

    // Use this for initialization
    public void OnMouseDown()
    {
        clic += 1;
        clic = clic % 2;
        print(clic);
    }

    public int GetClic()
    {
        return clic;
    }

    void Start () {
        clic = 0;
	}


	// Update is called once per frame
	void Update () {

    }
}
