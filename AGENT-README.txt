========================================================================
AGENT-README: CodeBrix.Platform.Fonts.OpenSans
A Comprehensive Guide for AI Coding Agents
========================================================================


OVERVIEW
========================================================================

CodeBrix.Platform.Fonts.OpenSans is a .NET 10 redistribution of the Open
Sans font family, packaged for the CodeBrix family. It supplies the Open
Sans variable font and a curated set of static instances as build-time
content assets for CodeBrix.Platform applications, and is equally usable
as a plain content-files NuGet in any .NET 10 project.

The library has effectively no managed code: the assembly is a metadata-
only .NET 10 DLL whose sole purpose is to host the bundled font content
files. The interesting payload lives in:

  - 37 `.ttf` font files (1 variable + 36 static) under
    lib/net10.0/CodeBrix.Platform.Fonts.OpenSans/Fonts/ inside the nupkg.
  - A `.ttf.manifest` JSON that maps font_style/font_weight/font_stretch
    triples to the matching static font file path.
  - A `CODEBRIX-DEVELOP.json` descriptor at the package root that tells
    CodeBrix.Develop how to wire this font into a generated application.
  - A `.uprimarker` file that CodeBrix.Platform build pipelines use to
    discover UPRI-bearing font asset packages.
  - An MSBuild `.targets` file under buildTransitive/net10.0/ that hooks
    into the CodeBrix.Platform `_CodeBrixAddLibraryAssets` target and
    prunes the redundant static fonts at consumer-build time (depending on
    the `SupportsFontManifest` MSBuild property), while always keeping the
    variable font OpenSans.ttf present.


INSTALLATION
========================================================================

NuGet package: CodeBrix.Platform.Fonts.OpenSans.ApacheLicenseForever

  dotnet add package CodeBrix.Platform.Fonts.OpenSans.ApacheLicenseForever

The library namespace inside the assembly is `CodeBrix.Platform.Fonts.OpenSans`
(without the `.ApacheLicenseForever` suffix; that suffix exists only on
the NuGet PackageId for license-disambiguation across the CodeBrix family).

Target framework: .NET 10.0 or higher.


KEY NAMESPACE
========================================================================

The library exposes no public managed types in its first iteration — the
assembly is metadata-only. Consumers reference the bundled font content
files via `ms-appx:///` URIs rooted at the assembly content folder:

  ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/OpenSans.ttf
  ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/OpenSans-Bold.ttf
  ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/OpenSans_Condensed-Regular.ttf
  ...etc.

Do NOT append a `#FamilyName` fragment to these URIs. CodeBrix.Platform
strips the fragment before resolving the font, so it buys nothing — and
on the value assigned to `FeatureConfiguration.Font.DefaultTextFontFamily`
it actively breaks the startup font-manifest preload, because the
".manifest" suffix the preload appends lands inside the URI fragment and
is then dropped.


FONT INVENTORY
========================================================================

The package ships 37 `.ttf` files plus 1 `.ttf.manifest`:

Variable font (used on platforms with SupportsFontManifest='true'):
  OpenSans.ttf  — covers weights 300-800 plus italic axis

Static fonts (used on platforms with SupportsFontManifest='false'):
  Six weights (Light, Regular, Medium, SemiBold, Bold, ExtraBold)
  in two styles (Normal, Italic) across three stretches:
    - Normal stretch:        OpenSans-{Weight}{Italic?}.ttf      (12 files)
    - Condensed stretch:     OpenSans_Condensed-{Weight}{Italic?}.ttf      (12 files)
    - SemiCondensed stretch: OpenSans_SemiCondensed-{Weight}{Italic?}.ttf  (12 files)

Manifest:
  OpenSans.ttf.manifest — JSON array of 36 entries mapping
    {font_style, font_weight, font_stretch} triples to the matching
    static font file's `ms-appx:///` URI.


CODEBRIX-DEVELOP.JSON
========================================================================

`CODEBRIX-DEVELOP.json` sits at the repository root and is packed to the
root of the nupkg. It is the font's self-description for CodeBrix.Develop's
"New CodeBrix.Platform Application" experience: the IDE reads it to learn
how to wire this font into a generated application, instead of carrying
per-font swap logic of its own.

  schemaVersion     Always 1 today. A consumer that does not recognise
                    the value should decline the font with a clear
                    message rather than guess.
  packageId         Must equal this package's NuGet PackageId.
  displayName       The typographic family name shown to the user, and
                    the authoritative value written into generated source.
  fontFamilyUri     The ms-appx URI of the primary font. No `#` fragment.
  resourceKey       The App.xaml resource key a generated application
                    uses (`OpenSansFont`).
  fallbackFontUris  Ordered ms-appx URIs of companion fonts, consulted for
                    codepoints the primary font lacks. Absent here: this
                    package ships no companion fonts.
  keyboardLayouts   The software-keyboard layout ids this package's glyph
                    coverage supports. Ids absent from this list are not
                    supported; there is deliberately no "unsupported"
                    list, so the complement of the platform's layout set
                    is always the correct answer. Open Sans covers 36 of
                    the 38 layouts — it has no Georgian (`ka`) or Armenian
                    (`hy`) glyphs, and ships no companions to supply them.

