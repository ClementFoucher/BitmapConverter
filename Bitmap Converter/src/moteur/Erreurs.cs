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
 
 using System.Collections;

namespace Bitmap_Converter
{
    /// <summary>
    /// Structure de donn�e utilis�e pour stocker les
    /// erreurs survenues lors de la conversion.
    /// 
    /// Elle est partag�e par tout le monde.
    /// </summary>
    public sealed class Erreurs
    {
        public ArrayList
            erron�s,
            nonSupprim�s;

        public bool erreur;

        public Erreurs()
        {
            Reset();
        }

        public void Reset()
        {
            this.erron�s = new ArrayList();
            this.nonSupprim�s = new ArrayList();
            this.erreur = false;
        }
    }
}
