using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classesMetier
{
    class Dossier
    {
        private string nomPatient;
        private string prenomPatient;
        private DateTime dateNaissancePatient;
        private List<Prestation> mesPrestations; //cette collection est initialement dans un ordre quelconque

        public List<Prestation> MesPrestations { get => mesPrestations; }
        public string NomPatient { get => nomPatient; set => nomPatient = value; }
        public string PrenomPatient { get => prenomPatient; set => prenomPatient = value; }
        public DateTime DateNaissancePatient { get => dateNaissancePatient; set => dateNaissancePatient = value; }
        internal List<Prestation> MesPrestations1 { get => mesPrestations; set => mesPrestations = value; }

        /// <summary>
        /// Constructeur de la classe Dossier
        /// </summary>
        /// <param name="nomPatient"></param>
        /// <param name="prenomPatient"></param>
        /// <param name="dateNaissancePatient"></param>
        public Dossier(string nomPatient, string prenomPatient, DateTime dateNaissancePatient)
        {
            this.nomPatient = nomPatient;
            this.prenomPatient = prenomPatient;
            this.dateNaissancePatient = dateNaissancePatient;
            this.mesPrestations = new List<Prestation>();
        }
        public Dossier(string nomPatient, string prenomPatient, DateTime dateNaissancePatient, Prestation prestation)
            : this(nomPatient, prenomPatient, dateNaissancePatient)
        {
            this.mesPrestations.Add(prestation);
        }
        public Dossier(string nomPatient, string prenomPatient, DateTime dateNaissancePatient, List<Prestation> mesPrestations) 
            : this(nomPatient, prenomPatient, dateNaissancePatient)
        {
            this.mesPrestations = mesPrestations;
        }


        /// <summary>
        /// Permet d'ajouter une prestation au Dossier
        /// </summary>
        /// <param name="unLibelle"></param>
        /// <param name="uneDate"></param>
        /// <param name="uneHeure"></param>
        /// <param name="unIntervenant"></param>
        public void AjoutePrestation(string unLibelle, DateTime uneDate, Intervenant unIntervenant)
        {
            Prestation prestation = new Prestation(unLibelle, uneDate, unIntervenant);
            this.mesPrestations.Add(prestation);
        }

        /// <summary>
        /// Retourne le nombre de prestations réalisées pour le dossier
        /// </summary>
        /// <returns></returns>
        public int GetNbPrestationsExternes()
        {
            int nb = 0;
            foreach (Prestation prestation in this.mesPrestations)
            {
                if (prestation.L_Intervenant is IntervenantExterne)
                {
                    nb += 1;
                }
            }
            return nb;
        }

        /// <summary>
        /// Retourne le nombre de jours de soins comptabilisés pour le dossier.
        /// Il ne s'agit pas ici de déterminer le nombre de prestations attachées à un dossier,
        /// mais le nombre de jours pour lesquels au moins une prestation a été réalisée
        /// </summary>
        /// <returns></returns>
        public int GetNbJoursSoin()
        {
            if (this.mesPrestations.Count == 0)
            {
                return 0;
            }
            int cpt = 1;
            this.Trier();
            Prestation unePrestation = this.mesPrestations.ElementAt(0);
            foreach (Prestation prestation in this.mesPrestations)
            {
                cpt += prestation.CompareTo(unePrestation);
                unePrestation = prestation;
            }
            return cpt;
        }
        /// <summary>
        /// Fonction permettant de trier dans l'ordre chronologique
        /// la liste mesPrestations
        /// </summary>
        private void Trier()
        {

            List<Prestation> prestaTrie = new List<Prestation>();
            List<Prestation> prestations = this.mesPrestations;
            while (prestations.Count != 0)
            {
                Prestation unePrestation = prestations.ElementAt(0);
                int index = 0;
                for (int i = 0; i < prestations.Count; i++)
                {
                    if (prestations.ElementAt(i).CompareTo(unePrestation) == -1)
                    {
                        unePrestation = prestations.ElementAt(i);
                        index = i;
                    }
                }
                prestations.RemoveAt(index);
                prestaTrie.Add(unePrestation);
            }
            this.mesPrestations = prestaTrie;
        }

        /// <summary>
        /// Autre alternative à GetNbJoursSoins
        /// </summary>
        /// <returns></returns>
        public int GetNbJoursSoinsV2()
        {
            List<Prestation> prestations = this.mesPrestations;
            for (int i = 0; i < prestations.Count - 1; i++)
            {
                Prestation unePresta = prestations.ElementAt(i);
                for (int y = i + 1; y < prestations.Count; y++)
                {
                    Prestation maPresta = prestations.ElementAt(y);
                    if (maPresta.CompareTo(unePresta) == 0)
                    {
                        prestations.RemoveAt(y);
                        y -= 1;
                    }
                }
                {
                }
            }
            return prestations.Count();
        }

        public int GetNbJoursSoinsV99()
        {
            int cpt = 0;
            bool test;

            for (int i = 0; i < this.mesPrestations.Count; i++)
            {
                test = true;
                for (int y = i + 1; y < this.mesPrestations.Count; y++)
                {
                    if (this.mesPrestations.ElementAt(i).CompareTo(this.mesPrestations.ElementAt(y)) == 0)
                    {
                        test = false;
                    }
                }
                if (test)
                {
                    cpt++;
                }
            }
            return cpt;
        }
    }
}
