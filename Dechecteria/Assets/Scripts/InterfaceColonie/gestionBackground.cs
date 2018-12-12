using System.Collections.Generic;
using UnityEngine;

public class GestionBackground : MonoBehaviour
{
    List<Transform> listCopy;

    public Transform Copy;
    public Transform Copy2;

    int i = 0;
    void GestionTailleSprite()
    {
        float A = this.GetComponent<SpriteRenderer>().bounds.size.x;
        float X = this.GetComponent<RectTransform>().position.x;
        float Y = this.GetComponent<RectTransform>().position.y;
        float Z = this.GetComponent<RectTransform>().position.z;

        float ScaleX = this.GetComponent<RectTransform>().localScale.x;
        float ScaleY = this.GetComponent<RectTransform>().localScale.y;
        float rand = Random.value;
        float n = 0;

        float Cbound = Copy.GetComponent<SpriteRenderer>().bounds.size.x;
        float Cx = Copy.GetComponent<RectTransform>().position.x;
        float Cy = Copy.GetComponent<RectTransform>().position.y;
        float Cz = Copy.GetComponent<RectTransform>().position.z;
        float Csx = Copy.GetComponent<RectTransform>().localScale.x;
        float Csy = Copy.GetComponent<RectTransform>().localScale.y;

        for (int i = 1; i <= 1024/(Cbound*100*Csx)+1; i++)
        {
            rand = Random.value;
            Transform C = Instantiate(Copy);

            C.transform.GetComponent<Transform>().position = new Vector3(Cx + i * (Cbound-2), Cy, Cz);
            C.transform.GetComponent<Transform>().localScale = new Vector3(Csx, Csy, 0);
            /*print("position : " + C.position.x);
            C.transform.GetComponent<Transform>().position = new Vector3(X + (i+rand) * A, Y, Z);
            C.transform.GetComponent<Transform>().localScale = new Vector3(ScaleX, ScaleY, 0);
            print("position : " + C.position.x);
            C.transform.GetComponent<Transform>().position = new Vector3(X + (i + rand) * A, Y, Z);
            C.transform.GetComponent<Transform>().localScale = new Vector3(ScaleX, ScaleY, 0);
            print("position : " + C.position.x);*/
        }
        print("Taillle sprite " + A+" "+ 1024 / (A * 100));
        //for (int i = 0; i < 1024/representation1.)
        //this.transform.GetComponent<Transform>().localScale -= new Vector3(0.01f, 0.01f, 0);

        Cbound = Copy2.GetComponent<SpriteRenderer>().bounds.size.x;
        Cx = Copy2.GetComponent<RectTransform>().position.x;
        Cy = Copy2.GetComponent<RectTransform>().position.y;
        Cz = Copy2.GetComponent<RectTransform>().position.z;
        Csx = Copy2.GetComponent<RectTransform>().localScale.x;
        Csy = Copy2.GetComponent<RectTransform>().localScale.y;

        for (int i = 1; i <= 1024 / (Cbound * 100 * Csx)+1; i++)
        {
            rand = Random.value;
            Transform C = Instantiate(Copy);

            C.transform.GetComponent<Transform>().position = new Vector3(Cx + i * (Cbound - 2), Cy, Cz);
            C.transform.GetComponent<Transform>().localScale = new Vector3(Csx, Csy, 0);
        }

    }
    // Use this for initialization
    void Start () {
        GestionTailleSprite();
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
