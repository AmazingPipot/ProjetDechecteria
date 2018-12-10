using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gestionBackground : MonoBehaviour {
    public Sprite representation1;
    public Sprite representation2;

    public GameObject Colonie;

    public Transform Copy;
    int i = 0;
    void gestionTailleSprite()
    {
        float A = this.GetComponent<SpriteRenderer>().bounds.size.x;
        float X = this.GetComponent<RectTransform>().position.x;
        float Y = this.GetComponent<RectTransform>().position.y;
        float Z = this.GetComponent<RectTransform>().position.z;
        float ScaleX = this.GetComponent<RectTransform>().localScale.x;
        float ScaleY = this.GetComponent<RectTransform>().localScale.y;
        float rand = Random.value;
        float n = 0;
        for (int i = -1; i <= 1024/(A*100)+2; i++)
        {
            rand = Random.value;
            Transform C = Instantiate(Copy);
            C.transform.GetComponent<Transform>().position = new Vector3(X+i * A, Y, Z);
            C.transform.GetComponent<Transform>().localScale = new Vector3(ScaleX, ScaleY, 0);
            print("position : " + C.position.x);
            C.transform.GetComponent<Transform>().position = new Vector3(X + (i+rand) * A, Y, Z);
            C.transform.GetComponent<Transform>().localScale = new Vector3(ScaleX, ScaleY, 0);
            print("position : " + C.position.x);
            C.transform.GetComponent<Transform>().position = new Vector3(X + (i + rand) * A, Y, Z);
            C.transform.GetComponent<Transform>().localScale = new Vector3(ScaleX, ScaleY, 0);
            print("position : " + C.position.x);
        }
        print("Taillle sprite " + A+" "+ 1024 / (A * 100));
        //for (int i = 0; i < 1024/representation1.)
        this.transform.GetComponent<Transform>().localScale -= new Vector3(0.01f, 0.01f, 0);
        
    }
    // Use this for initialization
    void Start () {
        gestionTailleSprite();
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
