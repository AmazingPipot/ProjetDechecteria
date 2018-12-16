using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dechecteria
{ 
    class MessageSerieux : MonoBehaviour
    {
        //public static GestionRoom Instance;
        GestionRoom Inst;
        public bool affiche = false;
        public Text paroleGaia;
        String textParole = "";

        public Text afficheurText;
        // Use this for initialization
        public int level;//Le level a partir duquel le message sera affiché
        int index = 0;
        float sautLigne = 1.0f;
        float sautCaractere = 0.04f;
        float TimeAttente1;
        float TimeAttente2;

        public GameObject boxTexte;

        public void lectureTexte()
        {
            TimeAttente2 -= Time.deltaTime;
          
            if (index < textParole.Length)
            {
                if (TimeAttente2 < 0.0f)
                {
                    print("Taile texte " + textParole.Length);
                    print("Texte " + textParole.Substring(index, 1) + "Index " + index);

                    if (textParole.Substring(index, 1) == "#")
                    {
                        afficheurText.text = "";
                        TimeAttente1 = sautLigne;
                        TimeAttente2 = sautCaractere;
                        index++;
                    }
                    else if (textParole.Substring(index, 1) == "\n")
                    {
                        TimeAttente1 -= Time.deltaTime;
                        if (TimeAttente1 < 0.0f)
                        {
                            afficheurText.text += textParole.Substring(index, 1);
                            index++;
                            TimeAttente1 = sautLigne;
                            TimeAttente2 = sautCaractere;
                        }
                    }
                    else
                    {
                        afficheurText.text += textParole.Substring(index, 1);
                        index++;
                        TimeAttente2 = sautCaractere;
                    }
                }
            }
            else
            {
                afficheurText.text = "";
                boxTexte.SetActive(false);
                affiche = true;
            }
        }

        void Start()
        {
            Inst = this.gameObject.GetComponent<GestionRoom>();
            //boxTexte.SetActive(false);
            /*if (paroleGaia != null)
            {
            textParole = paroleGaia.ToString();
            TimeAttente1 = sautLigne;
            TimeAttente2 = sautCaractere;
            afficheurText.text = "";
            }*/
        }

        // Update is called once per frame
        void Update()
        {
            if (Inst != null)
            {
                if (Inst.Level == level && affiche == false)
                {
                    Inst.enabled = true;
                    if (textParole == "")
                    {
                        textParole = paroleGaia.GetComponent<Text>().text;// paroleGaia.ToString();
                        print("Text a afficher " + textParole);
                    }
                    else
                    {
                        boxTexte.SetActive(true);
                        print("Level  de mon pmposant associe " + Inst.Level);
                        lectureTexte();
                    }
                }
            }
            else
            {
                //this.I//enabled = false;
            }
            /*if (paroleGaia != null)
                lectureTexte();*/
        }
    }
}

