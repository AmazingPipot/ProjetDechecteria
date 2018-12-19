using UnityEngine;

namespace Dechecteria
{
    public class InterfaceColonie : MonoBehaviour
    {
        public static InterfaceColonie Instance;

        public SousMenuColonie BoutonPoches;
        public SousMenuColonie BoutonAmelioration;
        public SousMenuColonie BoutonTraitement;

        int t1;
        int t2;
        int t3;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
        }

    }
}

