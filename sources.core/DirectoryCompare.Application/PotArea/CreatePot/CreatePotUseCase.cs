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

using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.DirectoryCompare.Domain.PotModel;
using DustInTheWind.DirectoryCompare.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.DirectoryCompare.Application.PotArea.CreatePot;

public class CreatePotUseCase : IRequestHandler<CreatePotRequest>
{
    private readonly IPotRepository potRepository;

    public CreatePotUseCase(IPotRepository potRepository)
    {
        this.potRepository = potRepository ?? throw new ArgumentNullException(nameof(potRepository));
    }

    public Task<Unit> Handle(CreatePotRequest request, CancellationToken cancellationToken)
    {
        VerifyPotDoesNotExist(request.Name);
        CreateNewPot(request);

        return Unit.Task;
    }

    private void VerifyPotDoesNotExist(string potName)
    {
        bool potAlreadyExists = potRepository.Exists(potName);

        if (potAlreadyExists)
            throw new PotAlreadyExistsException();
    }

    private void CreateNewPot(CreatePotRequest request)
    {
        Pot newPot = new()
        {
            Name = request.Name,
            Path = request.Path
        };

        potRepository.Add(newPot);
    }
}