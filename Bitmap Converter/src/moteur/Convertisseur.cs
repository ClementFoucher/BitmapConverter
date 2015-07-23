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

using System.IO;
using System.Drawing;
using System.Collections;

namespace Bitmap_Converter
{
    public sealed class Convertisseur
    {
        private Moteur moteur;

        private Param�tres param�tres;

        private ArrayList
            listeErron�s,
            listeNonSupprim�s,
            listeDesObjetsATraiter;

        private delegate void InvoquerLesEspritsDeLaFen�trePrincipale();
        private delegate void InvoquerLesEspritsAvecValeur(int i);

        public Convertisseur(Moteur m, ArrayList l)
        {
            listeErron�s = new ArrayList();
            listeNonSupprim�s = new ArrayList();
            listeDesObjetsATraiter = l;
            moteur = m;
            param�tres = m.Param�tres;
        }

        /// <summary>
        /// Convertir les fichiers selon les param�tres.
        /// </summary>
        /// <param name="listeDesObjetsATraiter">Fichiers � convertir</param>
        public void Convertir()
        {
            foreach (string s in listeDesObjetsATraiter)
            {
                try
                {
                    string nom = "";

                    Bitmap image = new Bitmap(s);

					if (param�tres.formatDeSortie == Format.Bmp)
					{
						nom = s.Replace(Path.GetExtension(s), ".bmp");
						if (!File.Exists(nom)) image.Save(nom, System.Drawing.Imaging.ImageFormat.Bmp);
						else listeErron�s.Add(s);
					}
					else if (param�tres.formatDeSortie == Format.Gif)
					{
						nom = s.Replace(Path.GetExtension(s), ".gif");
						if (!File.Exists(nom)) image.Save(nom, System.Drawing.Imaging.ImageFormat.Gif);
						else listeErron�s.Add(s);
					}
					else if (param�tres.formatDeSortie == Format.Jpeg)
					{
						nom = s.Replace(Path.GetExtension(s), ".jpg");
						if (!File.Exists(nom)) image.Save(nom, System.Drawing.Imaging.ImageFormat.Jpeg);
						else listeErron�s.Add(s);
					}
					else if (param�tres.formatDeSortie == Format.Png)
					{
						nom = s.Replace(Path.GetExtension(s), ".png");
						if (!File.Exists(nom)) image.Save(nom, System.Drawing.Imaging.ImageFormat.Png);
						else listeErron�s.Add(s);
					}
					else if (param�tres.formatDeSortie == Format.Tiff)
					{
						nom = s.Replace(Path.GetExtension(s), ".tif");
						if (!File.Exists(nom)) image.Save(nom, System.Drawing.Imaging.ImageFormat.Tiff);
						else listeErron�s.Add(s);
					}
					else if (param�tres.formatDeSortie == Format.Wmf)
					{
						nom = s.Replace(Path.GetExtension(s), ".wmf");
						if (!File.Exists(nom)) image.Save(nom, System.Drawing.Imaging.ImageFormat.Wmf);
						else listeErron�s.Add(s);
					}

                    image.Dispose();
                }
                catch
                {
                    listeErron�s.Add(s);
                }
                moteur.Fen�tre.Invoke(new InvoquerLesEspritsDeLaFen�trePrincipale(moteur.Fen�tre.AvancerDUnCran));
            }

            if (param�tres.supprimerLesSources)
            {
                moteur.Fen�tre.Invoke(new InvoquerLesEspritsAvecValeur(moteur.Fen�tre.PassageEnModeSuppression), new object[] { (listeDesObjetsATraiter.Count - listeErron�s.Count) });

                foreach (string s in listeDesObjetsATraiter)
                {
                    try
                    {
                        if (!listeErron�s.Contains(s)) File.Delete(s);
                    }
                    catch
                    {
                        listeNonSupprim�s.Add(s);
                    }
                    moteur.Fen�tre.Invoke(new InvoquerLesEspritsDeLaFen�trePrincipale(moteur.Fen�tre.AvancerDUnCran));
                }
            }


            moteur.Erreurs.erron�s = listeErron�s;
            moteur.Erreurs.nonSupprim�s = listeNonSupprim�s;
            if ((listeErron�s.Count != 0) || (listeNonSupprim�s.Count != 0)) moteur.Erreurs.erreur = true;
            else moteur.Erreurs.erreur = false;

            moteur.Fen�tre.Invoke(new InvoquerLesEspritsDeLaFen�trePrincipale(moteur.Fen�tre.ConversionTermin�e));

            moteur.Fen�tre.Invoke(new InvoquerLesEspritsDeLaFen�trePrincipale(moteur.Fen�tre.D�bloquer));
        }

    }
}
