﻿using System.Collections.Generic;
using UnityEngine;

namespace Dechecteria
{
    public class GestionEvolution : MonoBehaviour
    {
        public static GestionEvolution Instance;

        public List<int> baseRessourcePieceReserve = new List<int>();
        public List<int> baseRessourcePieceRecyclage = new List<int>();
        public List<int> baseRessourceAmelioration = new List<int>();
    
        public SousMenuColonie CurrentSousMenu;

        public int nbPieceReserve = 7;//variable indiquant le nombre de piece de type de reserve (0-6)
        public int nbPieceRecyclage = 14;//Nombre de piece dédiées au recyclage, de (7 à 13)
        //public int nbPieceComplexe = 14;//Nombre de piece dédiées au recyclage, de (13 à )
        public int nbAmelioration = 17;//Nombre de parametres améliorables (14-16)
        public int nbRessource = 6;//nombre de ressources traitables;
        public int reserveMax = 2000; //base quantite max de skockage

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("Une seule instance de Gestion Evolution est requise.");
                DestroyImmediate(this.gameObject);
            }
        }

        public void MAJameliorationPiece()
        {
            //ressourceOrga = nvPieceOrga;
        } 

	    void Start ()
        {
            /*
             * Base de ressource pour construire une pièce
             * 0 base orga
             * 1 base minéral
             * 2 base métal
             * 3 base pétrole
             * 4 base chimique
             * 5 base nucléaire
            */
            int B = 100;
            for (int i = 0; i < nbRessource; i++)
            {
                baseRessourcePieceReserve.Add((int)(B * (1.0 - (0.05 * i))));
            }
            /*
            baseRessourcePieceReserve.Add(100);
            baseRessourcePieceReserve.Add(90);
            baseRessourcePieceReserve.Add(80);
            baseRessourcePieceReserve.Add(70);
            baseRessourcePieceReserve.Add(60);
            baseRessourcePieceReserve.Add(50);
            baseRessourcePieceReserve.Add(40);
            */

            /*
             *  Ressource nécessaire pour la construction des pièces de transformations
             * 
            */
            B = 200;
            for (int i = 0; i < nbPieceRecyclage - nbPieceReserve; i++)
            {
                if (i == 0)
                {
                    baseRessourcePieceRecyclage.Add(B);
                }
                else
                {
                    baseRessourcePieceRecyclage.Add(baseRessourcePieceRecyclage[i-1]-10);
                } 
            }
            /*
             * Ressource de base nécessaire pour effectuer les amélioation
            */
            B = 300;
            for (int i = 0; i < nbRessource-1; i++)
            {
                baseRessourceAmelioration.Add((int)(B * (1.0-(0.05*i))));
            }
        }
    }

}
