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
using System.IO;

namespace Bitmap_Converter
{
    class OperationsSurLaListe
    {
        private Param�tres param�tres;
        private ListBox.ObjectCollection listeFichiers;  // Changer le type de stockage

        public OperationsSurLaListe(Param�tres p)
        {
            param�tres = p;
        }

        /// <summary>
        /// Retourne la liste des fichiers contenus dans
        /// le r�p�rtoire courant selont les param�tres.
        /// </summary>
        /// <returns></returns>
        public ListBox.ObjectCollection GetListe()
        {
            listeFichiers = (new ListBox()).Items;
            Cr�erListe(param�tres.dossierCourant);
            return listeFichiers;
        }

        /// <summary>
        /// Cr�er la liste des fichiers en fonction des param�tres.
        /// </summary>
        /// <param name="dossierCourant"></param>
        private void Cr�erListe(string chemin)
        {
            DirectoryInfo r�pertoire = new DirectoryInfo(chemin);
            FileInfo[] fichiers = null;

            // Penser � tester la longeur de l'extention � l'aide de :
            // if(Path.GetExtension(fileName).ToString().Length ==3) 

            if (param�tres.formatDEntr�e == Format.Bmp) fichiers = r�pertoire.GetFiles("*.bmp");
            else if (param�tres.formatDEntr�e == Format.Gif) fichiers = r�pertoire.GetFiles("*.gif");
            else if (param�tres.formatDEntr�e == Format.Jpeg)
            {
                FileInfo[][] fichiersJpg = new FileInfo[3][];
                fichiersJpg[0] = r�pertoire.GetFiles("*.jpg");
                fichiersJpg[1] = r�pertoire.GetFiles("*.jpeg");
                fichiersJpg[2] = r�pertoire.GetFiles("*.jpe");

                fichiers = new FileInfo[fichiersJpg[0].Length + fichiersJpg[1].Length + fichiersJpg[2].Length];

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < fichiersJpg[i].Length; j++)
                    {
                        if (i == 0) fichiers[j] = fichiersJpg[i][j];
                        else if (i == 1) fichiers[j + fichiersJpg[0].Length] = fichiersJpg[i][j];
                        else fichiers[j + fichiersJpg[0].Length + fichiersJpg[1].Length] = fichiersJpg[i][j];
                    }
                }
            }
            else if (param�tres.formatDEntr�e == Format.Png) fichiers = r�pertoire.GetFiles("*.png");
            else if (param�tres.formatDEntr�e == Format.Tiff) fichiers = r�pertoire.GetFiles("*.tif");
            else if (param�tres.formatDEntr�e == Format.Wmf) fichiers = r�pertoire.GetFiles("*.wmf");

            for (int i = 0; i < fichiers.Length; i++)
            {
                listeFichiers.Add(fichiers[i].FullName);
            }

            if (param�tres.inclureLesSousR�pertoires == true)
            {
                DirectoryInfo[] sousR�pertoires = r�pertoire.GetDirectories();

                foreach (DirectoryInfo sousR�pertoireCourant in sousR�pertoires)
                {
                    Cr�erListe(sousR�pertoireCourant.FullName);
                }
            }
        }
	}
}
