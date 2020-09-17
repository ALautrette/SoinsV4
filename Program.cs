using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using classesMetier;

namespace SoinsV3
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<Dossier> lesDossiers = new List<Dossier>();
                // Chargement du fichier et initialisation des collections de noeuds XML
                //lesDossiers, lesPrestations, lesIntervenants
                TraitementXML.ChargeFichierXML();
                // Chargement de la collection des objets de la classe Dossier
                lesDossiers = TraitementXML.XmlToDossiers();
                Console.WriteLine("Nombre de dossiers : " + lesDossiers.Count);
                // Affichage des dossiers
                TraitementXML.AfficherDossiers(lesDossiers);

                // Gestion des intervenants
                Console.WriteLine("\n\n----- Gestion des intervenants  -----");
                // Chargement de la collection des objets de la classe Intervenant et IntevenantExterne
                List<Intervenant> lesIntervenants = TraitementXML.XmlToIntervenants();
                // affichage des intervenants 
                TraitementXML.AfficherIntervenants(lesIntervenants);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Read();
        }
    }
}
