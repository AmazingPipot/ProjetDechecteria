using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{
    class GestionRoom : MonoBehaviour
    {
        public GameConstants.GestionRoomType Type;
        public GameObject spriteAssocier;
        public Image RoomDisplay;
        public GameObject IconeParent;
        public GameObject Icone;
        public GameObject canvas;
        //public AffichageAmelioration Icone;
        public bool Visible;
        public int Level;
        public int MaxCapacity;
        [SerializeField]
        private float resources;
        public int Resources
        {
            get
            {
                return Mathf.FloorToInt(resources);
            }
            set
            {
                resources = value;
            }
        }
        public int EnergyGain;
        public int Amelioration;
        public int AmeliorationDisp;
        public int vitesseAbsorption;
        Vector3 positionSprite;
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

        public void AddResources(float value)
        {
            resources += value;
        }

        void Update()
        {
            RoomDisplay.enabled = Visible;

            if (IconeParent != null)
            {
                if (Amelioration == 0 && AmeliorationDisp > 0 && Level > 0)
                {
                    if (Icone == null)
                    {
                        positionSprite = RoomDisplay.GetComponent<RectTransform>().position;
                        Icone = Instantiate(IconeParent) as GameObject;
                        Icone.transform.SetParent(canvas.transform, false);
                        Icone.transform.GetComponent<AffichageAmelioration>().room = this;
                        Icone.transform.GetComponent<RectTransform>().position = positionSprite;//this.gameObject.GetComponent<RectTransform>().position;
                        print("ICONE A AFFICHER");
                    }
                }
                else
                {
                    if (Icone != null)
                    {
                        Destroy(Icone);
                    }
                }
            }
        }
    }
}

