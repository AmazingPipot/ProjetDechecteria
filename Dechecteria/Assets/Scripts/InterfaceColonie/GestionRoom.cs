using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{
    class GestionRoom : MonoBehaviour
    {
        public GameConstants.GestionRoomType Type;
        public GameObject spriteAssocier;
        public Image RoomDisplay;
        public bool Visible;
        public int Level;
        public int MaxCapacity;
        public int Resources;
        public int EnergyGain;
        public int AmeliorationDisp;

        public bool isRecyclageRoom;

        public float IntervalGainEnergy;

        [Header("Debug")]
        public float TimeBeforeGainEnergy;

        void Start()
        {
            TimeBeforeGainEnergy = IntervalGainEnergy;

            /*
            if (Type == GestionRoomType.RECYCLAGE_CHIMIC)
            {
                Debug.Log("Je suis la piece recyclage chimic amazing");
            }
            */
        }

        void Update()
        {
            RoomDisplay.enabled = Visible;
        }
    }
}

