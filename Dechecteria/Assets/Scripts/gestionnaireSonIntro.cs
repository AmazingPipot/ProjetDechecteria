using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestionnaireSonIntro : MonoBehaviour {
    FMOD.Studio.EventInstance instOiseau;
    FMOD.Studio.EventInstance instUsine;
    FMOD.Studio.EventInstance instSirene;
    AudioSource audio;
    Introduction cible;

    public GameObject objCible;
    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();

        instOiseau = FMODUnity.RuntimeManager.CreateInstance("event:/DechecteriaSound/BruitageNature");
        instUsine = FMODUnity.RuntimeManager.CreateInstance("event:/DechecteriaSound/BruitageUsine");
        instSirene = FMODUnity.RuntimeManager.CreateInstance("event:/DechecteriaSound/BruitageVille");

        print("IS VALID " + instUsine.isValid());
        print("IS VALID " + instOiseau.isValid());

        instOiseau.start();
        instUsine.start();

        cible = objCible.GetComponent<Introduction>();

        audio.Play();// OneShot(audio.clip);
    }
	
	// Update is called once per frame
	void Update () {
        //print("Musique "+audio.isPlaying+" "+cible.isActiveAndEnabled);
        if (cible.isActiveAndEnabled == true)
        {
            audio.volume-= 0.01f;

            if (audio.volume < 0.1)
            {
                audio.Stop();//stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
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
