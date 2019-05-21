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

using DustInTheWind.DirectoryCompare.Comparison;
using DustInTheWind.DirectoryCompare.Entities;
using NUnit.Framework;

namespace DustInTheWind.DirectoryCompare.Tests.SnapshotComparerTests
{
    [TestFixture]
    public class IdenticalSnapshotsWithTwoDifferentFilesTests
    {
        private Snapshot snapshot1;
        private Snapshot snapshot2;
        private SnapshotComparer snapshotComparer;

        [SetUp]
        public void SetUp()
        {
            snapshot1 = new Snapshot();
            snapshot1.Files.AddRange(new[]
            {
                new HFile { Name = "File1.txt", Hash = new byte[] { 1, 2, 3 } },
                new HFile { Name = "File2.txt", Hash = new byte[] { 10, 20, 30 } }
            });
            snapshot2 = new Snapshot();
            snapshot2.Files.AddRange(new[]
            {
                new HFile { Name = "File1.txt", Hash = new byte[] { 1, 2, 3 } },
                new HFile { Name = "File2.txt", Hash = new byte[] { 10, 20, 30 } }
            });
            snapshotComparer = new SnapshotComparer(snapshot1, snapshot2);
        }

        [Test]
        public void OnlyInSnapshot1_is_empty()
        {
            snapshotComparer.Compare();

            Assert.That(snapshotComparer.OnlyInSnapshot1, Is.Empty);
        }

        [Test]
        public void OnlyInSnapshot2_is_empty()
        {
            snapshotComparer.Compare();

            Assert.That(snapshotComparer.OnlyInSnapshot2, Is.Empty);
        }

        [Test]
        public void DifferentNames_is_empty()
        {
            snapshotComparer.Compare();

            Assert.That(snapshotComparer.DifferentNames, Is.Empty);
        }

        [Test]
        public void DifferentContent_is_empty()
        {
            snapshotComparer.Compare();

            Assert.That(snapshotComparer.DifferentContent, Is.Empty);
        }
    }
}