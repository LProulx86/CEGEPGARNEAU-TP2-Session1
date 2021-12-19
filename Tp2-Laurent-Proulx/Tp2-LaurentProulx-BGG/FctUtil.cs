using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Tp2_LaurentProulx_BGG
{
    class FctUtil
    {
        const int nbMaxAffiche = 10;

        /// <summary>
        /// Permet de charger tous les jeux de société.
        /// </summary>
        /// <param name="cheminFichier">Chemin d'accès au fichier contenant les jeux au format CSV.</param>
        /// <returns>Vecteur de tous les jeux de société.</returns>
        public static JeuSociete[] ChargerJeuxSociete(string cheminFichier)
        {
            // Création du flux
            StreamReader lectureFichier = new StreamReader(cheminFichier);
            // Lecture du flux
            string listeComplete = lectureFichier.ReadToEnd();
            // Fermeture du flux
            lectureFichier.Close();
            //Création d'un vecteur pour enregistrement
            listeComplete = listeComplete.Replace("\r", "");
            string[] vectListe = listeComplete.Split('\n');
            // calcul du nombre de jeux dans la liste
            int nbJeux;
            if (vectListe[vectListe.Length - 1] != "")
            {
                nbJeux = vectListe.Length - 1;
            }
            else
            {
                nbJeux = vectListe.Length - 2;
            }
            //Création du vecteur avec le bon nombre de case
            JeuSociete[] vectJeux = new JeuSociete[nbJeux];

            for (int i = 0; i < nbJeux; i++)
            {
                // Sélection du jeu + saut de l'entête + séparation de la chaine
                string[] vectChamps = vectListe[i + 1].Split(';');
                // Enregistrement des données utiles dans nouveau vecteur
                vectJeux[i].Id = Convert.ToInt32(vectChamps[2]);
                vectJeux[i].Nom = vectChamps[3].Trim();
                vectJeux[i].DureeMin = Convert.ToInt32(vectChamps[7]);
                vectJeux[i].DureeMax = Convert.ToInt32(vectChamps[8]);
                vectJeux[i].EvalMoy = Convert.ToDouble(vectChamps[10], CultureInfo.InvariantCulture);
                vectJeux[i].VectCategories = vectChamps[17].Replace(" ", "").Split(',');
            }
            //retour du vecteur struct
            return vectJeux;
        }

        /// <summary>
        /// Permet d'obtenir toutes les catégories existantes pour des jeux de société.
        /// </summary>
        /// <param name="vectJeux">Vecteur de tous les jeux de société.</param>
        /// <returns>Vecteur de toutes les catégories existantes (sans doublons).</returns>
        public static string[] ObtenirToutesLesCategories(JeuSociete[] vectJeux)
        {
            // création d'une grand vecteur temporaire
            string[] categorieTempo = new string[200];
            // variable de type compteur pour la grandeur du vecteur
            int compteurElement = 0;
            // boucle pour parcourir tous les catégorie de tous les jeux
            for (int i = 0; i < vectJeux.Length; i++)
            {
                for (int j = 0; j < vectJeux[i].VectCategories.Length; j++)
                {   
                    //vérification si la catégorie existe
                    if (!ElementEstContenuDansVecteur(vectJeux[i].VectCategories[j], categorieTempo, compteurElement))
                    {
                        categorieTempo[compteurElement] = vectJeux[i].VectCategories[j];
                        compteurElement++;
                    }
                }
            }
            // création du vecteur final
            string[] categorie = new string[compteurElement];
            for (int i = 0; i < compteurElement; i++)
            {
                categorie[i] = categorieTempo[i];
            }
            // tri du vecteur
            Array.Sort(categorie);
            // retour du vecteur
            return categorie;
        }

        /// <summary>
        /// Permet de vérifier si un certain élément (une chaîne de caractères) est contenu
        /// dans un vecteur d'éléments non plein.
        /// </summary>
        /// <param name="element">Élément recherché.</param>
        /// <param name="vectElements">Vecteur d'éléments dans lequel on tente d'identifier l'élément recherché.</param>
        /// <param name="nbElemVect">Nombre d'éléments contenu dans le vecteur non plein.</param>
        /// <returns>true si l'élément est présent; false, autrement</returns>
        /// <remarks>Recherche insensible à la casse.</remarks>
        public static bool ElementEstContenuDansVecteur(string element, string[] vectElements, int nbElemVect)
        {
            // booleen true si l'objet est trouve
            bool resultat = false;
            // boucle qui vérifie si l'élément existe
            int index = 0;
            while (!resultat && index < nbElemVect )
            {
                if (element.ToLower() == vectElements[index].ToLower())
                {
                    resultat = true;
                }
                index++;
            }
            return resultat;

        }

        /// <summary>
        /// Permet de rechercher les jeux de société selon l'évaluation moyenne.
        /// </summary>
        /// <param name="vectJeux">Vecteur de tous les jeux de société.</param>
        /// <param name="evalMin">Évaluation moyenne minimale.</param>
        /// <param name="evalMax">Évaluation moyenne maximale.</param>
        /// <returns>Les 10 premiers jeux dont l'évaluation moyenne est entre le minimum et la maximum.</returns>
        public static JeuSociete[] RechercherSelonEvalMoy(JeuSociete[] vectJeux, double evalMin, double evalMax)
        {
            // variable de comptage
            int countJeu = 0;
            int index = 0 ;
            // création d'un vecteur temporaire
            JeuSociete[] rechercheTempo = new JeuSociete[nbMaxAffiche];
            // recherche des jeux compatibles aux critères
            while (countJeu < nbMaxAffiche && index < vectJeux.Length)
            {
                if (vectJeux[index].EvalMoy >= evalMin && vectJeux[index].EvalMoy <= evalMax)
                {
                    rechercheTempo[countJeu] = vectJeux[index];
                    countJeu++;
                }
                index++;
            }
            // création d'un vecteur de grandeur équivalente au résultats trouvé
            JeuSociete[] recherche = new JeuSociete[countJeu];
            for (int i = 0; i < recherche.Length; i++)
            {
                recherche[i] = rechercheTempo[i];
            }
            return recherche;
        }

        /// <summary>
        /// Permet de rechercher les jeux de société selon la durée.
        /// </summary>
        /// <param name="vectJeux">Vecteur de tous les jeux de société.</param>
        /// <param name="duree">Durée désirée du jeu.</param>
        /// <returns>Les 10 premiers jeux pour lesquels la durée désirée est entre la durée minimale et maximale du jeu.</returns>
        public static JeuSociete[] RechercherSelonDuree(JeuSociete[] vectJeux, int duree)
        {
            // variable de compteur
            int countJeu = 0;
            int index = 0;
            // vecteur temporaire
            JeuSociete[] rechercheTempo = new JeuSociete[nbMaxAffiche];
            //recherche selon critère 
            while (countJeu < nbMaxAffiche && index < vectJeux.Length)
            {
                if (vectJeux[index].DureeMin >= duree && vectJeux[index].DureeMax <= duree )
                {
                    rechercheTempo[countJeu] = vectJeux[index];
                    countJeu++;
                }
                index++;
            }
            // vecteur selon le nombre de résultats trouvé
            JeuSociete[] recherche = new JeuSociete[countJeu];
            for (int i = 0; i < recherche.Length; i++)
            {
                recherche[i] = rechercheTempo[i];
            }
            return recherche;
        }

        /// <summary>
        /// Permet de rechercher les jeux de société selon la catégorie.
        /// </summary>
        /// <param name="vectJeux">Vecteur de tous les jeux de société.</param>
        /// <param name="categorie">Catégorie du jeu.</param>
        /// <returns>Les 10 premiers jeux qui sont dans la catégorie.</returns>
        public static JeuSociete[] RechercherSelonCategorie(JeuSociete[] vectJeux, string categorie)
        {
            //variable compteur
            int countJeu = 0;
            int index = 0;
            // vecteur temporaire
            JeuSociete[] rechercheTempo = new JeuSociete[nbMaxAffiche];
            //recherche selon les critères 
            while (countJeu < nbMaxAffiche && index < vectJeux.Length)
            {
                // booleen : évite de doublé la concordance de 2 résultats pour la même jeux
                bool trouve = true;
                // indice de la 2e boucle
                int jIndex = 0;
                while (trouve)
                {
                    if (vectJeux[index].VectCategories[jIndex].ToLower().Contains(categorie.ToLower()))
                    {
                        rechercheTempo[countJeu] = vectJeux[index];
                        countJeu ++;
                        trouve = false;
                    }
                    jIndex++;
                }
                index++;
            }
            // création vecteur de la bonne grandeur
            JeuSociete[] recherche = new JeuSociete[countJeu];
            for (int i = 0; i < recherche.Length; i++)
            {
                recherche[i] = rechercheTempo[i];
            }
            return recherche;

        }

        /// <summary>
        /// Permet d'enregistrer dans un fichier texte le résultat d'une recherche de jeux.
        /// </summary>
        /// <param name="cheminFichier">Chemin d'accès au fichier dans lequel le résultat de la recherche sera enregistré.</param>
        /// <param name="vectJeuxTrouves">Vecteur de jeux trouvés lors de la recherche.</param>
        /// <param name="typeRech">Type de recherche effectué (1:selon éval. moy., 2:selon durée et 3:selon catégorie)</param>
        /// <param name="paraEvalMin">Évaluation moyenne minimum (utilisé si type de recherche = 1).</param>
        /// <param name="paraEvalMax">Évaluation moyenne maximum (utilisé si type de recherche = 1).</param>
        /// <param name="paraDuree">Durée d'une partie en minutes (utilisé si type de recherche = 2).</param>
        /// <param name="paraCategorie">Catégorie (utilisé si type de recherche = 3).</param>
        public static void EnregistrerResultatRecherche(string cheminFichier, JeuSociete[] vectJeuxTrouves, int typeRech, double paraEvalMin, double paraEvalMax, int paraDuree, string paraCatégorie)
        {
            // création du chemin d'enregistrement
            string cheminEnregistrement = ("C:\\data-420-04A-FX\\TP-2\\" + cheminFichier + ".csv");
            // création de l'entete
            string enTete = "Game_id;names;";

            // Entête enregistrement Moyenne
            if (typeRech == 1)
            {
                enTete += "min_time;max_time;avg_rating(" + paraEvalMin + ">= et <=" + paraEvalMax + ");category";
            }
            // Entête enregistrement Duree
            else if (typeRech == 2)
            {
                enTete += "min_time>=" + paraDuree + ";max_time<=" + paraDuree + ";avg_rating;category";
            }
            // Entête enregistrement Catégorie
            else if (typeRech == 3)
            {
                enTete += "min_time;max_time;avg_rating;category=" + paraCatégorie;
            }

            // création du fichier
            StreamWriter enregistrement = new StreamWriter(cheminEnregistrement, true);
            // écriture de l'entete
            enregistrement.WriteLine(enTete);
            // variable de chaine pour un jeu
            string LigneResult = "";
            // création des lignes une à la fois
            for (int i = 0; i < vectJeuxTrouves.Length; i++)
            {
                if ( vectJeuxTrouves[i].Id > 0)
                {
                    LigneResult = (vectJeuxTrouves[i].Id + ";" + vectJeuxTrouves[i].Nom + ";" + vectJeuxTrouves[i].DureeMin + ";" + vectJeuxTrouves[i].DureeMax + ";" + vectJeuxTrouves[i].EvalMoy + ";");
                    LigneResult += (vectJeuxTrouves[i].VectCategories[0]);
                    for (int j = 1; j < vectJeuxTrouves[i].VectCategories.Length; j++)
                    {
                        LigneResult += (", " + vectJeuxTrouves[i].VectCategories[j]);
                    }
                }
                // écriture de la ligne
                enregistrement.WriteLine(LigneResult);
            }
            // fermeture du flux
            enregistrement.Close();
            Console.WriteLine("Enregistrement Réussit");
        }
    }
}
