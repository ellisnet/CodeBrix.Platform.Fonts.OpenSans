# CodeBrix.Platform.Fonts.OpenSans

A redistribution of the Open Sans font family packaged as a CodeBrix-family NuGet library for .NET 10 applications.
CodeBrix.Platform.Fonts.OpenSans is a namespace-renamed, .NET 10 port of `Uno.Fonts.OpenSans` 2.8.1 — intended as a drop-in replacement for that package in CodeBrix.Platform-forked Uno applications, and usable as a plain content-files NuGet in any .NET 10 project that wants the Open Sans font set.
The library has no managed dependencies other than .NET, and is provided as a .NET 10 library and associated `CodeBrix.Platform.Fonts.OpenSans.ApacheLicenseForever` NuGet package.

CodeBrix.Platform.Fonts.OpenSans supports applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of Microsoft .NET.

## CodeBrix.Platform.Fonts.OpenSans supports:

* The Open Sans variable font (`OpenSans.ttf`) covering the full weight axis (300-800) and italic axis on platforms that support variable-font manifests.
* 36 static `.ttf` font files covering the Light/Regular/Medium/SemiBold/Bold/ExtraBold weights in Normal, Italic, Condensed, Condensed-Italic, SemiCondensed, and SemiCondensed-Italic stretches — for platforms that don't support variable fonts.
* A Uno-compatible `.ttf.manifest` JSON file that maps `font_style` / `font_weight` / `font_stretch` triples to the matching static font file.
* A `buildTransitive` MSBuild `.targets` file (hooking into Uno's `_UnoAddLibraryAssets` target) that automatically prunes the unused half of the font set at build time, based on the `SupportsFontManifest` property.
* The Uno `.uprimarker` file (renamed) so Uno-fork build pipelines can still discover the package as a UPRI-bearing font asset library.

## Sample Code

### Reference the font from XAML (Uno-fork app)

```xml
<TextBlock Text="Hello, world."
           FontFamily="ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/OpenSans.ttf#Open Sans" />
```

### Reference a specific static weight

```xml
<TextBlock Text="Bold sample"
           FontFamily="ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/OpenSans-Bold.ttf#Open Sans" />
```

### Migrating from Uno.Fonts.OpenSans

Find-and-replace `ms-appx:///Uno.Fonts.OpenSans/Fonts/` -> `ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/` across your XAML and code, and swap the NuGet reference from `Uno.Fonts.OpenSans` to `CodeBrix.Platform.Fonts.OpenSans.ApacheLicenseForever`. The font filenames, the manifest format, and the build-time pruning behavior are all preserved.

## License

The library code, the `.targets` file, and the package wrapper are licensed under the Apache License, Version 2.0. see: https://en.wikipedia.org/wiki/Apache_License

The Open Sans font files (`*.ttf`) themselves are licensed under the SIL Open Font License, Version 1.1 — see the bundled `OFL.txt` file. The combined NuGet package is published under the SPDX expression `Apache-2.0 AND OFL-1.1`.
