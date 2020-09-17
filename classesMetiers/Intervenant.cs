using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classesMetier
{
    class Intervenant
    {
        private string nom;
        private string prenom;
        private List<Prestation> lesPrestations;

        /// <summary>
        /// Constructeur de la classe Intervenant
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        public Intervenant(string nom, string prenom)
        {
            this.nom = nom;
            this.prenom = prenom;
            this.lesPrestations = new List<Prestation>();
        }

        /// <summary>
        /// accesseur de l'attribut nom
        /// </summary>
        public string Nom { get => nom;}
        /// <summary>
        /// accesseur de l'attribut prenom
        /// </summary>
        public string Prenom { get => prenom;}

        /// <summary>
        /// ajoute une Prestation dans la collection lesPrestations
        /// </summary>
        /// <param name="unePrestation"></param>
        public void AjoutePrestation(Prestation unePrestation)
        {
            this.lesPrestations.Add(unePrestation);
        }

        public virtual string AfficheIntervenantComplet()
        {            
            return "nom : "+ this.nom + "\nPrenom : " + this.prenom;
        }
    }
}
