/*
 * Copyright ? 2005 ; 2011 ; 2015 Cl?ment Foucher
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

namespace Bitmap_Converter
{
    public partial class APropos : Form
    {
        public APropos()
        {
            InitializeComponent();
        }

        private void boutonFermer_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}