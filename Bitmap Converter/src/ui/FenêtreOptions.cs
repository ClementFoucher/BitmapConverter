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
using System.IO;

namespace Bitmap_Converter
{
    public partial class Fen�treOptions : Form
    {
        private Param�tres param�tres;

        /// <summary>
        /// Constructeur par d�faut : appell� en d�but de programme,
        /// il initialise un nouvel objet Param�tres.
        /// </summary>
        public Fen�treOptions()
        {
            InitializeComponent();

            this.Text = "Entrez les param�tre de la conversion";
            annuler.Text = "Quitter";

            param�tres = new Param�tres();

            Initialiser();
        }

        /// <summary>
        /// Ce contructeur est appell� en cours de programme, afin
        /// de modifier une option.
        /// </summary>
        /// <param name="p">Les param�tres en cours.</param>
        public Fen�treOptions(Param�tres p)
        {
            InitializeComponent();

            param�tres = p;

            Initialiser();
        }

        private void Initialiser()
        {
            formatDEntree.Items.Add(Format.Bmp);
            formatDEntree.Items.Add(Format.Gif);
            formatDEntree.Items.Add(Format.Jpeg);
            formatDEntree.Items.Add(Format.Png);
            formatDEntree.Items.Add(Format.Tiff);
            formatDEntree.Items.Add(Format.Wmf);

            formatSortie.Items.Add(Format.Bmp);
            formatSortie.Items.Add(Format.Gif);
            formatSortie.Items.Add(Format.Jpeg);
            formatSortie.Items.Add(Format.Png);
            formatSortie.Items.Add(Format.Tiff);
            formatSortie.Items.Add(Format.Wmf);

            // Ne SURTOUT PAS modifier l'ordre des 2 commandes ci-dessous !
            formatSortie.SelectedItem = param�tres.formatDeSortie;
            formatDEntree.SelectedItem = param�tres.formatDEntr�e;
            
            inclureSousRepertoires.Checked = param�tres.inclureLesSousR�pertoires;
            supprimerSources.Checked = param�tres.supprimerLesSources;
            chemin.Text = param�tres.dossierCourant;
        } // Fin de la m�thode initialiser

        private void Parcourir(object sender, EventArgs e)
        {
            FolderBrowserDialog boiteParcourir = new FolderBrowserDialog();
            boiteParcourir.SelectedPath = chemin.Text;
            boiteParcourir.Description = "S�lectionnez le dossier contenant les fichiers que vous d�sirez convertir :";
            DialogResult r�sultat = boiteParcourir.ShowDialog();
            if (r�sultat == DialogResult.OK)
            {
                chemin.Text = boiteParcourir.SelectedPath;
            }
            boiteParcourir.Dispose();
        } // Fin de la m�thode parcourir

        private void ok_Click(object sender, EventArgs e)
        {
            // Tester la validit� du dossierCourant avant d'accepter le clic sur ok
            // + tester le format (retour aux bonnes vielles m�thodes ?). sinon :
            //if (formatDEntree.SelectedItem == formatSortie.SelectedItem) gnagnagna...

            if (Directory.Exists(chemin.Text)) param�tres.dossierCourant = chemin.Text;
            else
            {
                MessageBox.Show("Le chemin que vous avez entr� n'est pas correct. V�rifiez que le r�pertoire existe.");
                return;
            }
            param�tres.formatDEntr�e = (Format) formatDEntree.SelectedItem;
            param�tres.formatDeSortie = (Format) formatSortie.SelectedItem;
            param�tres.inclureLesSousR�pertoires = inclureSousRepertoires.Checked;
            param�tres.supprimerLesSources = supprimerSources.Checked;
        }

        private void chemin_TextChanged(object sender, EventArgs e)
        {
            param�tres.dossierCourant = chemin.Text;
        }

        public Param�tres Param�tres
        {
            get
            {
                return param�tres;
            }
        }

        private void formatDEntree_SelectedIndexChanged(object sender, EventArgs e)
        {
            Format sortie = (Format)formatSortie.SelectedItem;
            formatSortie.Items.Clear();

            if ((Format) formatDEntree.SelectedItem != Format.Bmp) formatSortie.Items.Add(Format.Bmp);
            if ((Format)formatDEntree.SelectedItem != Format.Gif) formatSortie.Items.Add(Format.Gif);
            if ((Format)formatDEntree.SelectedItem != Format.Jpeg) formatSortie.Items.Add(Format.Jpeg);
            if ((Format)formatDEntree.SelectedItem != Format.Png) formatSortie.Items.Add(Format.Png);
            if ((Format)formatDEntree.SelectedItem != Format.Tiff) formatSortie.Items.Add(Format.Tiff);
            if ((Format)formatDEntree.SelectedItem != Format.Wmf) formatSortie.Items.Add(Format.Wmf);

            if (sortie != (Format) formatDEntree.SelectedItem) formatSortie.SelectedItem = sortie;
            else if ((Format) formatDEntree.SelectedItem != Format.Jpeg) formatSortie.SelectedItem = Format.Jpeg;
            else formatSortie.SelectedItem = Format.Bmp;
        }
    }
}