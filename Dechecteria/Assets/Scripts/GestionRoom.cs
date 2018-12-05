using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionRoom : MonoBehaviour {

    public GameObject spriteAssocier;

    public bool visible;

	// Use this for initialization
	void Start () {
        visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (visible == false)
        {
            this.transform.GetComponent<SpriteRenderer>().enabled = false;
            //spriteAssocier.gameObject.SetActive(false);
        }
        else
            this.transform.GetComponent<SpriteRenderer>().enabled = true;

        //spriteAssocier.gameObject.SetActive(true);

    }
}
