using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dechecteria
{
    class CityTile : Tile {
        int cityTimer;
        double maxHeight = -0.20;
        double minHeight = 0.4;
        GameObject ownTile;
        GameObject height;

	    // Use this for initialization
	    void Start () {
            cityTimer = 0;
            ownTile = transform.root.gameObject;
            height = transform.GetChild(0).gameObject;
	    }

        // Update is called once per frame
        void Update(){
            cityTimer++;
            if (cityTimer > 59 && height.transform.localPosition.z > maxHeight) {
               height.transform.localPosition = new Vector3(0f, 0f, height.transform.localPosition.z - 0.005f);
            }
        }


    }

}
