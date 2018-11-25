using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceColonie : MonoBehaviour {

    public Button bt1;
    public Button bt2;
    public Button bt3;

    int taille = -40;
    int taille1 = -80;
    int taille2 = -80;
    int taille3 = -80;

    float Xorigin = 0;
    float Yorigin = 0;

    float XX = 0;
    public void gestionColonie(Button bt1, Button bt2, Button bt3)
    {
        XX = bt1.transform.GetComponent<RectTransform>().anchoredPosition.x;

        if (bt1.GetComponent<SousMenuColonie>().clic == 1 || bt2.GetComponent<SousMenuColonie>().clic == 1 || bt3.GetComponent<SousMenuColonie>().clic == 1)
        {
            if (bt1.GetComponent<SousMenuColonie>().clic == 1)
            {
                bt2.transform.GetComponent<SousMenuColonie>().clic = 0;
                bt3.transform.GetComponent<SousMenuColonie>().clic = 0;

                bt1.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin);
                bt2.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, bt1.transform.GetComponent<RectTransform>().anchoredPosition.y+taille1);
                bt3.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, bt2.transform.GetComponent<RectTransform>().anchoredPosition.y+taille);
            }
            else if (bt2.GetComponent<SousMenuColonie>().clic == 1)
            {
                bt1.transform.GetComponent<SousMenuColonie>().clic = 0;
                bt3.transform.GetComponent<SousMenuColonie>().clic = 0;

                bt1.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin);
                bt2.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin + taille);
                bt3.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, bt2.transform.GetComponent<RectTransform>().anchoredPosition.y + taille2);
            }
            else
            {
                bt1.transform.GetComponent<SousMenuColonie>().clic = 0;
                bt2.transform.GetComponent<SousMenuColonie>().clic = 0;

                bt1.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin);
                bt2.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin + taille);
                bt3.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, bt2.transform.GetComponent<RectTransform>().anchoredPosition.y+taille);
            }
        }
        else
        {
            placementBoutons();
        }
    }

    public void placementBoutons()
    {
        bt1.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin);
        bt2.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin + taille);
        bt3.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin + 2*taille);
    }
    void Start()
    {
        placementBoutons();
        //gestionColonie(bt1, bt2, bt3);
    }

    void Update()
    {
        gestionColonie(bt1, bt2, bt3);
    }
}
