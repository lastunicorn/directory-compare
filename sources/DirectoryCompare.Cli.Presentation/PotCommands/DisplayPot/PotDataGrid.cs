﻿// DirectoryCompare
// Copyright (C) 2017-2024 Dust in the Wind
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

using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.DirectoryCompare.Cli.Presentation.Utils;

namespace DustInTheWind.DirectoryCompare.Cli.Presentation.PotCommands.DisplayPot;

internal class PotDataGrid : CustomDataGrid
{
    public DataSizeFormat DataSizeFormat { get; set; }

    public PotDataGrid()
    {
        HeaderRow.IsVisible = false;
        CreateColumns();
    }

    private void CreateColumns()
    {
        Column nameColumn = new("Name")
        {
            ForegroundColor = ConsoleColor.White
        };
        Columns.Add(nameColumn);

        Column valueColumn = new("Value")
        {
            ForegroundColor = ConsoleColor.DarkGray
        };
        Columns.Add(valueColumn);
    }

    public void AddPot(PotViewModel viewModel)
    {
        TitleRow.TitleCell.Content = viewModel.Name;

        Rows.Add("Id", viewModel.Guid);

        List<string> lines = new()
        {
            viewModel.Path
        };

        if (viewModel.IncludedPaths is { Count: > 0 })
        {
            IEnumerable<string> includedPaths = viewModel.IncludedPaths
                .Select(x => $"  {x}");

            lines.AddRange(includedPaths);
        }

        MultilineText pathText = new(lines);
        Rows.Add((MultilineText)"Path", pathText);

        Rows.Add("Size", viewModel.Size.ToDataSizeDisplay(DataSizeFormat | DataSizeFormat.Detailed));

        if (viewModel.Description != null)
            Rows.Add("Description", viewModel.Description);
    }
}