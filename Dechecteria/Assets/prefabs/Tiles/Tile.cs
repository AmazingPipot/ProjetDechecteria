using UnityEngine;
using UnityEngine.EventSystems;

namespace Dechecteria
{
    class Tile : MonoBehaviour, IPointerClickHandler
    {

        /*
         * Si la case est une ville, une zone naturelle,...
         * si dessous valeurs possibles
         * 
         * [0-9] zone naturelle
         * 0 eau
         * 1 plaine
         * 2 foret
         * 3 montagne
         * ...
         * [10-19] campagne agricole
         * 10 champ
         * 11 village
         * 12 petite ville (faible defense/attaque)
         * 13 moyenne ville (moyenne defense/attaque)
         * 14 grosse ville (forte defense/attaque)
         * ...
         * [20-29] centre industriel
         * 20 centre industrielle mineral
         * 21 centre industrielle métal
         * 22 centre industrielle chimique
         * 23 centre industrielle petrole
         * 24 centre industrielle nucleaire
         * 25 centre industrielle papier
         * 26 centre industrielle plastique
         * 27 centre industrielle complexe
         * ...
         * [30-39] centre de recyclage
         * 30 décharge
         * 31 recyclage métal
         * 32 recyclage chimique
         * 33 ....
        */
        public GameConstants.TILE_TYPE Type;
        /*
         * Proportion de chaque élément décrit ci-dessous 
        */
        public float population=0;
        public float matiereOrganique=0;
        public float mineral=0;
        public float metal=0;
        public float chimique=0;
        public float petrole=0;
        public float nucleaire=0;

        public float papier=0;
        public float plastique=0;
        public float complexe=0;

        /*
         * Propriété de la case sur la vie de la créature
        */
        public bool pollution;// Si la case est ou non extrement polluée, tue la matiere organique si trop polluée
        public int defense;//La capacité de résistance à la créature de la case (ville)
        public int attaque;// Capacité offensive de la créature face à la case.

        public bool IsWalkable;

        public float gainPerTick;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Right)
            {
                
                if (OnTileClickEvent != null)
                {
                    OnTileClickEvent(this, eventData.pointerCurrentRaycast.worldPosition);
                }
            }
        }

        public event OnTileClickEventHandler OnTileClickEvent;
        public delegate void OnTileClickEventHandler(Tile tile, Vector3 worldPosition);
    }

}
