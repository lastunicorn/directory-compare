﻿// DirectoryCompare
// Copyright (C) 2017-2023 Dust in the Wind
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

using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Commando;

namespace DustInTheWind.DirectoryCompare.Cli.Presentation.PotCommands.DisplayPot;

internal class DisplayPotView : ViewBase<DisplayPotViewModel>
{
    public override void Display(DisplayPotViewModel viewModel)
    {
        if (viewModel.Exists)
        {
            DisplayPotInfo(viewModel.PotViewModel);
            CustomConsole.WriteLine();

            DisplaySnapshots(viewModel.Snapshots);
        }
        else
        {
            CustomConsole.WriteWarning("The pot does not exist.");
        }
    }

    private static void DisplayPotInfo(PotViewModel viewModel)
    {
        PotDataGrid potDataGrid = new();
        potDataGrid.AddPot(viewModel);

        potDataGrid.Display();
    }

    private void DisplaySnapshots(List<SnapshotViewModel> snapshots)
    {
        if (snapshots is { Count: > 0 })
        {
            SnapshotsDataGrid snapshotsDataGrid = new();
            snapshotsDataGrid.AddSnapshots(snapshots);

            snapshotsDataGrid.Display();
        }
        else
        {
            WriteValue("Snapshots", "<none>");
        }
    }
}