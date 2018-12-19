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
        Color Vert;
        Color Yellow;
        float initialValue;
        float rng;

        // Use this for initialization
        void Start()
        {
            Vert = new Color(0f/255f,188f/255f,56/255f);
            Yellow = Color.yellow;
            initialValue = matiereOrganique;
            rng = 0.5f * Random.Range(0f,0.5f);
        }

        // Update is called once per frame
        void Update()
        {
            float l = 1f - matiereOrganique / initialValue ;
            gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Vert,Yellow,l);
            if (matiereOrganique < initialValue*1.1f && matiereOrganique > 0)
            {
                matiereOrganique += GainPopulationPerSecond * coeffOrganique * rng;
                mineral += coeffMineral;
            }
        }
    }
}

