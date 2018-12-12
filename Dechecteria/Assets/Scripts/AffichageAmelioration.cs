using System.Collections.Generic;
using UnityEngine;

namespace Dechecteria
{
    class AffichageAmelioration : MonoBehaviour
    {
        //public Transform amelioration;
        private List<int> listA;
        List<int> listControl = new List<int>();
        public GestionRoom Spr;
        public GameObject button;
        public GameObject canvas;

        void AffichageButton()
        {
            listA = Colonie.Instance.listAmelioration;
            for (int i = 0; i < listA.Count; i++)
            {
                if (listA[i] > 0 && (i < Colonie.Instance.listPieces.Count && Colonie.Instance.listPieces[i] > 0))
                {
                    if (listControl[i] == 0)
                    {
                        Spr = Colonie.Instance.ListeGestionRooms[i];
                        /*if (i < 7)
                        {
                            Spr = Colonie.GetComponent<Colonie>().listeSprites[i];
                        }
                        else if (i > 9)
                        {
                            Spr = Colonie.GetComponent<Colonie>().listeSprites[i - 3];
                        }*/
                        listControl[i] = 1;
                        GameObject bt = Instantiate(button) as GameObject;
                        bt.transform.SetParent(canvas.transform, false);
                            //Transform bt = Instantiate(amelioration);
                        Vector3 pspr = Spr.GetComponent<RectTransform>().position.normalized;
                        print("POSITION VECTOR "+pspr);
                        bt.transform.GetComponent<RectTransform>().position = new /*Vector3(115, -20, 0);/*/Vector3(pspr.x, pspr.y, pspr.z);
                    }
                }
                else
                {
                    listControl[i] = 0;
                }
            }
       

        }

	    void Start ()
        {
            for (int i = 0; i < 17/*Colonie.GetComponent<Colonie>().listAmelioration.Count*/; i++)
            {
                listControl.Add(0);
            }
	    }
	
	    void Update ()
        {
            AffichageButton();
	    }
    }
}
