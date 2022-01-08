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
using DustInTheWind.ConsoleFramework;
using DustInTheWind.DirectoryCompare.Domain.Comparison;
using DustInTheWind.DirectoryCompare.Domain.Utils;

namespace DustInTheWind.DirectoryCompare.Cli.Presentation.MiscellaneousCommands
{
    internal class FindDuplicatesCommandView : ICommandView<FindDuplicatesCommandModel>
    {
        public void Display(FindDuplicatesCommandModel commandModel)
        {
            foreach (FilePair filePair in commandModel.FileDuplicates) 
                WriteDuplicate(filePair);
            
            WriteSummary(commandModel.DuplicateCount, commandModel.TotalSize);
        }
        
        private static void WriteDuplicate(FilePair filePair)
        {
            Console.WriteLine(filePair.FullPathLeft);
            Console.WriteLine(filePair.FullPathRight);

            Console.WriteLine($"{filePair.Size} ({filePair.Size.ToString(DataSizeUnit.Byte)})");
            Console.WriteLine();
        }

        private static void WriteSummary(int duplicateCount, DataSize totalSize)
        {
            Console.WriteLine($"Total duplicates: {duplicateCount:n0} files");
            Console.WriteLine($"Total size: {totalSize} ({totalSize.ToString(DataSizeUnit.Byte)})");
            Console.WriteLine();
        }
    }
}