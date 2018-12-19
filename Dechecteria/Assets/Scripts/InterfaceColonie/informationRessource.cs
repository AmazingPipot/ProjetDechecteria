using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{
    class informationRessource : MonoBehaviour
    {

        public Text Typetxt;
        public Text txt;
        public GameConstants.GestionRoomType Type;

        void Start()
        {
            Typetxt.text = "";
        }

        void Update()
        {
            Typetxt.text = txt.text.ToString()+" : "+Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Resources.ToString() + " / " + Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].MaxCapacity.ToString();
        }
    }
}
