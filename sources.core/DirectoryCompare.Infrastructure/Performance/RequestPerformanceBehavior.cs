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

using DustInTheWind.DirectoryCompare.Domain.Logging;
using MediatR;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DustInTheWind.DirectoryCompare.Infrastructure.Performance
{
    public class RequestPerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch timer;
        private readonly IProjectLogger logger;

        public RequestPerformanceBehavior(IProjectLogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            timer = new Stopwatch();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            timer.Start();

            string name = typeof(TRequest).Name;
            logger.Debug("Request {0} started.", name);

            try
            {
                return await next();
            }
            finally
            {
                timer.Stop();
                logger.Debug("Request {0} finished in {1:n0} milliseconds", name, timer.ElapsedMilliseconds);
            }
        }
    }
}