The array is generated, not hand-written — see PROVENANCE below.


CORE API REFERENCE
========================================================================

This library has no public managed API. Consumers interact with it
only through:

  1. NuGet content paths (`ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/...`)
     used as `FontFamily` values in XAML or in code that constructs
     XAML element trees, or by setting the CodeBrix.Platform default font:

       global::CodeBrix.Platform.UI.FeatureConfiguration.Font.DefaultTextFontFamily =
           "ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/OpenSans.ttf";

  2. The MSBuild `.targets` file under buildTransitive/net10.0/
     `CodeBrix.Platform.Fonts.OpenSans.ApacheLicenseForever.targets`,
     whose on-disk filename matches the NuGet PackageId so that NuGet's
     auto-import convention (NU5129) picks it up in consumer builds.
     It contains the target:

       <Target Name="CodeBrixRemoveUnusedOpenSans"
               AfterTargets="_CodeBrixAddLibraryAssets">

     On platforms that do not support the font manifest, this target
     removes the static fonts (leaving only the variable font). The
     variable OpenSans.ttf is never removed, so the direct
     `ms-appx:///.../OpenSans.ttf` reference resolves on every platform.

  3. `CODEBRIX-DEVELOP.json`, read by CodeBrix.Develop (see above).

If a future iteration of this library exposes a managed API (e.g. typed
accessors that return font streams or paths for non-CodeBrix.Platform
consumers), it will live under the `CodeBrix.Platform.Fonts.OpenSans`
root namespace and be documented in this file.


ARCHITECTURE
========================================================================

Repository layout:

  CodeBrix.Platform.Fonts.OpenSans/
    src/CodeBrix.Platform.Fonts.OpenSans/
      CodeBrix.Platform.Fonts.OpenSans.csproj
      InternalsVisibleTo.cs
      CodeBrix.Platform.Fonts.OpenSans.uprimarker     (empty file)
      buildTransitive/
        net10.0/
          CodeBrix.Platform.Fonts.OpenSans.ApacheLicenseForever.targets
      Fonts/
        OpenSans.ttf
        OpenSans.ttf.manifest
        OpenSans-{Light|Regular|Medium|SemiBold|Bold|ExtraBold}{Italic?}.ttf
        OpenSans_Condensed-{Weight}{Italic?}.ttf
        OpenSans_SemiCondensed-{Weight}{Italic?}.ttf
    tests/CodeBrix.Platform.Fonts.OpenSans.Tests/
      CodeBrix.Platform.Fonts.OpenSans.Tests.csproj
      AssemblyMetadataTests.cs
      ContentFilePresenceTests.cs
      ContentManifestTests.cs
      TargetsFileTests.cs
      TestAssetPaths.cs
    AGENT-README.txt
    CODEBRIX-DEVELOP.json
    LICENSE                  (Apache-2.0; library code)
    OFL.txt                  (SIL OFL 1.1; bundled font files)
    README.md
    THIRD-PARTY-NOTICES.txt

