// Directory Compare
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

using DustInTheWind.DirectoryCompare.Cli.Application.SnapshotArea.CreateSnapshot.Crawling;
using DustInTheWind.DirectoryCompare.DataStructures;
using DustInTheWind.DirectoryCompare.Ports.UserAccess;

namespace DustInTheWind.DirectoryCompare.Cli.Application.SnapshotArea.CreateSnapshot.DiskAnalysis;

internal class PreAnalysis
{
    private readonly DiskCrawler diskCrawler;
    private readonly ICreateSnapshotUi createSnapshotUi;

    public DataSize TotalDataSize { get; private set; }

    public PreAnalysis(DiskCrawler diskCrawler, ICreateSnapshotUi createSnapshotUi)
    {
        this.diskCrawler = diskCrawler ?? throw new ArgumentNullException(nameof(diskCrawler));
        this.createSnapshotUi = createSnapshotUi ?? throw new ArgumentNullException(nameof(createSnapshotUi));
    }

    public async Task RunAsync()
    {
        await AnnounceFilesIndexing();

        IEnumerable<ICrawlerItem> crawlerItems = diskCrawler.Crawl()
            .Where(x => x.Action == CrawlerAction.FileFound);

        int fileCount = 0;
        TotalDataSize = DataSize.Zero;

        bool fileWasFound = false;

        foreach (ICrawlerItem crawlerItem in crawlerItems)
        {
            try
            {
                fileWasFound = true;
                fileCount++;
                TotalDataSize += crawlerItem.Size;
            }
            catch (Exception ex)
            {
                await AnnounceFileIndexingError(crawlerItem.Path, ex);
            }

            if (fileCount % 1000 == 0)
            {
                await AnnounceFileIndexingProgress(TotalDataSize, fileCount);
                fileWasFound = false;
            }
        }

        if (fileWasFound)
            await AnnounceFileIndexingProgress(TotalDataSize, fileCount);

        await AnnounceFilesIndexed(TotalDataSize, fileCount);
    }

    private Task AnnounceFileIndexingError(string path, Exception exception)
    {
        IndexingErrorInfo indexingErrorInfo = new(exception, path);
        return createSnapshotUi.AnnounceFileIndexingError(indexingErrorInfo);
    }

    private Task AnnounceFilesIndexing()
    {
        return createSnapshotUi.AnnounceFilesIndexing();
    }

    private Task AnnounceFileIndexingProgress(ulong dataSize, int fileCount)
    {
        FileIndexInfo fileIndexInfo = new()
        {
            DataSize = dataSize,
            FileCount = fileCount
        };

        return createSnapshotUi.AnnounceFileIndexingProgress(fileIndexInfo);
    }

    private Task AnnounceFilesIndexed(ulong dataSize, int fileCount)
    {
        FileIndexInfo fileIndexInfo = new()
        {
            DataSize = dataSize,
            FileCount = fileCount
        };

        return createSnapshotUi.AnnounceFilesIndexed(fileIndexInfo);
    }
}