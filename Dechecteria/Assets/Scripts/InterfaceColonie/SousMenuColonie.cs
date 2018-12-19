using UnityEngine;

namespace Dechecteria
{
    public class SousMenuColonie : MonoBehaviour
    {
        public enum ButtonName {
            RESERVES,
            EVOLUTIONS,
            TRANSFORMATION

        }

        public GameObject gestionAffichage;
        public bool IsActive;
        public ButtonName Name;

        private int ClicksCount;

        public void Start()
        {
            AffichageSousBoutons();
            ClicksCount = 0;
        }

        public void AffichageSousBoutons()
        {
            if (GestionEvolution.Instance.CurrentSousMenu != null && GestionEvolution.Instance.CurrentSousMenu != this)
            {
                GestionEvolution.Instance.CurrentSousMenu.gestionAffichage.SetActive(false);
                GestionEvolution.Instance.CurrentSousMenu.IsActive = false;

            }
            if (IsActive)
            {
                GestionEvolution.Instance.CurrentSousMenu = this;
            }
            else
            {
                GestionEvolution.Instance.CurrentSousMenu = null;
            }
            gestionAffichage.SetActive(IsActive);
        }

        public void OnMouseDown()
        {
            ClicksCount++;
            IsActive = !IsActive;
            AffichageSousBoutons();
            if (Name == ButtonName.RESERVES && ClicksCount == 1)
            {
                CameraController.Instance.DisplayBubble("Crée une réserve afin de pouvoir récupérer de la ressource organique", TooltipBubble.TipPosition.LEFT, gestionAffichage.transform.GetChild(0).position.x + 190.0f, gestionAffichage.transform.GetChild(0).position.y, 500.0f);
            }
        }
    }
}
