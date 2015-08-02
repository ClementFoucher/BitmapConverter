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

// Initial header:

/************************************/
/*      Programm� par : Epok__      */
/*        Date : 22/07/2005         */
/*          Heure : 17:13           */
/*                                  */
/* Edit� le 22/07/2015 par hasard ! */
/* Bon anniversaire Bitmap Converter*/
/*                                  */
/*          Version : 1.2           */
/************************************/
/*  Classe permettant de convertir	*/
/* des s�ries d'images d'un format	*/
/*		   bitmap � un autre		*/
/************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Bitmap_Converter
{

    public partial class Fen�trePrincipale : Form
    {
        private Erreurs erreurs;

        public event D�marrerConversionEventHandler    D�marrerConversion;
        public event InterrompreConversionEventHandler InterrompreConversion;

        public Fen�trePrincipale(Erreurs erreurs)
        {
            this.erreurs = erreurs;

            InitializeComponent();

            barreDeProgression.Minimum = 0;
            barreDeProgression.Step = 1;

            ActualiserListe();
        }

        // Public

        public void ProcessusD�marr�()
        {
            liste.Enabled = false;
            s�lectionnerTout.Enabled = false;
            actualiser.Enabled = false;
            convertir.Enabled = false;
            barreDeMenus.Enabled = false;

            interrompre.Visible = true;
        }

        public void PasserEnModeConversion(int nombre)
        {
            texteDEtat.Text = "Conversion en cours...";

            barreDeProgression.Value = 0;
            barreDeProgression.Maximum = nombre;            
        }

        public void PasserEnModeSuppression(int nombreObjetsASupprimer)
        {
            texteDEtat.Text = "Suppression des fichiers sources en cours...";

            barreDeProgression.Value = 0;
            barreDeProgression.Maximum = nombreObjetsASupprimer;
        }

        public void ProcessusTermin�()
        {
            texteDEtat.Text = "Conversion Termin�e";

            liste.Enabled = true;
            s�lectionnerTout.Enabled = true;
            actualiser.Enabled = true;
            convertir.Enabled = true;
            barreDeMenus.Enabled = true;

            interrompre.Visible = false;

            AfficherLesErreurs();
        }

        public void AvancerDUnCran()
        {
            barreDeProgression.PerformStep();
        }


        // Priv�

        private void ActualiserListe()
        {
            texteDEtat.Text = "En attente pour la conversion...";
            barreDeProgression.Value = 0;

            liste.Items.Clear();
            SortedList<int, string> nouvelleListe = G�n�rateurDeListes.G�n�rerListe();
            foreach (string s in nouvelleListe.Values)
            {
                liste.Items.Add(s);
            }
        }

        private void AfficherLesErreurs()
        {
            if (this.erreurs.erreur)
                (new Fen�treErreurs(this.erreurs)).ShowDialog();
            else
                MessageBox.Show("Pas d'erreurs lors de la conversion.", "R�sultat de la conversion");
        }

        private void UserRequest_S�lectionnerTout(object sender, EventArgs e)
        {
            for (int i = 0; i < liste.Items.Count; i++)
            {
                liste.SetSelected(i, true);
            }
        }

        private void UserRequest_D�s�lectionnerTout(object sender, EventArgs e)
        {
            for (int i = 0; i < liste.Items.Count; i++)
            {
                liste.SetSelected(i, false);
            }
        }

        private void UserRequest_AfficherAProposDe(object sender, EventArgs e)
        {
            (new APropos()).ShowDialog();
        }

        private void UserRequest_OuvrirOptions(object sender, EventArgs e)
        {
            Fen�treOptions f = new Fen�treOptions();

            DialogResult d = f.ShowDialog();

            if (d == DialogResult.OK)
            {
                f.Dispose();
                ActualiserListe();
            }
        }

        private void UserRequest_AfficherErreurs(object sender, EventArgs e)
        {
            AfficherLesErreurs();
        }

        private void UserRequest_Interrompre(object sender, EventArgs e)
        {
            InterrompreConversion();
        }

        private void UserRequest_Convertir(object sender, EventArgs e)
        {
            ArrayList listeConv = new ArrayList();

            foreach (string s in liste.SelectedItems)
            {
                listeConv.Add(s);
            }

            D�marrerConversion(listeConv);

        }

        private void UserRequest_Actualiser(object sender, EventArgs e)
        {
            ActualiserListe();
        }
    }
}
