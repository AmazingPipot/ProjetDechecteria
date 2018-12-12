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
        if (bt1.GetComponent<SousMenuColonie>().clic == 1 || bt2.GetComponent<SousMenuColonie>().clic == 1 || bt3.GetComponent<SousMenuColonie>().clic == 1)
        {
            if (bt1.GetComponent<SousMenuColonie>().clic == 1 && t1 > t2 && t1 > t3)
            {
                bt2.transform.GetComponent<SousMenuColonie>().clic = 0;
                bt3.transform.GetComponent<SousMenuColonie>().clic = 0;
            }
            else if (bt2.GetComponent<SousMenuColonie>().clic == 1 && t2 > t1 && t2 > t3)
            {
                bt1.transform.GetComponent<SousMenuColonie>().clic = 0;             
                bt3.transform.GetComponent<SousMenuColonie>().clic = 0;
            }
            else if (bt3.GetComponent<SousMenuColonie>().clic == 1 && t3 > t1 && t3 > t2)
            {
                bt1.transform.GetComponent<SousMenuColonie>().clic = 0;
                bt2.transform.GetComponent<SousMenuColonie>().clic = 0;
            }
        }

        if (Gestion.GetComponent<gestionEvolution>().T > 1000)
        {
            bt1.transform.GetComponent<SousMenuColonie>().T = 0;
            bt2.transform.GetComponent<SousMenuColonie>().T = 0;
            bt3.transform.GetComponent<SousMenuColonie>().T = 0;
            Gestion.transform.GetComponent<gestionEvolution>().T = 0;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        t1 = bt1.GetComponent<SousMenuColonie>().T;
        t2 = bt2.GetComponent<SousMenuColonie>().T;
        t3 = bt3.GetComponent<SousMenuColonie>().T;

        gestionColonie();
    }
}
