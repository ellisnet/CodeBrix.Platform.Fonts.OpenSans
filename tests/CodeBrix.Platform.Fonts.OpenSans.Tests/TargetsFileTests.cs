using System.IO;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Platform.Fonts.OpenSans.Tests;

public class TargetsFileTests
{
    [Fact]
    public void Targets_file_is_present()
        => File.Exists(TestAssetPaths.TargetsFilePath).Should().BeTrue();

    [Fact]
    public void Targets_file_declares_codebrix_target_name()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().Contain("Name=\"CodeBrixRemoveUnusedOpenSans\"");
    }

    [Fact]
    public void Targets_file_hooks_after_codebrix_add_library_assets()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().Contain("AfterTargets=\"_CodeBrixAddLibraryAssets\"");
    }

    [Fact]
    public void Targets_file_uses_net10_lib_paths()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().Contain("lib\\net10.0\\CodeBrix.Platform.Fonts.OpenSans\\Fonts");
    }

    [Fact]
    public void Targets_file_contains_no_residual_upstream_target_name()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().NotContain("UnoRemoveUnusedOpenSans");
    }

    [Fact]
    public void Targets_file_contains_no_residual_uno_lib_path()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().NotContain("Uno.Fonts.OpenSans\\Fonts");
    }

    [Fact]
    public void Targets_file_supports_font_manifest_condition_present()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().Contain("$(SupportsFontManifest)");
    }

    [Fact]
    public void Targets_file_never_removes_the_variable_font()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        // The variable font (Fonts\OpenSans.ttf, no dash) must not appear in a
        // Remove= expression; only the dash-bearing static fonts are pruned.
        content.Should().NotContain("Fonts\\OpenSans.ttf\"");
    }
}