Inside the produced NuGet (.nupkg), the file layout is:
  buildTransitive/net10.0/CodeBrix.Platform.Fonts.OpenSans.ApacheLicenseForever.targets
  lib/net10.0/CodeBrix.Platform.Fonts.OpenSans.dll
  lib/net10.0/CodeBrix.Platform.Fonts.OpenSans.uprimarker
  lib/net10.0/CodeBrix.Platform.Fonts.OpenSans/Fonts/*.ttf
  lib/net10.0/CodeBrix.Platform.Fonts.OpenSans/Fonts/OpenSans.ttf.manifest
  AGENT-README.txt
  CODEBRIX-DEVELOP.json
  README.md
  OFL.txt
  THIRD-PARTY-NOTICES.txt
  icon-codebrix-128.png

The `lib/net10.0/CodeBrix.Platform.Fonts.OpenSans/Fonts/` content layout is
load-bearing: the `ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/...`
URIs that consumers reference resolve relative to the assembly name, so
if the assembly is renamed the content folder must be renamed in lockstep.


CODING CONVENTIONS (CodeBrix family)
========================================================================

This repository follows every CodeBrix family convention. Most are
inherited from the standard library scaffold; key points:

  * Target framework: net10.0 only. No multi-targeting.
  * Nullable reference types (NRT): OFF (do not set <Nullable>enable</Nullable>).
    No `?` annotations on reference types; no `!` null-forgiveness operator.
    Value-type nullables (`int?`, `DateOnly?`, etc.) are fine.
  * No global usings.
  * `<GenerateDocumentationFile>true</GenerateDocumentationFile>` is on.
    Every public/protected member of a public type needs an XML doc
    comment. CS1591 is fixed at source, never suppressed. (In this
    library's first iteration there are no public types, so CS1591
    is trivially clean.)
  * Tests use xUnit v3 + SilverAssertions;
    `TestContext.Current.CancellationToken` is threaded through any
    cancellable call inside a test.
  * No project-level warning suppression (`<NoWarn>`, `<WarningLevel>0</>`,
    `<TreatWarningsAsErrors>false</>`, etc. are all forbidden).
  * Copyright string in the csproj prepends the upstream attribution
    to the standard CodeBrix copyright line, per the family's
    porting-guidance rule:
      Copyright (C) 2015-2025 Uno Platform Inc. Open Sans font (c) Steve
      Matteson, distributed under SIL OFL 1.1. Copyright (c) 2026
      Jeremy Ellis and contributors.

For the full list of family conventions see CODEBRIX_LIBRARY_OBSERVATIONS.txt
in the CodeBrix.Library.Dev-private repo.


TESTING
========================================================================

Tests live under tests/CodeBrix.Platform.Fonts.OpenSans.Tests/. Run with:

  dotnet test CodeBrix.Platform.Fonts.OpenSans.slnx

The test suite covers:

  * Manifest JSON: that OpenSans.ttf.manifest deserializes cleanly,
    contains the expected number of entries (36), and that every
    entry's family_name path is rooted at
    `ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/`.
  * Content-file presence: that all 37 `.ttf` files referenced by
    the manifest plus the variable `OpenSans.ttf` exist on disk
    next to the test assembly's expected build-output font folder
    (resolved via `AppContext.BaseDirectory` + `TestAssets/Fonts/`,
    centralized in `TestAssetPaths`).
  * Assembly metadata: that the produced library assembly is named
    `CodeBrix.Platform.Fonts.OpenSans`, that its `.uprimarker`
    sibling file exists, and that the manifest carries no foreign
    family path tokens (catches incomplete rename regressions).
  * .targets file: that the buildTransitive .targets file is present
    next to the test assembly, that it declares the
    `CodeBrixRemoveUnusedOpenSans` MSBuild target, that it hooks
    `AfterTargets="_CodeBrixAddLibraryAssets"`, that it never removes the
    variable font, and that it carries no foreign family path tokens.


PROVENANCE
========================================================================

The `.csproj`, `.targets`, `.ttf.manifest`, `CODEBRIX-DEVELOP.json`,
`.uprimarker`, and documentation are original CodeBrix-family files. The
only third-party material is the Open Sans `.ttf` font binaries, which are
redistributed bit-for-bit unmodified. Their per-file provenance and the
license terms are recorded in THIRD-PARTY-NOTICES.txt (binary `.ttf` files
cannot carry an inline provenance comment).

The `keyboardLayouts` array in CODEBRIX-DEVELOP.json is GENERATED, not
hand-written: it is computed by intersecting each software-keyboard
layout's required character set (from the layout definitions in
CodeBrix.Platform) against the `cmap` of every font this package ships.
Nothing in this repository's build reads CodeBrix.Platform — the array is
computed by a developer-run tool and checked in as data. Regenerate it
whenever the platform's layout set changes or this package's font set
changes.


KNOWN GOTCHAS
========================================================================

  * `ms-appx:///` URIs are resolved by the CodeBrix.Platform runtime, not
    by .NET itself. Outside a CodeBrix.Platform host, those URIs won't
    resolve to anything. Plain .NET 10 console / test apps that reference
    this package can still access the .ttf files via the package's
    on-disk location (`<nuget-cache>/codebrix.platform.fonts.opensans.apachelicenseforever/<version>/lib/net10.0/CodeBrix.Platform.Fonts.OpenSans/Fonts/...`),
    but they have to do that lookup themselves.

  * NEVER add a `#FamilyName` fragment to a font URI in this package's
    documentation or descriptor. CodeBrix.Platform strips it during font
    resolution, and on `DefaultTextFontFamily` it silently disables the
    startup manifest preload (the appended ".manifest" lands inside the
    fragment and is dropped by `Uri.PathAndQuery`).

  * The .targets file hooks `AfterTargets="_CodeBrixAddLibraryAssets"` —
    the asset target defined by the CodeBrix.Platform UI build tasks. If
    that internal MSBuild target name ever changes again, this .targets
    file must be updated in lockstep — otherwise the static-font pruning
    will silently stop firing.

  * The Open Sans Reserved Font Name (per SIL OFL 1.1 condition 3)
    must not be reused for any modified version of the fonts. Do NOT
    rename the .ttf files in a way that would imply a derivative work
    bearing the same display name.
