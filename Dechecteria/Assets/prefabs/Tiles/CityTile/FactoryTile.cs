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
        private FMODUnity.StudioEventEmitter emitter;

        private float TimeElapsed;

        // Use this for initialization
        public override void Start()
        {
            base.Start();
            childCount = transform.childCount;
            destroyed = false;
            TimeElapsed = 0.0f;
        }

        // Update is called once per frame
        void Update()
        {

            TimeElapsed += Time.deltaTime;
            if (TimeElapsed >= 1.0f)
            {
                if (population > 0)
                {
                    population += GainPopulationPerSecond;
                    metal += GainPopulationPerSecond * coeffMetal;
                    mineral += GainPopulationPerSecond * coeffMineral;
                    petrole += GainPopulationPerSecond * coeffPetrol;
                    chimique += GainPopulationPerSecond * coeffChimique;
                    complexe += GainPopulationPerSecond * coeffComplexe;
                    plastique += GainPopulationPerSecond * coeffPlastique;
                    papier += GainPopulationPerSecond * coeffPapier;
                }
                TimeElapsed = 0.0f;
            }

            if (!destroyed && population <= 0.0f)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                destroyed = true;
                changeParam(2);
            }
        }

        void changeParam(int i)
        {
            emitter.SetParameter("Intensity", i);
        }

    }
}