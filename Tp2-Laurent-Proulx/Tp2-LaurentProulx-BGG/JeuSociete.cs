using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp2_LaurentProulx_BGG
{
    public struct JeuSociete
    {       
            /// <summary>
            ///  Contient le numéro identifiant le jeu.
            /// </summary>
            public int Id;

            /// <summary>
            ///  Contient le nom du jeu.
            /// </summary>
            public string Nom;

            /// <summary>
            /// Contient l'évaluation moyenne.
            /// </summary>
            public double EvalMoy;

            /// <summary>
            /// contient la durée minimum du jeu.
            /// </summary>
            public int DureeMin;

            /// <summary>
            /// Contient la durée maximale du jeu.
            /// </summary>
            public int DureeMax;

            /// <summary>
            /// Vecteur contenant la liste des catégorie du jeu.
            /// </summary>
            public string[] VectCategories;
      
    }
}
