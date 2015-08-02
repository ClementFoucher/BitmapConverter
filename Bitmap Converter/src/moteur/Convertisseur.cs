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
    public delegate void D�marrageProcessusEventHandler();

    public delegate void D�marrageConversionEventHandler(int nombre);
    public delegate void FichierConvertiEventHandler();
    public delegate void ConversionTermin�eEventHandler();

    public delegate void D�marrageSuppressionEventHandler(int nombre);
    public delegate void FichierSupprim�EventHandler();
    public delegate void SuppressionTermin�eEventHandler();

    public delegate void ProcessusTermin�EventHandler();
    
    sealed class Convertisseur
    {
        public event D�marrageProcessusEventHandler   D�marrageProcessus;
        public event D�marrageConversionEventHandler  D�marrageConversion;
        public event FichierConvertiEventHandler      FichierConverti;
        public event ConversionTermin�eEventHandler   ConversionTermin�e;
        public event D�marrageSuppressionEventHandler D�marrageSuppression;
        public event FichierSupprim�EventHandler      FichierSupprim�;
        public event SuppressionTermin�eEventHandler  SuppressionTermin�e;
        public event ProcessusTermin�EventHandler     ProcessusTermin�;

        private Erreurs erreurs;

        private ArrayList
            listeDesObjetsATraiter,
            listeErron�s,
            listeNonSupprim�s;

        private bool continuer;

        public Convertisseur(Erreurs erreurs, ArrayList liste)
        {
            this.listeDesObjetsATraiter = liste;
            this.erreurs = erreurs;

            this.listeErron�s = new ArrayList();
            this.listeNonSupprim�s = new ArrayList();
            this.continuer = true;
        }

        /// <summary>
        /// Convertir les fichiers selon les param�tres.
        /// Il s'agit de la boucle principale du thread.
        /// </summary>
        public void Convertir()
        {
            // Event
            if (D�marrageProcessus != null)
                D�marrageProcessus();

            // Event
            if (D�marrageConversion != null)
                D�marrageConversion(this.listeDesObjetsATraiter.Count);

            foreach (string s in this.listeDesObjetsATraiter)
            {
                if (continuer == true)
                {
                    try
                    {
                        string nom = "";

                        Bitmap image = new Bitmap(s);

                        if (Properties.Settings.Default.TargetType == Format.Bmp.ToString())
                        {
                            nom = s.Replace(Path.GetExtension(s), ".bmp");
                            if (!File.Exists(nom))
                                image.Save(nom, System.Drawing.Imaging.ImageFormat.Bmp);
                            else
                                this.listeErron�s.Add(s);
                        }
                        else if (Properties.Settings.Default.TargetType == Format.Gif.ToString())
                        {
                            nom = s.Replace(Path.GetExtension(s), ".gif");
                            if (!File.Exists(nom))
                                image.Save(nom, System.Drawing.Imaging.ImageFormat.Gif);
                            else
                                this.listeErron�s.Add(s);
                        }
                        else if (Properties.Settings.Default.TargetType == Format.Jpeg.ToString())
                        {
                            nom = s.Replace(Path.GetExtension(s), ".jpg");
                            if (!File.Exists(nom))
                                image.Save(nom, System.Drawing.Imaging.ImageFormat.Jpeg);
                            else
                                this.listeErron�s.Add(s);
                        }
                        else if (Properties.Settings.Default.TargetType == Format.Png.ToString())
                        {
                            nom = s.Replace(Path.GetExtension(s), ".png");
                            if (!File.Exists(nom))
                                image.Save(nom, System.Drawing.Imaging.ImageFormat.Png);
                            else
                                this.listeErron�s.Add(s);
                        }
                        else if (Properties.Settings.Default.TargetType == Format.Tiff.ToString())
                        {
                            nom = s.Replace(Path.GetExtension(s), ".tif");
                            if (!File.Exists(nom))
                                image.Save(nom, System.Drawing.Imaging.ImageFormat.Tiff);
                            else
                                this.listeErron�s.Add(s);
                        }
                        else if (Properties.Settings.Default.TargetType == Format.Wmf.ToString())
                        {
                            nom = s.Replace(Path.GetExtension(s), ".wmf");
                            if (!File.Exists(nom))
                                image.Save(nom, System.Drawing.Imaging.ImageFormat.Wmf);
                            else
                                this.listeErron�s.Add(s);
                        }
                        else // Si proprit�t� mal initialis�e
                            this.listeErron�s.Add(s);

                        image.Dispose();
                    }
                    catch
                    {
                        this.listeErron�s.Add(s);
                    }

                    // Event
                    if (FichierConverti != null)
                        FichierConverti();
                }
                else
                    break;
            }

            this.erreurs.erron�s = this.listeErron�s;

            // Event
            if (ConversionTermin�e != null)
                ConversionTermin�e();

            if ((Properties.Settings.Default.DeleteSources == true) &&  (continuer == true))
            {
                // Event
                if (D�marrageSuppression != null)
                    D�marrageSuppression(this.listeDesObjetsATraiter.Count - this.listeErron�s.Count);

                foreach (string s in this.listeDesObjetsATraiter)
                {
                    if (continuer == true)
                    {
                        try
                        {
                            if (!this.listeErron�s.Contains(s))
                                File.Delete(s);
                        }
                        catch
                        {
                            this.listeNonSupprim�s.Add(s);
                        }

                        // Event
                        if (FichierSupprim� != null)
                            FichierSupprim�();
                    }
                    else
                        break;
                }

                this.erreurs.nonSupprim�s = this.listeNonSupprim�s;

                // Event
                if (SuppressionTermin�e != null)
                    SuppressionTermin�e();
            }

            if ((this.listeErron�s.Count != 0) || (this.listeNonSupprim�s.Count != 0))
                this.erreurs.erreur = true;
            else
                this.erreurs.erreur = false;

            // Event
            if (ProcessusTermin� != null)
                ProcessusTermin�();
        }

        /// <summary>
        /// Interrompt le processus proprement
        /// </summary>
        public void Interrompre()
        {
            this.continuer = false;
        }

    }
}
