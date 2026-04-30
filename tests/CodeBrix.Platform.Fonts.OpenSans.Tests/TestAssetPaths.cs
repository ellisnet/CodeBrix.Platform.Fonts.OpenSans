using System;
using System.IO;

namespace CodeBrix.Platform.Fonts.OpenSans.Tests;

internal static class TestAssetPaths
{
    public static string TestAssetsRoot { get; } =
        Path.Combine(AppContext.BaseDirectory, "TestAssets");

    public static string FontsFolder { get; } =
        Path.Combine(TestAssetsRoot, "Fonts");

    public static string ManifestPath { get; } =
        Path.Combine(FontsFolder, "OpenSans.ttf.manifest");

    public static string VariableFontPath { get; } =
        Path.Combine(FontsFolder, "OpenSans.ttf");

    public static string UprimarkerPath { get; } =
        Path.Combine(TestAssetsRoot, "CodeBrix.Platform.Fonts.OpenSans.uprimarker");

    public static string TargetsFilePath { get; } =
        Path.Combine(TestAssetsRoot, "buildTransitive", "net10.0", "CodeBrix.Platform.Fonts.OpenSans.ApacheLicenseForever.targets");
}
