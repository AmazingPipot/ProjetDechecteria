using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{
    class GestionBackground : MonoBehaviour
    {
        public Colonie Instance;
        public Image img;
        public float tailleMaxImage;
        public float factor;
        float energieD = 0;
        float energieInt = 0;
        float energieF = 0;
        int T = 0;

        void GestionTailleSprite()
        {
            energieD = Colonie.Instance.energieMax;

            if (energieD != energieF)
            {
                T += 5;
                energieF = energieD;
            }

            if (T > 0)
            {
                tailleMaxImage -=  0.05f * factor;
                img.transform.GetComponent<RectTransform>().localScale = new Vector3(1, tailleMaxImage, 1);
            }
            else
            {
                
            }
        }
        // Use this for initialization
        void Start() {
            energieD = Colonie.Instance.energieMax / 50000;
            energieInt = energieD;
            energieF = energieD;
        }

        // Update is called once per frame
        void Update() {
            if (T > 0)
            {
                T--;
            }
            else
            {
                T = 0;
            }
            GestionTailleSprite();
        }
    }
}
