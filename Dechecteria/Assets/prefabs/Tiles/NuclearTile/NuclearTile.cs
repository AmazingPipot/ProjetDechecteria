using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Centrale nucleaire
    Beaucoup nucleaire/complexe
    Moyen metal/mineral
    Faible humain
    + Attaque + PV
*/
namespace Dechecteria
{
    class NuclearTile : Tile
    {
        public float coeffHuman;
        public float coeffComplexe;
        public float coeffMetal;
        public float coeffMineral;
        bool destroyed;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(population > 0)
            {
                population += gainPerTick * coeffHuman;
                nucleaire += gainPerTick;
                metal += gainPerTick * coeffMetal;
                mineral += gainPerTick * coeffMineral;
                complexe += gainPerTick * coeffComplexe;
            }else if (!destroyed && population == 0)
            {
                destroyed = true;
                for(int i=0; i < transform.GetChild(0).gameObject.transform.childCount; i++)
                {
                    transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            
        }
    }
}
