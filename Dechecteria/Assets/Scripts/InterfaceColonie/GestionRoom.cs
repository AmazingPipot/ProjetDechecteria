using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{
    class GestionRoom : MonoBehaviour
    {
        public GameConstants.GestionRoomType Type;
        public GameObject spriteAssocier;
        public Image RoomDisplay, RoomDisplay2;
        public Text RoomName, RoomLevelText;
        public GameObject son;
        public GameObject IconeParent;
        public GameObject Icone;
        public GameObject canvas;
        //public AffichageAmelioration Icone;
        public bool Visible;
        public int Level;
        public int MaxCapacity;

        public bool constructionEnCours = false;

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

        public Text paroleGaia;
        public bool aEteAffiche = false;

        public float IntervalGainEnergy;

        [Header("Debug")]
        public float TimeBeforeGainEnergy;

        void Start()
        {
            TimeBeforeGainEnergy = IntervalGainEnergy;

            RoomDisplay.rectTransform.localScale = new Vector2(0, 0);
            positionSprite = RoomDisplay.GetComponent<RectTransform>().position;
            /*if (paroleGaia != null)
            {
                Mess.paroleGaia = paroleGaia;
            }*/
        }

        public void AddResources(float value)
        {
            resources += value;
        }

        public float GetResourcesf()
        {
            return resources;
        }

        public void AnimationRoom(Vector2 scaleRoom)
        {
            if(RoomDisplay.rectTransform.localScale.x < scaleRoom.x && RoomDisplay.rectTransform.localScale.y < scaleRoom.y)
            {
                
                RoomDisplay.rectTransform.localScale = new Vector2(RoomDisplay.rectTransform.localScale.x + 0.2f*scaleRoom.x / 8,
                                                                    RoomDisplay.rectTransform.localScale.y + 0.2f*scaleRoom.y / 8);
            }
        }

        public void AnimationInsideRoom()
        {
            if (!isRecyclageRoom)
            {
                Vector2 maxScale = RoomDisplay.rectTransform.localScale;
                RoomDisplay2.rectTransform.localScale = new Vector2(resources / MaxCapacity * maxScale.x, resources / MaxCapacity * maxScale.y);
            }
        }

        void Update()
        {
            RoomDisplay.enabled = Visible;
            RoomDisplay2.enabled = Visible;

            if (Visible)
            {
                RoomName.enabled = true;
                RoomLevelText.enabled = true;

                //RoomName.rectTransform.position = new Vector2(positionSprite.x, positionSprite.y+10);
                //RoomLevelText.rectTransform.anchoredPosition = new Vector2(positionSprite.x, positionSprite.y + 105);
            }
            else
            {
                RoomName.enabled = false;
                RoomLevelText.enabled = false;
            }
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

            AnimationInsideRoom();

            if (Resources < 0)
            {
                Resources = 0;
            }
            //Affichage du text sensibilisateur
            /*if (paroleGaia != null)
            {
                if (Mess.affiche == false && Level == 1)
                {
                    Mess.lectureTexte();
                }
            }*/
        }
    }
}

