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

using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections;

namespace Bitmap_Converter
{
    /// <summary>
    /// Moteur centralise les �changes entre les dif�rents threads.
    /// </summary>
    public sealed class Moteur
    {
        private Param�tres param�tres;
        private Fen�trePrincipale fen�tre;
        private Thread threadConversion;
        private Erreurs erreurs;

        /// <summary>
        /// Cr�e un objet moteur initialis� aux
        /// param�tres par d�faut.
        /// </summary>
        public Moteur()
        {
            Initialiser();
        }

        /// <summary>
        /// Initialisation du moteur : on
        /// ouvre la fen�tre d'options puis
        /// on lance la fen�tre principale.
        /// </summary>
        private void Initialiser()
        {
            Fen�treOptions f = new Fen�treOptions();

            DialogResult d = f.ShowDialog();

            if (d == DialogResult.OK)
            {
                param�tres = f.Param�tres;
                erreurs = new Erreurs();
                f.Dispose();

                fen�tre = new Fen�trePrincipale(this);

                Application.Run(fen�tre);
            }

            else Application.Exit();
        }

        /// <summary>
        /// L'utilisateur � lanc� le processus de conversion
        /// </summary>
        /// <param name="listeDesObjetsSelectionn�s"></param>
        public void Convertir(ListBox.SelectedObjectCollection listeDesObjetsSelectionn�s)
        {
            fen�tre.Bloquer();
            // G�rer les acc�s du thread
            Convertisseur conv = new Convertisseur(this, ToListe(listeDesObjetsSelectionn�s));
            threadConversion = new Thread(new ThreadStart(conv.Convertir));
            threadConversion.Start();
        }

        /// <summary>
        /// Conversion vers format de liste ArrayList
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        private ArrayList ToListe(ListBox.SelectedObjectCollection listeIn)
        {
            ArrayList listeOut = new ArrayList();
            foreach (string s in listeIn)
            {
                listeOut.Add(s);
            }
            return listeOut;
        }

        /// <summary>
        /// Afficher la fen�tre des erreurs.
        /// </summary>
        public void AfficherLesErreurs()
        {
            if (erreurs.erreur)
                (new Fen�treErreurs(erreurs)).ShowDialog();
            else MessageBox.Show("Pas d'erreurs lors de la conversion.", "R�sultat de la conversion");
        }

        /// <summary>
        /// Interruption de la conversion par l'utilisateur
        /// </summary>
        public void InterrompreConversion()
        {
            // NE doit pas interrompre le thread n'importe quand
            threadConversion.Interrupt();
        }

        /// <summary>
        /// Afficher la fen�tre de modification des options
        /// </summary>
        public void ModifierOptions()
        {
            Fen�treOptions f = new Fen�treOptions(param�tres);

            DialogResult d = f.ShowDialog();

            if (d == DialogResult.OK)
            {
                param�tres = f.Param�tres;
                f.Dispose();
                fen�tre.Actualiser(new object(), new EventArgs());
            }
        }

        // Pour que les autres objets aient un acc�s direct entre eux
        public Param�tres Param�tres
        {
            get
            {
                return param�tres;
            }
        }
        public Fen�trePrincipale Fen�tre
        {
            get
            {
                return fen�tre;
            } 
        }
        public Erreurs Erreurs
        {
            get
            {
                return erreurs;
            }
        }
    }
}
