﻿// DirectoryCompare
// Copyright (C) 2017-2020 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using DustInTheWind.ConsoleFramework;
using DustInTheWind.ConsoleTools;
using DustInTheWind.DirectoryCompare.Domain.PotModel;

namespace DustInTheWind.DirectoryCompare.Cli.UI.PotCommands
{
    internal class DisplayPotsCommandView : IView<DisplayPotsCommand>
    {
        public void Display(DisplayPotsCommand command)
        {
            if (command.Pots == null)
                return;

            foreach (Pot pot in command.Pots)
            {
                string guid = pot.Guid.ToString().Substring(0, 8);
                CustomConsole.Write(guid);
                CustomConsole.Write(" ");

                CustomConsole.WriteEmphasies(pot.Name);
                CustomConsole.Write(" - ");

                CustomConsole.WriteEmphasies(pot.Path);
                CustomConsole.WriteLine();
            }
        }
    }
}