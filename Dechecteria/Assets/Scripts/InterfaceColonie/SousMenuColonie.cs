using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SousMenuColonie : MonoBehaviour {
    public GameObject Gestion;

    public Button bt;

    public int clic;
    public float nTaille;
    public int T = 0;

    float xParent;
    float yParent;

    public List<Button> listButton = new List<Button>();

    // Use this for initialization
    public void OnMouseDown()
    {
        clic += 1;
        clic = clic % 2;

        T = Gestion.GetComponent<gestionEvolution>().T;
        print("T "+this.name+ " " + T);
        //print(clic);
    }

    /*public int GetClic()
    {
        return clic;
    }*/

    public void gestionList()
    {
        nTaille = -40 * listButton.Count;

        xParent = bt.GetComponent<RectTransform>().anchoredPosition.x;
        yParent = bt.GetComponent<RectTransform>().anchoredPosition.y;

        //print("Bouton "+bt.name+" position "+ yParent);

        if (clic == 1)
        {
            for (int i = 0; i < listButton.Count; i++)
            {
                listButton[i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(xParent + 50, yParent - (i+1) * 40);
                //print(" Y " + listButton[i].GetComponent<RectTransform>().anchoredPosition.y);
            }
        }
        else
        {
            for (int i = 0; i < listButton.Count; i++)
            {
                listButton[i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000, 1000);
            }
            listButton.Clear();
        }
    }

    public void ajoutBouton(Button b)
    {
        if (clic == 1)
        {
            //print("BOUTON AJOUTE");
            this.listButton.Add(b);
        }
    }

    void Start () {
        clic = 0;
	}


	// Update is called once per frame
	void Update () {
        gestionList();
    }
}
