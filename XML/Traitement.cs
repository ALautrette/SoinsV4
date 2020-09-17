using classesMetier;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SoinsV3
{
    abstract class TraitementXML
    {
        private static XmlNodeList lesDossiers;
        private static XmlNodeList lesPrestations;
        private static XmlNodeList lesIntervenants;

        /// <summary>
        /// Obtient ou définit la collection des dossiers (TagName dossier)
        /// </summary>
        public static XmlNodeList LesDossiers
        {
            get
            {
                return lesDossiers;
            }

            set
            {
                lesDossiers = value;
            }
        }
        
        /// <summary>
        /// Obtient ou définit la collection des prestations (TagName prestation)
        /// </summary>
        public static XmlNodeList LesPrestations
        {
            get
            {
                 return lesPrestations;
            }

            set
            {
                lesPrestations = value;
            }
        }

        /// <summary>
        /// Obtient ou définit la collection des dossiers (TagName intervenant)
        /// </summary>
        public static XmlNodeList LesIntervenants
        {
            get
            {
                return lesIntervenants;
            }

            set
            {
                lesIntervenants = value;
            }
        }

        /// <summary>
        /// Définit les collections de noeuds XML pour les tags 
        /// dossier
        /// prestation
        /// intervenant
        /// </summary>
        /// <param name="racine">2lément racine du fichier XML à partir duquel la recherche d'éléments va s'effectuer</param>
        private static void Initialiser(XmlElement racine)
        {
            LesDossiers = racine.ChildNodes[0].ChildNodes;
            LesPrestations = racine.ChildNodes[1].ChildNodes;
            LesIntervenants = racine.ChildNodes[2].ChildNodes;
            
        }

        /// <summary>
        /// Charge le fichier XML à traiter. 
        /// Définit l'élément racine du document
        /// initialise les collections
        /// Voir https://docs.microsoft.com/fr-fr/dotnet/api/system.xml.xmldocument?view=netcore-3.1
        /// </summary>
        public static void ChargeFichierXML()
        {
            XmlDocument soinsXml = new XmlDocument();
            string fichier = ConfigurationManager.AppSettings["chemin"];
            soinsXml.Load(fichier);
            XmlElement racine = soinsXml.DocumentElement;
            Initialiser(racine);
            
        }

        /// <summary>
        /// à partir de la collection de noeuds XML lesDossiers,
        /// construit une collection d'objets de la classe Dossier.
        /// Pour chaque dossier, ses prestations et les intervenants des prestations
        /// </summary>
        /// <returns>une collection d'objets de la classe Dossier</returns>
        public static List<Dossier> XmlToDossiers()
        {
            List<Dossier> lesDossiers = new List<Dossier>();
            foreach (XmlElement unDossier in LesDossiers)
            {
                Dossier dossier = XmlToDossier(unDossier);
                lesDossiers.Add(dossier);
            }
            return lesDossiers;
        }
        /// <summary>
        /// construit, à partir d'un noeud XML un objet de la classe Dossier.
        /// Avec, ses prestations et les intervenants des prestations
        /// </summary>
        /// <param name="unDossierXML">Elément XML du dossier à créer</param>
        /// <returns>Un objet de la classe Dossier</returns>
        public static Dossier XmlToDossier(XmlElement unDossierXML)
        {
            string nom = unDossierXML.ChildNodes[0].InnerText;
            string prenom = unDossierXML.ChildNodes[1].InnerText;
            DateTime dateNaissance = TraitementXML.XmlToDateTime((XmlElement)unDossierXML.ChildNodes[2]);
            if (unDossierXML.GetElementsByTagName("dossierprestations").Count == 0){

                // pas de prestation
                return new Dossier(nom, prenom, dateNaissance);
            }
            else
            {
				// au moins une prestation
                XmlNodeList lesPrestations = (unDossierXML.GetElementsByTagName("dossierprestations")[0]).ChildNodes;
                List<Prestation> lesPrestationsDuDossier = new List<Prestation>();
                foreach (XmlElement unePrestation in lesPrestations)
                {
                    int idPrestation = Convert.ToInt32(unePrestation.GetAttribute("idprestation"));
                    XmlElement unePrestationXML = CherchePrestation(idPrestation);
                    Prestation prestation = XmlToPrestation(unePrestationXML);
                    lesPrestationsDuDossier.Add(prestation);
                }
                return new Dossier(nom, prenom, dateNaissance, lesPrestationsDuDossier);

            }
        }

        /// <summary>
        /// construit, à partir d'un noeud XML un objet de la classe Prestation.
        /// Avec son Intervenant
        /// </summary>
        /// <param name="unePrestationXML">Elément XML de la prestation à créer</param>
        /// <returns>Un objet de la classe Prestation</returns>
        private static Prestation XmlToPrestation(XmlElement unePrestationXML)
        {
            string libellePrestation = unePrestationXML.ChildNodes[0].InnerText;
            DateTime datePrestation = TraitementXML.XmlToDateTime((XmlElement)unePrestationXML.ChildNodes[1]);
            int idIntervenant = Convert.ToInt32(unePrestationXML.GetAttribute("idintervenant"));
            XmlElement unIntervenantXML = ChercheIntervenant(idIntervenant);
            Intervenant unIntervenant = TraitementXML.XmlToIntervenant(unIntervenantXML);

            return new Prestation(libellePrestation, datePrestation, unIntervenant);
        }

        /// <summary>
        /// construit, à partir d'un noeud XML un objet de la classe Intervenant qui 
        /// peut être un intervenant ou un intervenant externe.
        /// </summary>
        /// <param name="unIntervenantXML">Elément XML de l'intervenant à créer</param>
        /// <returns>Un objet de la classe Dossier</returns>
        private static Intervenant XmlToIntervenant(XmlElement unIntervenantXML)
        {
            string nomIntervenant = unIntervenantXML.ChildNodes[0].InnerText;
            string prenomIntervenant = unIntervenantXML.ChildNodes[1].InnerText;
            if (unIntervenantXML.ChildNodes.Count == 2)
            {
				// ce n'est pas un spécialiste
                return new Intervenant(nomIntervenant, prenomIntervenant);
            }
            else
            {
				// c'est un spécialiste
                string specialiteIntervenant = unIntervenantXML.ChildNodes[2].InnerText;
                string adresseIntervenant = unIntervenantXML.ChildNodes[3].InnerText;
                string telIntervenant = unIntervenantXML.ChildNodes[4].InnerText;
                return new IntervenantExterne(nomIntervenant, prenomIntervenant, specialiteIntervenant, adresseIntervenant, telIntervenant);
            }
        }
        /// <summary>
        /// à partir de la collection de noeuds XML lesIntervenants, 
        /// construit une collection d'objets de la classe Intervenant.
        /// </summary>
        /// <returns>une collection d'objets de la classe Intervenant</returns>
        public static List<Intervenant> XmlToIntervenants()
        {
            List<Intervenant> lesIntervenants = new List<Intervenant>();
            foreach (XmlElement unIntervenantXml in TraitementXML.LesIntervenants)
            {
                lesIntervenants.Add(XmlToIntervenant(unIntervenantXml));   
            }
            return lesIntervenants;
        }
        /// <summary>
        /// construit, à partir d'un noeud XML un objet de la classe Intervenant
        /// Avec ses prestations 
        /// </summary>
        /// <param name="unIntervenantXml">Elément XML de l'intervenant à créer</param>
        /// <returns>Un objet de la classe Intervenant</returns>
        public static Intervenant XmlToIntervenantComplet(XmlElement unIntervenantXml)
        {
            Intervenant unIntervenant = XmlToIntervenant(unIntervenantXml);
            int idIntervenant = Convert.ToInt16(unIntervenantXml.GetAttribute("idintervenant"));
            foreach(XmlElement unePrestationXml in LesPrestations)
            {
                if (Convert.ToInt32(unePrestationXml.GetAttribute("idintervenant")) == idIntervenant)
                {
                    unIntervenant.AjoutePrestation(XmlToPrestation(unePrestationXml));
                }
            }
            return unIntervenant;
        }
        /// <summary>
        /// Construit, à partir d'un noeud XML un objet de la classe DateTime
        /// avec ous sans les heures.
        /// </summary>
        /// <param name="uneDateTimeXml">Elément XML contenant les données sur la date à créer</param>
        /// <returns>Un objet de la classe DateTime</returns>
        private static DateTime XmlToDateTime(XmlElement uneDateTimeXml)
        {
            //XmlElement datenaissanceXml = (XmlElement)unDossier.ChildNodes[2];
            int annee = Convert.ToInt16(uneDateTimeXml.GetElementsByTagName("yyyy")[0].InnerText);
            int mois = Convert.ToInt16(uneDateTimeXml.GetElementsByTagName("mm")[0].InnerText);
            int jour = Convert.ToInt16(uneDateTimeXml.GetElementsByTagName("dd")[0].InnerText);
            int heurePrestation = ((uneDateTimeXml.GetElementsByTagName("hh")).Count == 0) ? (0) : (Convert.ToInt16(uneDateTimeXml.GetElementsByTagName("hh")[0].InnerText));
            int minutePrestation = ((uneDateTimeXml.GetElementsByTagName("mi")).Count == 0) ? (0) : (Convert.ToInt16(uneDateTimeXml.GetElementsByTagName("mi")[0].InnerText));
            return new DateTime(annee, mois, jour,heurePrestation, minutePrestation,0);
        }
        /// <summary>
        /// cherche et retourne un élément correspondant à une prestation dont l'ID passé en paramètre
        /// </summary>
        /// <param name="idPrestation">ID de la prestation cherchée</param>
        /// <returns>un élément XML contenant les données de la prestation</returns>
        private static XmlElement CherchePrestation(int idPrestation)
        {            
            int i = 0;
            while (Convert.ToInt16(((XmlElement)lesPrestations[i]).GetAttribute("idprestation"))!=idPrestation && i < LesPrestations.Count )
            {
                i++;
                
            }
            if (Convert.ToInt16(((XmlElement)lesPrestations[i]).GetAttribute("idprestation")) == idPrestation)
            {
                return (XmlElement)lesPrestations[i];
            }
            else
            {
                throw new Exception("Prestation non trouvée, arrêt du traitement");
            }                
        }
        /// <summary>
        /// cherche et retourne un élément correspondant à un intervenant dont l'ID passé en paramètre
        /// </summary>
        /// <param name="idIntervenant">ID de l'intervenant cherché</param>
        /// <returns>un élément XML contenant les données de l'intervenant</returns>
        private static XmlElement ChercheIntervenant(int idIntervenant)
        {
            int i = 0;
            while (Convert.ToInt16(((XmlElement)LesIntervenants[i]).GetAttribute("idintervenant")) != idIntervenant && i < LesIntervenants.Count)
            {
                i++;
            }
            if (Convert.ToInt16(((XmlElement)LesIntervenants[i]).GetAttribute("idintervenant")) == idIntervenant)
            {
                return (XmlElement)LesIntervenants[i];
            }
            else
            {
                throw new Exception("Intervenant non trouvé, arrêt du traitement");
            }
        }
        /// <summary>
        /// Affiche de façon formatée les données des dossiers contenus dans une liste de dossiers
        /// Cette méthode affiche les dossiers compets avec les prestations et intervenants.
        /// </summary>
        /// <param name="lesDossiers">Les Dossiers à afficher</param>
        public static void AfficherDossiers(List<Dossier> lesDossiers)
        {
            foreach (Dossier unDossier in lesDossiers)
            {
                TraitementXML.AfficherDossier(unDossier);
            }

        }
        /// <summary>
        /// Affiche de façon formatée les données des intervenants et intervenants externes
        ///  contenus dans une liste d'intervenants.
        /// Cette méthode affiche les intervenants avec les prestations effectuées.
        /// </summary>
        /// <param name="lesIntervenants"></param>
        public static void AfficherIntervenants(List<Intervenant> lesIntervenants)
        {
            foreach (Intervenant unIntervenant in lesIntervenants)
            {
                Console.WriteLine(unIntervenant.AfficheIntervenantComplet() + "\n");
            }
        }
        /// <summary>
        /// Affiche de façon formatée les données d'un dossier
        /// Cette méthode affiche les dossiers compets avec les prestations et intervenants.
        /// </summary>
        /// <param name="unDossier">Le Dossier à afficher</param>
        public static void AfficherDossier(Dossier unDossier)
        {
            Console.WriteLine("----- Début dossier --------------");
            Console.WriteLine("Nom : " + unDossier.NomPatient 
                + "\nPrenom : " + unDossier.PrenomPatient 
                + "\nDate de naissance : " + unDossier.DateNaissancePatient.ToShortDateString());
            Console.WriteLine("\tNombre de prestations : " + unDossier.MesPrestations.Count);
            if (unDossier.MesPrestations.Count > 0)
            {
                foreach (Prestation unePrestation in unDossier.MesPrestations)
                {
                    Console.WriteLine("\t" + unePrestation.Libelle 
                        + " - " + unePrestation.DateSoin.ToString() 
                        + " - " + unePrestation.L_Intervenant);
                }
                Console.WriteLine("nombre de jours de soins : " + unDossier.GetNbJoursSoin());
                Console.WriteLine("nombre de prestations externes : " + unDossier.GetNbPrestationsExternes());

            }
            Console.WriteLine("---   Fin du dossier   ---\n");
        }

    }
}
