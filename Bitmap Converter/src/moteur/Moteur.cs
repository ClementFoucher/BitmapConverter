/*
 * Copyright � 2005 ; 2011 ; 2015 Cl�ment Foucher
 *
 * Distributed under the GNU GPL v2. For full terms see the file LICENSE.txt.
 *
 *
 * This file is part of Bitmap Converter.
 *
 * Bitmap Converter is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, version 2 of the License.
 *
 * Bitmap Converter is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Bitmap Converter. If not, see <http://www.gnu.org/licenses/>.
 */

using System.Windows.Forms;
using System.Threading;
using System.Collections;

namespace Bitmap_Converter
{
    public delegate void D�marrerConversionEventHandler(ArrayList listeDesObjetsSelectionn�s);
    public delegate void InterrompreConversionEventHandler();

    /// <summary>
    /// Le moteur centralise les �changes entre les dif�rents threads.
    /// Il AGIT sur les threads, et recoit de EVENEMENTS de leur part.
    /// </summary>
    public sealed class Moteur
    {
        private Thread threadConversion;
        private Fen�trePrincipale fen�tre;
        private Convertisseur convertisseur;
        private Erreurs erreurs;

        /// <summary>
        /// Initialisation du moteur : on
        /// ouvre la fen�tre d'options puis
        /// on lance la fen�tre principale.
        /// 
        /// Comme c'est le moteur qui g�re la boucle d'application,
        /// on peut se permettre de faire Exit() si n�cessaire.
        /// </summary>
        public Moteur()
        {
            Fen�treOptions f = new Fen�treOptions(true);

            DialogResult d = f.ShowDialog();

            f.Dispose();

            if (d == DialogResult.OK)
            {
                this.erreurs = new Erreurs();

                this.fen�tre = new Fen�trePrincipale(this.erreurs);

                // Abonnement aux �v�nements fournis par l'IU
                this.fen�tre.D�marrerConversion    += Convertir;
                this.fen�tre.InterrompreConversion += Interrompre;
                
                Application.Run(this.fen�tre);
            }
            else
                Application.Exit();
        }

        /// <summary>
        /// L'utilisateur � lanc� le processus de conversion
        /// </summary>
        /// <param name="listeDesObjetsSelectionn�s"></param>
        private void Convertir(ArrayList listeDesObjetsSelectionn�s)
        {
            erreurs.Reset();
            convertisseur = new Convertisseur(this.erreurs, listeDesObjetsSelectionn�s);

            // Abonnement aux �v�nements fournis par le thread de conversion
            convertisseur.D�marrageProcessus += this.D�marrageProcessus;
            convertisseur.ProcessusTermin� += this.ProcessusTermin�;

            this.threadConversion = new Thread(new ThreadStart(convertisseur.Convertir));
            this.threadConversion.Start();
        }

        /// <summary>
        /// Interruption de la conversion par l'utilisateur
        /// </summary>
        private void Interrompre()
        {
            this.convertisseur.Interrompre();
        }

        private delegate void InvoquerLesEspritsDeLaFen�trePrincipale();
        private delegate void InvoquerLesEspritsAvecValeur(int i);

        private void D�marrageProcessus()
        {
            convertisseur.D�marrageProcessus -= this.D�marrageProcessus;

            convertisseur.D�marrageConversion += this.PassageEnModeConversion;

            // Ev�nement d�clench� par un thread diff�rent: invoquer
            this.fen�tre.Invoke(new InvoquerLesEspritsDeLaFen�trePrincipale(this.fen�tre.ProcessusD�marr�));
        }

        private void PassageEnModeConversion(int nombre)
        {
            convertisseur.D�marrageConversion -= this.PassageEnModeConversion;

            convertisseur.FichierConverti += this.AvancerBarreDeProgression;
            if (Properties.Settings.Default.DeleteSources == true)
                convertisseur.D�marrageSuppression += this.PassageEnModeSuppression;

            // Ev�nement d�clench� par un thread diff�rent: invoquer
            this.fen�tre.Invoke(new InvoquerLesEspritsAvecValeur(this.fen�tre.PasserEnModeConversion), new object[] { nombre });
        }

        private void PassageEnModeSuppression(int nombre)
        {
            convertisseur.D�marrageSuppression -= this.PassageEnModeSuppression;
            convertisseur.FichierConverti -= this.AvancerBarreDeProgression;

            convertisseur.FichierSupprim� += this.AvancerBarreDeProgression;

            // Ev�nement d�clench� par un thread diff�rent: invoquer
            this.fen�tre.Invoke(new InvoquerLesEspritsAvecValeur(this.fen�tre.PasserEnModeSuppression), new object[] { nombre });
        }

        private void ProcessusTermin�()
        {
            // Ev�nement d�clench� par un thread diff�rent: invoquer
            this.fen�tre.Invoke(new InvoquerLesEspritsDeLaFen�trePrincipale(this.fen�tre.ProcessusTermin�));

            threadConversion = null;
            convertisseur    = null;
        }

        private void AvancerBarreDeProgression()
        {
            // Ev�nement d�clench� par un thread diff�rent: invoquer
            this.fen�tre.Invoke(new InvoquerLesEspritsDeLaFen�trePrincipale(this.fen�tre.AvancerDUnCran));
        }

    }
}
