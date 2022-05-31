﻿namespace BinanceTrackerDesktop.DirectoryFiles.Paths;

public sealed class ApplicationDirectoryPaths
{
    public const string UserDataDirectoryName = "Data";

    public static readonly string Resources = nameof(Resources);

    public static readonly string Localizations = Path.Combine(Resources, nameof(Localizations));

    public static readonly string User = Path.Combine(Resources, nameof(User));

    public static readonly string UserData = Path.Combine(User, UserDataDirectoryName);

    public static readonly string Images = Path.Combine(Resources, nameof(Images));

    public static readonly string Themes = Path.Combine(Resources, nameof(Themes));
}