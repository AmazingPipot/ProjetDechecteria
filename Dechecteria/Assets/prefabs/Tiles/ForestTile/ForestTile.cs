using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dechecteria
{
    /*
    Foret:
    Beaucoup organique
    Moyen mineral
    Peu metal (si tu veux gisement naturel)
    */

    class ForestTile : Tile
    {

        float initialValue;
        int childCount;
        GameObject[] tree;

        // Use this for initialization
        void Start()
        {
            initialValue = matiereOrganique;
            childCount = transform.childCount;
            tree = new GameObject[childCount];
            for (int i = 0; i < childCount; i++)
            {
                tree[i] = transform.GetChild(i).gameObject;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (matiereOrganique < initialValue) {
                matiereOrganique += gainPerTick;
                plastique += gainPerTick*1.45f;
            } else if (matiereOrganique > initialValue)
            {
                matiereOrganique = initialValue;
            }

            int limit = Mathf.FloorToInt(childCount * (matiereOrganique / initialValue));
            for (int i = 0; i < limit; i++)
            {
                tree[i].SetActive(true);
            }
            for (int i = limit; i < childCount; i++)
            {
                tree[i].SetActive(false);
            }
        }
    }
}