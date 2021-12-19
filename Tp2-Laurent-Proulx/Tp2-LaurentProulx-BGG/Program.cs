using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Tp2_LaurentProulx_BGG
{
    class Program
    {
        const string cheminFichier = "C:\\data-420-04A-FX\\TP-2\\bgg-db.csv";
        static string ligne = "--------------------------------------------------------";
        static void Main(string[] args)
        {   
            //chargement des jeux dans un vecteur struct
            JeuSociete[] vectJeux = FctUtil.ChargerJeuxSociete(cheminFichier);
            //Création d'un vecteur de résultat vide pour résultat futur
            JeuSociete[] resultatRecherche = null;
            //création d'un vecteur contenant toutes les catégories
            string[] listeCategorie = FctUtil.ObtenirToutesLesCategories(vectJeux);
            //déclaration de variable pour l'enregistrement.
            int choixDerniereRech = 0;
            double evalMin = 0;
            double evalMax = 0;
            int duree = 0;
            string rechercheCat = "";
            // affichage et sélection du choix
            char choixMenu = FaireChoixDansMenu();
            // boucle sélection du menu
            while (choixMenu != '6')
            {
                if (choixMenu == '1')
                {
                    /**********************************
                     *Recherche par évaluation moyenne*
                     **********************************/

                    //Enregistrement du dernier choix pour enregistrement
                    choixDerniereRech = 1;
                    //Premier critère de recherche pour evaluation moyenne
                    Console.Write("\nVeillez entrer l'évaluation moyenne minimale ( plus grande ou égal à 1 et plus petit que 10) : ");
                    evalMin = Convert.ToDouble(Console.ReadLine(), CultureInfo.InvariantCulture);
                    //Confirmation du critère + ressaisit
                    while (evalMin < 1 || evalMin > 10)
                    {
                        Console.WriteLine("ERREUR : L'évaluation moyenne minimale n'est pas dans l'intervalle spécifié.");
                        Console.Write("Veillez entrer l'évaluation moyenne minimale ( plus grande ou égal à 1 et plus petit que 10) : ");
                        evalMin = Convert.ToDouble(Console.ReadLine(), CultureInfo.InvariantCulture);
                    }
                    Console.WriteLine(ligne);
                    //Deuxième critère de recherche pour évaluation moyenne
                    Console.Write("Veillez entrer l'évaluation moyenne maximale ( plus grande ou égal à {0} et plus petit que 10) : ", evalMin);
                    evalMax = Convert.ToDouble(Console.ReadLine(), CultureInfo.InvariantCulture);
                    //confirmation du critères + ressaisit
                    while (evalMax < evalMin || evalMax > 10)
                    {
                        Console.WriteLine("ERREUR : L'évaluation moyenne maximale n'est pas dans l'intervalle spécifié.");
                        Console.Write("Veillez entrer l'évaluation moyenne maximale ( plus grande ou égal à {0} et plus petit que 10) : ", evalMin);
                        evalMax = Convert.ToDouble(Console.ReadLine(), CultureInfo.InvariantCulture);
                    }
                    //recherche selon les moyennes min et max
                    resultatRecherche = FctUtil.RechercherSelonEvalMoy(vectJeux, evalMin, evalMax);
                    //affichage des résultats
                    AfficherJeux(resultatRecherche);
                }
                else if (choixMenu == '2')
                {
                    /*********************
                     *Recherche par durée*
                     *********************/

                    //Enregistrement du dernier choix pour enregistrement
                    choixDerniereRech = 2;
                    //Critère de recherche pour durée
                    Console.Write("Veillez entrer la durée de la partie en minutes (Plus grande ou égale à 0) : ");
                    duree = Convert.ToInt32(Console.ReadLine());
                    //Confirmation du critère de recherche + ressaisit
                    while (duree < 0)
                    {
                        Console.WriteLine("ERREUR : La durée ne peut pas être négative.");
                        Console.Write("Veillez entrer la durée de la partie en minutes (Plus grande ou égale à 0) : ");
                        duree = Convert.ToInt32(Console.ReadLine());
                    }
                    //Recherche selon durée
                    resultatRecherche = FctUtil.RechercherSelonDuree(vectJeux, duree);
                    //Affichage des résultats
                    AfficherJeux(resultatRecherche);
                }
                else if (choixMenu == '3')
                {
                    /*************************
                     *Recherche par catégorie*
                     *************************/

                    //Enregistrement du dernier choix pour enregistrement
                    choixDerniereRech = 3;
                    // Affichage des catégories du database
                    Console.WriteLine("\nCatégorie disponible ({0})", listeCategorie.Length);
                    Console.WriteLine(ligne);
                    Console.Write("{0}", listeCategorie[0]);
                    for (int i = 1; i < listeCategorie.Length; i++)
                    {
                        Console.Write(", {0}", listeCategorie[i]);
                    }
                    Console.WriteLine("\n"+ligne);
                    //critère de recherche pour catégorie
                    Console.Write("Veillez entrer la catégorie (le nom partiel suffit avec un minimum de 3 caractères) : ");
                    rechercheCat = Console.ReadLine();
                    //Confirmation du critère de recheche + ressaisit
                    while (rechercheCat.Length < 3)
                    {
                        Console.WriteLine("ERREUR : nombre de caractères insuffisant pour effectuer une recherche.");
                        Console.Write("Veillez entrer la catégorie (le nom partiel suffit avec un minimum de 3 caractères) : ");
                        rechercheCat = Console.ReadLine();
                    }
                    //Recherche selon la catégorie
                    resultatRecherche = FctUtil.RechercherSelonCategorie(vectJeux, rechercheCat);
                    //Affichage des résultats
                    AfficherJeux(resultatRecherche);
                }
                else if (choixMenu == '4')
                {
                    /*****************************************
                     *Enregistrement de la dernière recherche*
                     *****************************************/

                    //Confirmation d'une recherche
                    if (choixDerniereRech != 0)
                    {   
                        //Nom du fichier
                        Console.Write("\nEntrez le nom du fichier (sans extension) :");
                        string nomFichier = Console.ReadLine();
                        //Nom différent que l'original + ressaisit
                        while (nomFichier == "bgg-db")
                        {
                            Console.WriteLine("Utilisez un nom différent que le fichier d'origine");
                            nomFichier = Console.ReadLine();
                        }
                        //Enregistrement du fichier
                        FctUtil.EnregistrerResultatRecherche(nomFichier, resultatRecherche, choixDerniereRech, evalMin, evalMax, duree, rechercheCat);
                    }
                    // Aucune recherche effectué
                    else 
                    {
                        Console.WriteLine("\nOPÉRATION IMPOSSIBLE : Vous n'avez pas encore effectué de recherche.");
                    }
                    //Retour au menu.
                    Console.WriteLine("\nAppuyer sur une touche pour retourner au menu principale...");
                    Console.ReadKey();
                }
                else if (choixMenu == '5')
                {
                    /****************************
                     *Affichage des statistiques*
                     ****************************/

                    // Eval de la moyenne
                    double sommeEval = 0;
                    // Représente l'index des vecteurs (meilleur eval, pire eval, catégorie + pop. 1, 2 et 3)
                    int meilEval = 0;
                    int pireEval = 0;
                    // liste pour compter le nombre de catégorie + valeur initiale évaluation (dernière).
                    int[] catCount = new int[listeCategorie.Length + 1];
                    catCount[listeCategorie.Length] = -1;
                    int cat1 = listeCategorie.Length;
                    int cat2 = listeCategorie.Length;
                    int cat3 = listeCategorie.Length;
                    
                    //Calcul des statistiques moyenne, meilleur et pire.
                    for (int i = 0; i < vectJeux.Length; i++)
                    {
                        sommeEval += vectJeux[i].EvalMoy;
                        if (vectJeux[i].EvalMoy > vectJeux[meilEval].EvalMoy)
                        {
                            meilEval = i;
                        }
                        if (vectJeux[i].EvalMoy < vectJeux[pireEval].EvalMoy)
                        {
                            pireEval = i;
                        }
                    }
                    double moyenneEval = sommeEval / vectJeux.Length;

                    // calcul du nombre de catégorie
                    for (int i = 0; i < vectJeux.Length; i++)
                    {
                        for (int j = 0; j < vectJeux[i].VectCategories.Length; j++)
                        {
                            int index = Array.IndexOf(listeCategorie, vectJeux[i].VectCategories[j]);
                            catCount[index] ++;
                        }
                    }
                    // Détermination des 3 premières catégories
                    for (int i = 0; i < catCount.Length; i++)
                    { 
                        // Recherche des catégories les plus populaires
                        if (catCount[i] > catCount[cat1])
                        {
                            cat3 = cat2;
                            cat2 = cat1;
                            cat1 = i;
                        }
                        else if ( catCount[i] > catCount[cat2])
                        {
                            cat3 = cat2;
                            cat2 = i;
                        }
                        else if ( catCount[i] > catCount[cat3])
                        {
                            cat3 = i;
                        }
                    }
                    // Affichage des stastitiques

                    Console.WriteLine("\nL'évaluation moyenne est {0:0.###}", moyenneEval);
                    Console.WriteLine(ligne);
                    Console.WriteLine("Le jeu avec la meilleure évaluation est:");
                    AfficherJeu(vectJeux[meilEval]);
                    Console.WriteLine("Le jeu avec la pire évaluation est :");
                    AfficherJeu(vectJeux[pireEval]);
                    Console.WriteLine("La catégorie la plus populaire : {0} avec {1} jeux", listeCategorie[cat1], catCount[cat1]);
                    Console.WriteLine(ligne);
                    Console.WriteLine("La deuxième catégorie la plus populaire : {0} avec {1} jeux", listeCategorie[cat2], catCount[cat2]);
                    Console.WriteLine(ligne);
                    Console.WriteLine("La troisième catégorie la plus populaire : {0} avec {1} jeux", listeCategorie[cat3], catCount[cat3]);
                    Console.WriteLine(ligne);
                }
                // Mauvais choix menu + ressaisit
                else
                {
                    Console.WriteLine("ERREUR : Choix invalide");
                    Console.WriteLine("\nAppuyer sur une touche pour retourner au menu principale...");
                    Console.ReadKey();
                }
                // Nouveau choix menu
                choixMenu = FaireChoixDansMenu();
            }
            // choix de quitter
            Console.WriteLine("\nAu revoir");
        }

        /// <summary>
        /// Permet d'afficher le menu et de faire un choix.
        /// </summary>
        /// <returns>Le choix de l'utilisateur dans le menu.</returns>
        static char FaireChoixDansMenu()
        {
            //affichage des choix du menu
            Console.WriteLine("\nMENU PRINCIPALE");
            Console.WriteLine("===============");
            Console.WriteLine("1) Rechercher les jeux selon l'évaluation moyenne.");
            Console.WriteLine("2) Rechercher les jeux selon la durée de la partie.");
            Console.WriteLine("3) Rechercher les jeux selon une catégorie.");
            Console.WriteLine("4) Enregistrer le résultat de la derniere recherche.");
            Console.WriteLine("5) Afficher les statistiques");
            Console.WriteLine("6) Fin du programme.");
            Console.Write("\nVotre choix ? :");
            //sélection du caractère.
            char choix = Console.ReadKey().KeyChar;
            Console.WriteLine("");
            //retour du caractère
            return choix;
        }

        /// <summary>
        /// Permet d'afficher dans la console plusieurs jeux de société.
        /// </summary>
        /// <param name="vectJeux">Vecteur de jeux de société à afficher.</param>
        static void AfficherJeux(JeuSociete[] vectJeux)
        {
            //Affichage du 'x' si 2 ou plus objects à afficher
            string pluriel = "x";
            if (vectJeux.Length <= 1)
            {
                pluriel = "";
            }
            //Entete
            Console.WriteLine("\nRésultat de la recherche");
            Console.WriteLine("========================");
            //Aucune Jeu
            if (vectJeux.Length == 0)
            {
                Console.WriteLine("Aucun jeu corespondant au critère de recherche");
            }
            //Affichage des Jeux
            else
            {
                Console.WriteLine("{0} jeu{1} correspondant aux critères de recherche (max = 10)", vectJeux.Length, pluriel);
                Console.WriteLine(ligne);
                for (int i = 0; i < vectJeux.Length; i++)
                { 
                    AfficherJeu(vectJeux[i]);
                }
            }
            //Retour au menu
            Console.WriteLine("\nAppuyer sur une touche pour retourner au menu principale...");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet d'afficher dans la console un seul jeu de société.
        /// </summary>
        /// <param name="jeu">Le jeu de société à afficher.</param>
        static void AfficherJeu(JeuSociete jeu)
        {
            //Création de la chaine pour Catégorie
            string categorie = jeu.VectCategories[0];
            for (int j = 1; j < jeu.VectCategories.Length; j++)
            {
                categorie += (", " + jeu.VectCategories[j]);
            }
            //Affichage des données du jeu
            Console.WriteLine("{0} ({1})", jeu.Nom, jeu.Id);
            Console.WriteLine("De {0} à {1} min.", jeu.DureeMin, jeu.DureeMax);
            Console.WriteLine("Éval. Moy. = {0:N3}", jeu.EvalMoy);
            Console.WriteLine("Catégories : {0}", categorie);
            Console.WriteLine(ligne);
        }
    }
}
