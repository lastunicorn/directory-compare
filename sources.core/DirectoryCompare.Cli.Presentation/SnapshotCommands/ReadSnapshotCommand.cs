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

using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.DirectoryCompare.Application.SnapshotArea.CreateSnapshot;
using DustInTheWind.DirectoryCompare.DiskAnalysis;
using DustInTheWind.DirectoryCompare.Infrastructure;

namespace DustInTheWind.DirectoryCompare.Cli.Presentation.SnapshotCommands;

// Example:
// read <pot-name>

[NamedCommand("read", Order = 9, Description = "Creates a new snapshot in a specific pot.")]
public class ReadSnapshotCommand : CommandBase<ReadSnapshotCommandView>
{
    private readonly RequestBus requestBus;

    [AnonymousParameter(Order = 1)]
    public string PotName { get; set; }

    public ReadSnapshotCommand(RequestBus requestBus)
        : base(new ReadSnapshotCommandView())
    {
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
    }

    public override async Task Execute()
    {
        CreateSnapshotRequest request = new()
        {
            PotName = PotName
        };

        IDiskAnalysisProgress diskAnalysisProgress = await requestBus.PlaceRequest<CreateSnapshotRequest, IDiskAnalysisProgress>(request);
        diskAnalysisProgress.Progress += HandleAnalysisProgress;

        diskAnalysisProgress.WaitToEnd();
        Console.FinishDisplay();
    }

    private void HandleAnalysisProgress(object sender, DiskAnalysisProgressEventArgs value)
    {
        int percentage = (int)value.Percentage;
        Console.HandleProgress(percentage);
    }
}