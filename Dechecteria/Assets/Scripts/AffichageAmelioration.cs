using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{
    class AffichageAmelioration : MonoBehaviour
    {
        //public Transform amelioration;
        //Type
        public Image imageIcone;
        public GestionRoom room;
        float y = 0;
        int tic = 0;
        //Vect;
        /*private List<int> listA;
        List<int> listControl = new List<int>();
        public GestionRoom Spr;
        public GameObject button;
        public GameObject canvas;*/
        public void OnMouseDown()
        {
            if (room != null)
            {
                room.Amelioration += 1;
                room.AmeliorationDisp -= 1;
                Destroy(this.gameObject);
            }
        }
        void AffichageButton()
        {
            tic++;

            if (tic <= 15)
            {
                y = 1;
            }
            else if (tic < 30)
            {
                y = -1;
            }
            else
            {
                tic = 0;
            }
            this.gameObject.transform.Translate(0, y, 0);//transform.GetComponent<RectTransform>().position;
        }

	    void Start ()
        {

        }
	
	    void Update ()
        {
            AffichageButton();
	    }
    }
}
