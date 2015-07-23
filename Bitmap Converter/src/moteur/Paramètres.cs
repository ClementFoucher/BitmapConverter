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

namespace Bitmap_Converter
{
    /// <summary>
    /// Cette classe regroupe les param�tres qui d�crivent
    /// le comportement du logiciel.
    /// </summary>
    public sealed class Param�tres
    {
        public bool
            inclureLesSousR�pertoires,
            supprimerLesSources;

        public Format
            formatDEntr�e = Format.Bmp,
            formatDeSortie = Format.Jpeg;

        public string
            dossierCourant;

        /// <summary>
        /// Initialise les param�tres par d�faut du logiciel.
        /// </summary>
        public Param�tres()
        {
            inclureLesSousR�pertoires = true;
            supprimerLesSources = false;

            formatDEntr�e = Format.Bmp;
            formatDeSortie = Format.Jpeg;

            dossierCourant = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }

    }
}
