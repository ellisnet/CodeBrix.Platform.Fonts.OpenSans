========================================================================
AGENT-README: CodeBrix.Platform.Fonts.OpenSans
A Comprehensive Guide for AI Coding Agents
========================================================================


OVERVIEW
========================================================================

CodeBrix.Platform.Fonts.OpenSans is a .NET 10 redistribution of the Open
Sans font family, packaged for the CodeBrix family. It is a namespace-
renamed port of `Uno.Fonts.OpenSans` 2.8.1, intended as a drop-in
replacement for that package in CodeBrix.Platform-forked Uno applications.

The library has effectively no managed code: the assembly is a metadata-
only .NET 10 DLL whose sole purpose is to host the bundled font content
files. The interesting payload lives in:

  - 37 `.ttf` font files (1 variable + 36 static) under
    lib/net10.0/CodeBrix.Platform.Fonts.OpenSans/Fonts/ inside the nupkg.
  - A `.ttf.manifest` JSON that maps font_style/font_weight/font_stretch
    triples to the matching static font file path.
  - A `.uprimarker` file that Uno-fork build pipelines use to discover
    UPRI-bearing font asset packages.
  - An MSBuild `.targets` file under buildTransitive/net10.0/ that hooks
    into Uno's `_UnoAddLibraryAssets` target and prunes either the
    static fonts or the variable font at consumer-build time, depending
    on the `SupportsFontManifest` MSBuild property.


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

The library exposes no public managed types in its first iteration —
the assembly is metadata-only, matching the shape of upstream
Uno.Fonts.OpenSans. Consumers reference the bundled font content
files via `ms-appx:///` URIs rooted at the assembly content folder:

  ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/OpenSans.ttf
  ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/OpenSans-Bold.ttf
  ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/OpenSans_Condensed-Regular.ttf
  ...etc.


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


CORE API REFERENCE
========================================================================

This library has no public managed API. Consumers interact with it
only through:

  1. NuGet content paths (`ms-appx:///CodeBrix.Platform.Fonts.OpenSans/Fonts/...`)
     used as `FontFamily` values in XAML or in code that constructs
     XAML element trees.

  2. The MSBuild `.targets` file under buildTransitive/net10.0/
     `CodeBrix.Platform.Fonts.OpenSans.ApacheLicenseForever.targets`,
     whose on-disk filename matches the NuGet PackageId so that NuGet's
     auto-import convention (NU5129) picks it up in consumer builds.
     It contains the target:

       <Target Name="CodeBrixRemoveUnusedOpenSans"
               AfterTargets="_UnoAddLibraryAssets">

     This target conditionally removes either the static fonts (when
     the consumer platform supports the variable-font manifest) or the
     variable font (when the consumer platform doesn't), so the final
     output ships only the font files actually used.

If a future iteration of this library exposes a managed API (e.g.
typed accessors that return font streams or paths for non-Uno
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
  * Tests use xUnit v3 + SilverAssertions; coverlet.collector
    for coverage; `TestContext.Current.CancellationToken` is threaded
    through any cancellable call inside a test.
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
    sibling file exists, and that the manifest contains zero
    occurrences of the upstream `Uno.Fonts.OpenSans` path token
    (catches incomplete namespace-rename regressions).
  * .targets file: that the buildTransitive .targets file is present
    next to the test assembly, that it declares the
    `CodeBrixRemoveUnusedOpenSans` MSBuild target, that it still
    hooks `AfterTargets="_UnoAddLibraryAssets"`, and that it contains
    no leftover `Uno.Fonts.OpenSans\Fonts` path tokens from the
    upstream port.


PROVENANCE OF PORTED FILES
========================================================================

Files derived from upstream Uno.Fonts.OpenSans 2.8.1 carry a
`//was previously: <upstream-ns>;` provenance comment on their
namespace line (for .cs files) or an analogous comment in their
file header (for non-.cs files where comments are syntactically
allowed). Binary `.ttf` files cannot carry inline provenance,
so the file-by-file provenance for those is recorded in
THIRD-PARTY-NOTICES.txt instead.


KNOWN GOTCHAS
========================================================================

  * `ms-appx:///` URIs are resolved by Uno's runtime, not by .NET
    itself. Outside an Uno-fork host, those URIs won't resolve to
    anything. Plain .NET 10 console / test apps that reference this
    package can still access the .ttf files via the package's
    on-disk location (`<nuget-cache>/codebrix.platform.fonts.opensans.apachelicenseforever/<version>/lib/net10.0/CodeBrix.Platform.Fonts.OpenSans/Fonts/...`),
    but they have to do that lookup themselves.

  * The .targets file's `AfterTargets="_UnoAddLibraryAssets"` reference
    is preserved verbatim from upstream. If the CodeBrix-fork of Uno
    eventually renames that internal MSBuild target name, this .targets
    file must be updated in lockstep — otherwise the conditional
    pruning of static-vs-variable fonts will silently stop firing.

  * The Open Sans Reserved Font Name (per SIL OFL 1.1 condition 3)
    must not be reused for any modified version of the fonts. Do NOT
    rename the .ttf files in a way that would imply a derivative work
    bearing the same display name.
