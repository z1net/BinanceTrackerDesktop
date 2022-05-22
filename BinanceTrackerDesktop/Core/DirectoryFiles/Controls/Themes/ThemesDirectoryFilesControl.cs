﻿using BinanceTrackerDesktop.Core.DirectoryFiles.Base;
using BinanceTrackerDesktop.Core.DirectoryFiles.Formats;
using BinanceTrackerDesktop.Core.DirectoryFiles.Item;
using BinanceTrackerDesktop.Core.DirectoryFiles.Paths;
using static BinanceTrackerDesktop.Core.DirectoryFiles.Controls.Themes.ThemesDirectoryFilesControl;

namespace BinanceTrackerDesktop.Core.DirectoryFiles.Controls.Themes
{
    public sealed class ThemesDirectoryFilesControl : DirectoryFilesControlBase<ThemeDirectoryFileItem>
    {
        public override string FolderPath => ApplicationDirectoryPaths.Themes;

        public override IEnumerable<string> FilesExtensions => new List<string>
        {
            FilesFormatExtensions.THEME,
        };

        public override IEnumerable<ThemeDirectoryFileItem> Files { get; }



        public ThemesDirectoryFilesControl()
        {
            Directory.CreateDirectory(FolderPath);

            List<ThemeDirectoryFileItem> items = new List<ThemeDirectoryFileItem>();
            foreach (string filePath in base.GetAllFilePathFromDirectory())
            {
                items.Add(new ThemeDirectoryFileItem(filePath));
            }

            Files = items;
        }



        public sealed class ThemeDirectoryFileItem : IResultableDirectoryFileItem
        {
            public object Result { get; }

            public string FilePath { get; }

            public string FileName { get; }



            public ThemeDirectoryFileItem(string filePath)
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentNullException(nameof(filePath));
                }

                if (File.Exists(filePath) == false)
                {
                    throw new FileNotFoundException(nameof(filePath));
                }

                Result = File.ReadAllText(filePath);
                FileName = Path.GetFileName(filePath);
                FilePath = filePath;
            }



            public TResult GetResult<TResult>()
            {
                return (TResult)Result;
            }

            public string GetStringResult()
            {
                return GetResult<string>();
            }
        }

        public sealed class RegisteredThemes
        {
            public const string DarkTheme = "dark";

            public const string LightTheme = "light";
        }
    }
}