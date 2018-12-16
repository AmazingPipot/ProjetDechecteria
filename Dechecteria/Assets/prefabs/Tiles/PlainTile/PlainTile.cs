using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Plaine
    Moyen organique/mineral
*/

namespace Dechecteria
{
    class PlainTile : Tile
    {
        [Space(10)]
        public float coeffOrganique;
        public float coeffMineral;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            matiereOrganique += gainPerTick * coeffOrganique;
            mineral += coeffMineral;
        }
    }
}
