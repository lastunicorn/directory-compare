﻿// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using DustInTheWind.DirectoryCompare.Cli.Application.Utils;
using DustInTheWind.DirectoryCompare.Domain.PotModel;
using DustInTheWind.DirectoryCompare.Ports.DataAccess;
using DustInTheWind.DirectoryCompare.Ports.UserAccess;
using MediatR;

namespace DustInTheWind.DirectoryCompare.Cli.Application.PotArea.DeletePot;

public class DeletePotUseCase : IRequestHandler<DeletePotRequest>
{
    private readonly IPotRepository potRepository;
    private readonly IUserInterface userInterface;

    public DeletePotUseCase(IPotRepository potRepository, IUserInterface userInterface)
    {
        this.potRepository = potRepository ?? throw new ArgumentNullException(nameof(potRepository));
        this.userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface));
    }

    public async Task Handle(DeletePotRequest request, CancellationToken cancellationToken)
    {
        Pot pot = await RetrievePot(request.PotName);
        await AskForConfirmation(pot);
        await potRepository.DeleteById(pot.Guid);
    }

    private async Task<Pot> RetrievePot(string nameOrId)
    {
        return await potRepository.GetByNameOrId(nameOrId);
    }

    private async Task AskForConfirmation(Pot pot)
    {
        PotDeletionRequest potDeletionRequest = new()
        {
            PotName = pot.Name,
            PotId = pot.Guid
        };
        bool confirmation = await userInterface.ConfirmToDelete(potDeletionRequest);

        if (!confirmation)
            throw new OperationCanceledException($"The pot {pot.Name} was not deleted.");
    }
}