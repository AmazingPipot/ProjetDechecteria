using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Les villes
    Beaucoup Organique exclusivement humains
    Beaucoup metal/mineral
    Moyen petrole
    Faible chimique
    Beaucoup complexe
    papier
    pastique
    + Attaque et PV
*/

namespace Dechecteria
{
    class CityTile : Tile
    {
        float maxHeight = -0.20f;
        float minHeight = 0.76f;
        GameObject ownTile;
        GameObject height;
        [Tooltip("When Max Height is achieved")]
        public float maxPopulation;
        [Space(10)]
        public float coeffOrganique;
        public float coeffMetal;
        public float coeffMineral;
        public float coeffPetrol;
        public float coeffChimique;
        public float coeffComplexe;
        public float coeffPlastique;
        public float coeffPapier;

        private float TimeElapsed;
        private bool destroyed;
        private FMODUnity.StudioEventEmitter emitter;
        private GameObject creature;

        int damage;
        //Map map;

        // Use this for initialization
        public override void Start()
        {
            base.Start();
            damage = 0;
            ownTile = transform.root.gameObject;
            height = transform.GetChild(0).gameObject;
            TimeElapsed = 0.0f;
            destroyed = false;
            emitter = GetComponent<FMODUnity.StudioEventEmitter>();
            creature = GameObject.Find("Creature");
            //map = GetComponent("Map").GetComponent<Map>();
        }

        // Update is called once per frame
        void Update()
        {
            if (population > 0)
            {
                TimeElapsed += Time.deltaTime;
                if (TimeElapsed >= 1.0f)
                {
                    population += GainPopulationPerSecond;
                    float value = (population / maxPopulation);
                    matiereOrganique += value * coeffOrganique;
                    metal += value * coeffMetal;
                    mineral += value * coeffMineral;
                    petrole += value * coeffPetrol;
                    chimique += value * coeffChimique;
                    complexe += value * coeffComplexe;
                    plastique += value * coeffPlastique;
                    papier += value * coeffPapier;
                    TimeElapsed = 0.0f;
                }
                
            }
            /*
            if (population > maxPopulation)
            {
                int x = (int)transform.position.x,
                    y = (int)transform.position.y;
                if (x > 0 && compare(x,y))
                {
                    map.ChangeTile(x, y, GameConstants.TILE_TYPE.CITY);
                }

            }
            */
            height.transform.localPosition = new Vector3(0f, 0f, HowHigh());
            if(!destroyed) {
                if (population < 1)
                {
                    destroyed = true;
                    changeParam(2);
                }
                else if (!destroyed && Vector3.Distance(creature.transform.position,transform.position)<1.5f)
                {
                    changeParam(1);
                }
                else
                { changeParam(0); }

            }
        }

        float HowHigh()
        {
            float l = 0.0f;
            if (population > maxPopulation)
                l = 1.0f;
            else
                l = population / maxPopulation;
            //print("height: "+Mathf.Lerp(minHeight, maxHeight, l)+ " l : " + l);
            return Mathf.Lerp(minHeight, maxHeight, l);
        }
        /*
        bool compare(int x, int y)
        {
            GameConstants.TILE_TYPE test = map.creation_tiles[(int)x - 1, (int)y];
            return ( test == GameConstants.TILE_TYPE.PLAIN
                || test == GameConstants.TILE_TYPE.FOREST);

        }
        */

        void changeParam(int i)
        {
            emitter.SetParameter("Intensity", i);
        }
    }

}
