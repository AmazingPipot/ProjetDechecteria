using UnityEngine;

namespace Dechecteria
{
    public class SousMenuColonie : MonoBehaviour
    {
        public GameObject gestionAffichage;
        public bool IsActive;

        public void Start()
        {
            AffichageSousBoutons();
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
            IsActive = !IsActive;
            AffichageSousBoutons();
        }
    }
}
