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

using DustInTheWind.DirectoryCompare.Domain.Comparison;
using DustInTheWind.DirectoryCompare.Domain.DataAccess;
using DustInTheWind.DirectoryCompare.Domain.Entities;
using MediatR;
using System;

namespace DustInTheWind.DirectoryCompare.Application.UseCases.CompareSnapshots
{
    public class CompareSnapshotsRequestHandler : RequestHandler<CompareSnapshotsRequest, SnapshotComparer>
    {
        private readonly ISnapshotRepository snapshotRepository;

        public CompareSnapshotsRequestHandler(ISnapshotRepository snapshotRepository)
        {
            this.snapshotRepository = snapshotRepository ?? throw new ArgumentNullException(nameof(snapshotRepository));
        }

        protected override SnapshotComparer Handle(CompareSnapshotsRequest request)
        {
            Snapshot snapshot1 = snapshotRepository.GetLast(request.PotName1);

            if (snapshot1 == null)
                throw new Exception($"There is no pot with the name '{request.PotName1}'.");

            Snapshot snapshot2 = snapshotRepository.GetLast(request.PotName2);

            if (snapshot2 == null)
                throw new Exception($"There is no pot with the name '{request.PotName2}'.");

            SnapshotComparer comparer = new SnapshotComparer(snapshot1, snapshot2);
            comparer.Compare();

            return comparer;
        }
    }
}