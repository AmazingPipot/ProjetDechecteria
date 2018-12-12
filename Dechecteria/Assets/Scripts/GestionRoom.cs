using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{
    class GestionRoom : MonoBehaviour
    {
        public GameConstants.GestionRoomType Type;
        public GameObject spriteAssocier;
        private Image Image;
        public bool Visible;
        public int Level;
        public int MaxCapacity;
        public int Resources;
        public int EnergyGain;

        public bool isRecyclageRoom;

        public float IntervalGainEnergy;

        [Header("Debug")]
        public float TimeBeforeGainEnergy;

        void Start()
        {
            TimeBeforeGainEnergy = IntervalGainEnergy;
            Image = GetComponent<Image>();

            /*
            if (Type == GestionRoomType.RECYCLAGE_CHIMIC)
            {
                Debug.Log("Je suis la piece recyclage chimic amazing");
            }
            */
        }

        void Update()
        {
            Image.enabled = Visible;
        }
    }
}

