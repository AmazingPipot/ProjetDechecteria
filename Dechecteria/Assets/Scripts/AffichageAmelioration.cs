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

        RectTransform pos;
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

            
       

        }

	    void Start ()
        {
           pos = this.gameObject.GetComponent<RectTransform>();
        }
	
	    void Update ()
        {
            AffichageButton();
	    }
    }
}
