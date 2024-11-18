# Opus Classical

In F#!

## How to run

Have [DotNet 9](https://dotnet.microsoft.com/en-us/download) installed.

Have [Fantomas](https://fsprojects.github.io/fantomas/docs/end-users/GettingStarted.html) installed for formatting F# code.

Have [Bun](https://bun.sh/docs/installation) installed.

Go to the web app project `cd OpusClassicalWeb`.

Install static assets build chain packages `bun i`.

Build static assets in watch mode `bun run build:css:watch`.

Run the web app in watch mode in other terminal tab `dotnet watch run --environment Development`.
