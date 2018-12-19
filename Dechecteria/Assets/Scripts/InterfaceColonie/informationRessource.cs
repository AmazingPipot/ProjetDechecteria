using Dechecteria;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dececteria
{
    class informationRessource : MonoBehaviour {

        public Text Typetxt;
        public Text txt;
        public GameConstants.GestionRoomType Type;
        // Use this for initialization
        void Start()
        {
            Typetxt.text = "";
        }

        // Update is called once per frame
        void Update() {
            Debug.Log(Typetxt.transform.position);
            Typetxt.text = txt.text.ToString()+" : "+Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].Resources.ToString() + " / " + Colonie.Instance.ListeGestionRooms[Convert.ToInt32(Type)].MaxCapacity.ToString();
        }
    }
}
