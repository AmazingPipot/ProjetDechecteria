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

        public override void Start()
        {
            base.Start();
        }

        void Update()
        {
            if (population > 0)
            {
                nucleaire += GainPopulationPerSecond;
                metal += GainPopulationPerSecond * coeffMetal;
                mineral += GainPopulationPerSecond * coeffMineral;
                complexe += GainPopulationPerSecond * coeffComplexe;
            }
            else if (!destroyed && population <= 0.0f)
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
