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
        public GameObject COLONIE;
        bool affichage1 = false , affichage2 = false, aEteAffiche = false;
        Colonie aff;
        String textParole = "";

        public Text afficheurText;
        // Use this for initialization
        public int level;//Le level a partir duquel le message sera affiché
        int index = 0;
        float sautLigne = 2.0f;
        float sautCaractere = 0.04f;
        float TimeAttente1;
        float TimeAttente2;

        public GameObject boxTexte;

        public void lectureTexte()
        {
            if (affichage1 == false || affichage2 == true)
            {
                affichage2 = true;
                COLONIE.transform.GetComponent<Colonie>().textEnCours = affichage2;
                TimeAttente2 -= Time.deltaTime;

                if (index < textParole.Length)
                {
                    if (TimeAttente2 < 0.0f)
                    {

                        if (textParole.Substring(index, 1) == "#")
                        {
                            TimeAttente1 -= Time.deltaTime;
                            if (TimeAttente1 < 0.0f)
                            {
                                afficheurText.text = "";
                                TimeAttente1 = sautLigne;
                                TimeAttente2 = sautCaractere;
                                index++;
                            }
                        }
                        else
                        {
                            afficheurText.text += textParole.Substring(index, 1);
                            index++;
                            TimeAttente2 = sautCaractere;
                            TimeAttente1 = sautLigne;
                        }
                    }
                }
                else
                {
                    COLONIE.transform.GetComponent<Colonie>().textEnCours = false;
                    affichage2 = false;
                    afficheurText.text = "";
                    boxTexte.SetActive(false);
                    Inst.aEteAffiche = true;
                    affiche = true;
                }
            }
            /*else
            {
                level++;
            }*/
        }

        void Start()
        {
            Inst = this.gameObject.GetComponent<GestionRoom>();

            aff = COLONIE.GetComponent<Colonie>();
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
            if (aff != null)
            {
                affichage1 = aff.textEnCours;
            }
            else
                aff = COLONIE.GetComponent<Colonie>();

            if (Inst != null)
            {
                if (Inst.Level >= level && Inst.aEteAffiche == false)
                {
                    Inst.enabled = true;
                    if (textParole == "")
                    {
                        textParole = paroleGaia.GetComponent<Text>().text;// paroleGaia.ToString();
                        //print("Text a afficher " + textParole);
                    }
                    else
                    {
                        boxTexte.SetActive(true);
                        //print("Level  de mon pmposant associe " + Inst.Level);
                        lectureTexte();
                    }
                }
            }
            /*else
            {
                //this.I//enabled = false;
            }*/
            /*if (paroleGaia != null)
                lectureTexte();*/
        }
    }
}

