using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classesMetier
{
    class Prestation
    {
        private string libelle;
        private DateTime dateSoin;
        private Intervenant l_Intervenant;

        /// <summary>
        /// Constructeur de la classe Prestation
        /// </summary>
        /// <param name="libelle"></param>
        /// <param name="dateSoin"></param>
        /// <param name="l_Intervenant"></param>
        public Prestation(string libelle, DateTime dateSoin, Intervenant l_Intervenant)
        {
            this.libelle = libelle;
            this.dateSoin = dateSoin;
            this.l_Intervenant = l_Intervenant;
            l_Intervenant.AjoutePrestation(this);
        }

        public string Libelle { get => libelle; }
        public DateTime DateSoin { get => dateSoin; }
        public Intervenant L_Intervenant { get => l_Intervenant; }

        /// <summary>
        /// Fonction permettant de comparer 2 prestations, la prestation courante et
        /// le paramètre unePrestation
        /// la comparaison porte ici sur la date de la prestation
        /// retourne 0 si la date de la prestation courante est égale à la date de la prestation unePrestation
        /// retourne 1 si la date de la prestation courante est postérieure à la date de la prestation unePrestation
        /// retourne -1 si la date de la prestation courante est antérieure à la date de la prestation unePrestation
        /// </summary>
        /// <param name="unePrestation"></param>
        /// <returns></returns>
        public int CompareTo(Prestation unePrestation)
        {
            //string str = this.dateSoin.ToShortDateString();
            //DateTime date = DateTime.Parse(str);
            //str = unePrestation.DateSoin.ToShortDateString();
            //DateTime dateCompare = DateTime.Parse(str);
            //return date.CompareTo(dateCompare);
            return this.dateSoin.Date.CompareTo(unePrestation.dateSoin.Date);
        }
    }
}
