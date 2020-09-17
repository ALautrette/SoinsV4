using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classesMetier
{
    class IntervenantExterne: Intervenant
    {
        private string specialite;
        private string adresse;
        private string tel;       

        /// <summary>
        /// Constructeur de la classe 
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// <param name="specialite"></param>
        /// <param name="adresse"></param>
        /// <param name="tel"></param>
        public IntervenantExterne(string nom, string prenom, string specialite, string adresse, string tel) :base(nom, prenom)
        {
            this.specialite = specialite;
            this.adresse = adresse;
            this.tel = tel;
        }

        public override string AfficheIntervenantComplet()
        {
            return base.AfficheIntervenantComplet() 
                + "\nSpécialité : " + this.specialite 
                + "\nAdresse : " + this.adresse 
                + "\nNuméro de téléphone : " + this.tel;
        }

    }
}
