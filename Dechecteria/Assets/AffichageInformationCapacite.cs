using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Dechecteria
{
    class AffichageInformationCapacite : MonoBehaviour
    {

        public Text Typetxt;
        //public Text txt;
        public GameConstants.CapaciteCreature Type;

        // Use this for initialization
        void Start()
        {
            Typetxt.text = "";
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(Typetxt.transform.position);
            Typetxt.text = Colonie.Instance.listCapaciteCreature[Convert.ToInt32(Type)].ToString();
        }
    }
}
