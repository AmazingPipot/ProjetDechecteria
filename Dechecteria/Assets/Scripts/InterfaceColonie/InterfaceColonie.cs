using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceColonie : MonoBehaviour {
    public GameObject Gestion;

    public Button bt1;
    public Button bt2;
    public Button bt3;

    float taille = -40;
    float taille1;// = -80;
    float taille2;// = -80;
    float taille3;// = -80;

    int t1;
    int t2;
    int t3;

    public float Xorigin = 0;
    public float Yorigin = 50;

    public void gestionColonie(/*Button bt1, Button bt2, Button bt3*/)
    {
        //XX = bt1.transform.GetComponent<RectTransform>().anchoredPosition.x;

        if (bt1.GetComponent<SousMenuColonie>().clic == 1 || bt2.GetComponent<SousMenuColonie>().clic == 1 || bt3.GetComponent<SousMenuColonie>().clic == 1)
        {
            if (bt1.GetComponent<SousMenuColonie>().clic == 1 && t1 > t2 && t1 > t3)
            {
                bt2.transform.GetComponent<SousMenuColonie>().clic = 0;
                //bt2.transform.GetComponent<SousMenuColonie>().listButton.Clear();

                bt3.transform.GetComponent<SousMenuColonie>().clic = 0;
                //bt3.transform.GetComponent<SousMenuColonie>().listButton.Clear();

                bt1.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin);
                bt2.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin+taille+taille1);
                bt3.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin+2*taille+taille1);
            }
            else if (bt2.GetComponent<SousMenuColonie>().clic == 1 && t2 > t1 && t2 > t3)
            {
                bt1.transform.GetComponent<SousMenuColonie>().clic = 0;
                //bt1.transform.GetComponent<SousMenuColonie>().listButton.Clear();

                bt3.transform.GetComponent<SousMenuColonie>().clic = 0;
                //bt3.transform.GetComponent<SousMenuColonie>().listButton.Clear();

                //bt2.transform.GetComponent<SousMenuColonie>().xParent = Xorigin + 50;
                //bt2.transform.GetComponent<SousMenuColonie>().yParent = Yorigin + taille;

                bt1.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin);
                bt2.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin + taille);
                bt3.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin + 2 * taille + taille2);
            }
            else if (bt3.GetComponent<SousMenuColonie>().clic == 1 && t3 > t1 && t3 > t2)
            {
                bt1.transform.GetComponent<SousMenuColonie>().clic = 0;
                //bt1.transform.GetComponent<SousMenuColonie>().listButton.Clear();

                bt2.transform.GetComponent<SousMenuColonie>().clic = 0;
                //bt2.transform.GetComponent<SousMenuColonie>().listButton.Clear();

                //bt3.transform.GetComponent<SousMenuColonie>().xParent = Xorigin + 50;
                //bt3.transform.GetComponent<SousMenuColonie>().yParent = Yorigin + 2 * taille;

                bt1.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin);
                bt2.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin + taille);
                bt3.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin + 2 * taille);
            }
            /*bt1.transform.GetComponent<SousMenuColonie>().T = 0;
            bt1.transform.GetComponent<SousMenuColonie>().T = 0;
            bt1.transform.GetComponent<SousMenuColonie>().T = 0;
            Gestion.transform.GetComponent<gestionEvolution>().T = 0;*/
            /*print("Y1 " + bt1.GetComponent<RectTransform>().anchoredPosition.y);
            print("Y2 " + bt2.GetComponent<RectTransform>().anchoredPosition.y);
            print("Y3 " + bt3.GetComponent<RectTransform>().anchoredPosition.y);*/
        }
        else
        {
            placementBoutons();
        }

        if (Gestion.GetComponent<gestionEvolution>().T > 1000)
        {
            bt1.transform.GetComponent<SousMenuColonie>().T = 0;
            bt2.transform.GetComponent<SousMenuColonie>().T = 0;
            bt3.transform.GetComponent<SousMenuColonie>().T = 0;
            Gestion.transform.GetComponent<gestionEvolution>().T = 0;
        }
    }

    public void placementBoutons()
    {
        bt1.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin);
        bt1.transform.GetComponent<SousMenuColonie>().listButton.Clear();

        bt2.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin + taille);
        bt2.transform.GetComponent<SousMenuColonie>().listButton.Clear();

        bt3.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xorigin, Yorigin + 2 * taille);
        bt3.transform.GetComponent<SousMenuColonie>().listButton.Clear();
    }
    void Start()
    {
        taille1 = bt1.GetComponent<SousMenuColonie>().nTaille;
        taille2 = bt2.GetComponent<SousMenuColonie>().nTaille;
        taille3 = bt3.GetComponent<SousMenuColonie>().nTaille;
        //print(taille1 + " " + taille2 + " " + taille3);
        placementBoutons();
        //gestionColonie(/*bt1, bt2, bt3*/);
    }

    void Update()
    {
        taille1 = bt1.GetComponent<SousMenuColonie>().nTaille;
        taille2 = bt2.GetComponent<SousMenuColonie>().nTaille;
        taille3 = bt3.GetComponent<SousMenuColonie>().nTaille;

        t1 = bt1.GetComponent<SousMenuColonie>().T;
        t2 = bt2.GetComponent<SousMenuColonie>().T;
        t3 = bt3.GetComponent<SousMenuColonie>().T;

        bt2.GetComponent<SousMenuColonie>().gestionList();
        print("Les temps : t1 "+t1 + " t2 " + t2 + " t3 " + t3);
        //print(bt1.GetComponent<RectTransform>().anchoredPosition.x+ " "+ bt1.GetComponent<RectTransform>().anchoredPosition.y);
        //print(bt1.GetComponent<SousMenuColonie>().clic);
        gestionColonie();
    }
}
