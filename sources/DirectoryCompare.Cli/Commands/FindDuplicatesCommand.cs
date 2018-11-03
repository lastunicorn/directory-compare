﻿// DirectoryCompare
// Copyright (C) 2017 Dust in the Wind
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

using DustInTheWind.DirectoryCompare.Cli.ResultExporters;
using DustInTheWind.DirectoryCompare.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace DustInTheWind.DirectoryCompare.Cli.Commands
{
    internal class FindDuplicatesCommand : ICommand
    {
        public ProjectLogger Logger { get; set; }
        public string Path1 { get; set; }
        public ConsoleDuplicatesExporter Exporter { get; set; }
        public bool CheckFilesExist { get; set; }

        public void DisplayInfo()
        {
        }

        public void Execute()
        {
            JsonFileSerializer serializer = new JsonFileSerializer();
            XContainer xContainer1 = serializer.ReadFromFile(Path1);

            List<Tuple<string, XFile>> files = new List<Tuple<string, XFile>>();
            Read(files, xContainer1, Path.DirectorySeparatorChar.ToString());

            int duplicateCount = 0;
            long totalSize = 0;

            for (int i = 0; i < files.Count; i++)
            {
                for (int j = i + 1; j < files.Count; j++)
                {
                    Tuple<string, XFile> tuple1 = files[i];
                    Tuple<string, XFile> tuple2 = files[j];

                    Duplicate duplicate = new Duplicate(tuple1, tuple2, CheckFilesExist, xContainer1);

                    if(duplicate.AreEqual)
                    {
                        duplicateCount++;
                        totalSize += duplicate.Size;
                        Exporter.WriteDuplicate(duplicate.FullPath1, duplicate.FullPath2, duplicate.Size);
                    }
                }
            }

            Exporter.WriteSummary(duplicateCount, totalSize);
        }

        private void Read(List<Tuple<string, XFile>> files, XDirectory xDirectory, string parentPath)
        {
            if (xDirectory.Files != null)
                foreach (XFile xFile in xDirectory.Files)
                {
                    string filePath = Path.Combine(parentPath, xFile.Name);
                    files.Add(new Tuple<string, XFile>(filePath, xFile));
                }

            if (xDirectory.Directories != null)
                foreach (XDirectory xSubDirectory in xDirectory.Directories)
                {
                    string subdirectoryPath = Path.Combine(parentPath, xSubDirectory.Name);
                    Read(files, xSubDirectory, subdirectoryPath);
                }
        }
    }
}