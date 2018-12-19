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
        float rng;
        Color Vert;
        Color Yellow;
        
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
            rng = 0.5f + Random.Range(0f, 0.5f);
            Vert = new Color(0f / 255f, 188f / 255f, 56 / 255f);
            Yellow = Color.yellow;
        }

        // Update is called once per frame
        void Update()
        {
            if (matiereOrganique < initialValue*1.1f && matiereOrganique > 0f) {
                matiereOrganique += GainPopulationPerSecond * rng;
                plastique += GainPopulationPerSecond*1.45f;
            } else if (matiereOrganique > initialValue)
            {
                matiereOrganique = initialValue;
            }

            float l = 1f - matiereOrganique / initialValue;
            gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Vert, Yellow, l);

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