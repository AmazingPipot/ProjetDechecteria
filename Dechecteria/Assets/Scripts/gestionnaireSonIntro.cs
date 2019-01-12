using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestionnaireSonIntro : MonoBehaviour {
    FMOD.Studio.EventInstance instOiseau;
    FMOD.Studio.EventInstance instUsine;
    FMOD.Studio.EventInstance instSirene;
    Introduction cible;

    public GameObject objCible;
    // Use this for initialization
    void Start () {
        instOiseau = FMODUnity.RuntimeManager.CreateInstance("event:/DechecteriaSound/BruitageNature");
        instUsine = FMODUnity.RuntimeManager.CreateInstance("event:/DechecteriaSound/BruitageUsine");
        instSirene = FMODUnity.RuntimeManager.CreateInstance("event:/DechecteriaSound/BruitageVille");

        print("IS VALID " + instUsine.isValid());
        print("IS VALID " + instOiseau.isValid());

        instOiseau.start();
        instUsine.start();

        cible = objCible.GetComponent<Introduction>();
    }
	
	// Update is called once per frame
	void Update () {
		if (cible.seisme == true)
        {
            instOiseau.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        if (cible.fin == true)
        {
            instSirene.start();
        }
	}

    void OnDestroy()
    {
        instSirene.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instUsine.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
