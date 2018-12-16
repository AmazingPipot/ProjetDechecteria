using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Introduction : MonoBehaviour {
    public Text affText;
    // Use this for initialization
    int index = 0;
    string Intro;
    float TimeAttente1 = 1.0f;
    float TimeAttente2 = 0.1f;

    void lectureTexte()
    {
        if (index < Intro.Length)
        {
            if (TimeAttente2 < 0.0f)
            {
                print("Taile texte " + Intro.Length);
                print("Texte " + Intro.Substring(index, 1) + "Index " + index);

                if (Intro.Substring(index, 1) == "#")
                {
                    affText.text = "";
                    TimeAttente1 = 1.0f;
                    TimeAttente2 = 0.1f;
                    index++;
                }
                else if (Intro.Substring(index, 1) == "\n") {
                    TimeAttente1 -= Time.deltaTime;
                    if (TimeAttente1 < 0.0f)
                    {
                        affText.text += Intro.Substring(index, 1);
                        index++;
                        TimeAttente1 = 1.0f;
                        TimeAttente2 = 0.1f;
                        

                    }
                }
                else
                {
                    affText.text += Intro.Substring(index, 1);
                    index++;
                    TimeAttente2 = 0.1f;
                }
            }
        }
    }
	void Start () {
        affText.text = "";
        Intro = "Les Hommes...\n"+
            "Les Hommes mes enfants, m'ont trahi...\n"+
            "Ils polluent ma chair, ils détruisent ma terre ! \n"+
            "Nombreux sont mes enfants à avoir disparu par leur faute...\n"+
            "...\n"+"#"+
            "Il faut que cela cesse ! Je ne puis plus tolérer tant d'inconscience de leur part !\n"+
            "O toi petite chose bien inconsciente du malheur qui se prépare...\n"+
            "Je te charge d'une mission.\n"+"#"+
            "Prend un peu de mon pouvoir, prend un peu de ma tristesse et prend beaucoup de ma colère \n!"+
            "Les Hommes vont disparaître !\n"+"#"+
            "A toi petite chose je te confie cette tâche.\n"+"#"+
            "Détruit leur cité, détruit leurs enfants !\n"+
            "Purifie ma Terre de leur méfait ! Utilise leur création pour les détruire !\n"+"#"+
            "TOUS !";
	}
	
	// Update is called once per frame
	void Update () {
        lectureTexte();
        TimeAttente2 -= Time.deltaTime;
        //Intro = Intro.Remove(0);
	}
}
