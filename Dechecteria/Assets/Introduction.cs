using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Introduction : MonoBehaviour {
    public Text affText;
    // Use this for initialization
    int index = 0;
    string Intro;

    void lectureTexte()
    {
        if (index < Intro.Length-2)
        {
            print("Taile texte " + Intro.Length);
            print("Texte "+ Intro.Substring(index, 1)+ "Index "+index);
            affText.text += Intro.Substring(index, 1);
            index++;
        }
    }
	void Start () {
        affText.text = "";
        Intro = "Les Hommes...\n"+
            "Les Hommes mes enfants, m'ont trahi...\n"+
            "Ils polluent ma chair, ils détruisent ma terre ! \n"+
            "Nombreux sont mes enfants à avoir disparu par leur faute...\n"+
            "...\n"+
            "Il faut que cela cesse ! Je ne puis plus tolérer tant d'inconscience de leur part !\n"+
            "O toi petite chose bien inconsciente du malheur qui se prépare...\n"+
            "Je te charge d'une mission.\n"+
            "Prend un peu de mon pouvoir, prend un peu de ma tristesse et prend beaucoup de ma colère \n!"+
            "Les Hommes vont disparaître !\n"+
            "A toi petite chose je te confie cette tâche.\n"+
            "Détruit leur cité, détruit leurs enfants !/n"+
            "Purifie ma Terre de leur méfait ! Utilise leur création pour les détruire !\n"+
            "TOUS !";
	}
	
	// Update is called once per frame
	void Update () {
        lectureTexte();
        //Intro = Intro.Remove(0);
	}
}
