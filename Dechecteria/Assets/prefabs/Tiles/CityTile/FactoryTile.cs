using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    Zone industrielle 
    Moyen humain
    Moyen mineral/metal
    Beaucoup petrol/chimique/complexe
    + attaque + PV
*/
namespace Dechecteria
{
    class FactoryTile : Tile
    {
        int childCount;
        [Space(10)]
        public float coeffMetal;
        public float coeffMineral;
        public float coeffPetrol;
        public float coeffChimique;
        public float coeffComplexe;
        public float coeffPlastique;
        public float coeffPapier;
        bool destroyed;

        // Use this for initialization
        void Start()
        {
            
            childCount = transform.childCount;
            destroyed = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(population > 0)
            {
                population += gainPerTick;
                metal += gainPerTick * coeffMetal;
                mineral += gainPerTick * coeffMineral;
                petrole += gainPerTick * coeffPetrol;
                chimique += gainPerTick * coeffChimique;
                complexe += gainPerTick * coeffComplexe;
                plastique += gainPerTick * coeffPlastique;
                papier += gainPerTick * coeffPapier;
            } else if (!destroyed && population == 0)
            {
                for(int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}