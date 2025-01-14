﻿using BinanceTrackerDesktop.Core.DirectoryFiles.Item;

namespace BinanceTrackerDesktop.Core.DirectoryFiles
{
    public interface IDirectoryFilesControl<TFileItem> where TFileItem : IDirectoryFileItem
    {
        string FolderPath { get; }

        IEnumerable<TFileItem> Files { get; }

        IEnumerable<string> FileExtensions { get; }



        TFileItem GetDirectoryFile(string name);

        IEnumerable<string> GetAllFilePathFromDirectory();
    }
}
