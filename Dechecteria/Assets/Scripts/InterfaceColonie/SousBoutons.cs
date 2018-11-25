using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SousBoutons : MonoBehaviour {

    //public List<Button> listButton = new List<Button>();
    public Button BT;
    public Button bt1;
    public Button bt2;
    public Button bt3;
    public Button bt4;

    float Xorigin = 0;
    float Yorigin = 0;

    float lx = 20;
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
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
