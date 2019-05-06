﻿// DirectoryCompare
// Copyright (C) 2017-2019 Dust in the Wind
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

using DirectoryCompare.CliFramework;
using DustInTheWind.DirectoryCompare.Application;
using DustInTheWind.DirectoryCompare.Cli.ResultExporters;
using MediatR;
using System;
using DustInTheWind.DirectoryCompare.Application.Compare;

namespace DustInTheWind.DirectoryCompare.Cli.Commands
{
    internal class CompareFilesCommand : ICommand
    {
        private readonly IMediator mediator;

        public string Description => "Compares two json hash files.";

        public CompareFilesCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public void Execute(Arguments arguments)
        {
            CompareDisksRequest request = CreateRequest(arguments);
            mediator.Send(request).Wait();
        }

        private static CompareDisksRequest CreateRequest(Arguments arguments)
        {
            return new CompareDisksRequest
            {
                Path1 = arguments[0],
                Path2 = arguments[1],
                Exporter = arguments.Count >= 3
                    ? (IComparisonExporter)new FileComparisonExporter { ResultsDirectory = arguments[2] }
                    : (IComparisonExporter)new ConsoleComparisonExporter()
            };
        }
    }
}