# CodeBrix.Platform.Fonts.OpenSans

A redistribution of the Open Sans font family packaged as a CodeBrix-family NuGet library for .NET 10 applications.
CodeBrix.Platform.Fonts.OpenSans is a content-files font package for CodeBrix.Platform applications — supplying the Open Sans variable font and its static instances as build-time assets — and is equally usable as a plain content-files NuGet in any .NET 10 project that wants the Open Sans font set.
The library has no managed dependencies other than .NET, and is provided as a .NET 10 library and associated `CodeBrix.Platform.Fonts.OpenSans.ApacheLicenseForever` NuGet package.

CodeBrix.Platform.Fonts.OpenSans supports applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of Microsoft .NET.

## CodeBrix.Platform.Fonts.OpenSans supports:

* The Open Sans variable font (`OpenSans.ttf`) covering the full weight axis (300-800) and italic axis, used directly on every platform.
* 36 static `.ttf` font files covering the Light/Regular/Medium/SemiBold/Bold/ExtraBold weights in Normal, Italic, Condensed, Condensed-Italic, SemiCondensed, and SemiCondensed-Italic stretches — for platforms that resolve fonts through the static-instance manifest.
* A `.ttf.manifest` JSON file that maps `font_style` / `font_weight` / `font_stretch` triples to the matching static font file.
* A `CODEBRIX-DEVELOP.json` descriptor that tells CodeBrix.Develop how to wire this font into a generated application and which software-keyboard layouts the package's glyph coverage supports.
* A `buildTransitive` MSBuild `.targets` file (hooking into the CodeBrix.Platform `_CodeBrixAddLibraryAssets` target) that prunes the redundant static font files at build time on platforms that don't need them, while always keeping the variable `OpenSans.ttf` available.
* The CodeBrix `.uprimarker` file so CodeBrix.Platform build pipelines discover the package as a UPRI-bearing font asset library.

## Sample Code

### Reference the font from XAML (CodeBrix.Platform app)

```xml
<TextBlock Text="Hello, world."
           FontFamily="ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/OpenSans.ttf" />
```

### Reference a specific static weight

```xml
<TextBlock Text="Bold sample"
           FontFamily="ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/OpenSans-Bold.ttf" />
```

### Set Open Sans as the default text font (CodeBrix.Platform app)

```csharp
global::CodeBrix.Platform.UI.FeatureConfiguration.Font.DefaultTextFontFamily =
    "ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/OpenSans.ttf";
```

Note that the font URI carries no `#FamilyName` fragment. CodeBrix.Platform strips such a fragment before resolving the font, and leaving it on the value assigned to `DefaultTextFontFamily` prevents the startup font-manifest preload from finding the manifest.

## Script coverage

Open Sans covers 36 of the 38 software-keyboard layouts CodeBrix.Platform ships. It has no Georgian or Armenian glyphs, and this package bundles no companion fonts to supply them — see the sibling `CodeBrix.Platform.Fonts.Roboto` and `CodeBrix.Platform.Fonts.Merriweather` packages, which do.

## License

The library code, the `.targets` file, and the package wrapper are licensed under the Apache License, Version 2.0. see: https://en.wikipedia.org/wiki/Apache_License

The Open Sans font files (`*.ttf`) themselves are licensed under the SIL Open Font License, Version 1.1 — see the bundled `OFL.txt` file. The combined NuGet package is published under the SPDX expression `Apache-2.0 AND OFL-1.1`.

See `THIRD-PARTY-NOTICES.txt` for the full attribution of the bundled font family.
