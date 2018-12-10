using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nouvellePartie : MonoBehaviour {
    public GameObject NewPartie;
    public GameObject ControllerMenu;

    public void LancementPartie()
    {
        NewPartie.SetActive(true);
        ControllerMenu.SetActive(false);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
