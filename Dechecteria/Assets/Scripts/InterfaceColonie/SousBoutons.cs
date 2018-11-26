using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SousBoutons : MonoBehaviour {

    public GameObject Colonie;
    public GameObject Controller;
    List<int> listNecessaire = new List<int>();
    List<int> listRessource = new List<int>();

    public void OnMouseDown()
    {
        print("JE SUIS CLIQUE");
        if (verificationList() == true)
        {
            print("construction possible");
        }
    }

    public bool verificationList()
    {
        bool r = true;
        for (int i = 0; i < listNecessaire.Count; i++)
        {
            if (listRessource[i] > Colonie.GetComponent<Colonie>().listReserve[listNecessaire[i]])
            {
                r = false;
            }
        }
        return r;
    }
    //public List<Button> listButton = new List<Button>();
    //public Button bt;
    /*public Button bt1;
    public Button bt2;
    public Button bt3;
    public Button bt4;*/



    /*float lx = 20;
    float ly = 40;

    void disparitionSousBoutons()
    {
        if (BT.GetComponent<SousMenuColonie>().clic == 0)
        {
            
        }
    }
    void placementSousButton()
    {
        if (BT.GetComponent<SousMenuColonie>().clic != 0)
        {
            Xorigin = BT.transform.GetComponent<RectTransform>().anchoredPosition.x;
            Yorigin = BT.transform.GetComponent<RectTransform>().anchoredPosition.y;

            bt1.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin + lx, Yorigin + ly);
            bt2.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin + lx, Yorigin + 2 * ly);
            bt3.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin + lx, Yorigin + 3 * ly);
            bt4.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin + lx, Yorigin + 4 * ly);
        }
    }*/
    // Use this for initialization
    void Start () {
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000, 1000);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
