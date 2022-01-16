namespace Site.Templates

open System
open System.IO
open System.Security.Cryptography
open System.Text
open FSharpPlus
open Giraffe.ViewEngine

/// Top-level layout template
module App =
    let computeFileHash (path: string) : string =
        File.ReadAllText(path)
        |> Encoding.UTF8.GetBytes
        |> (new SHA256Managed()).ComputeHash
        |> BitConverter.ToString
        |> String.replace "-" ""

    let cssHash = computeFileHash "static/bundle.css"
    let jsHash = computeFileHash "static/bundle.js"
    let logsId = "571bc95e-5d08-489f-b649-6f475f772b00"

    /// Renders HTML
    let view (pageTitle: string, pageDescription: string) (content: XmlNode list) =
        html [ _lang "en" ] [
            head [] [
                title [] [
                    str $"{pageTitle} | Opus Classical"
                ]
                meta [ _name "description"
                       _content pageDescription ]
                meta [ _charset "utf-8" ]
                meta [ _name "viewport"
                       _content "width=device-width, initial-scale=1.0" ]
                link [ _rel "apple-touch-icon"
                       _sizes "180x180"
                       _href "/apple-touch-icon.png" ]
                link [ _rel "icon"
                       _type "image/png"
                       _sizes "32x32"
                       _href "/favicon-32x32.png" ]
                link [ _rel "icon"
                       _type "image/png"
                       _sizes "16x16"
                       _href "/favicon-16x16.png" ]
                link [ _rel "manifest"
                       _href "/site.webmanifest" ]
                link [ _rel "mask-icon"
                       _href "/safari-pinned-tab.svg"
                       _color "#fff" ]
                meta [ _name "msapplication-TileColor"
                       _content "#da532c" ]
                meta [ _name "theme-color"
                       _content "#ffffff" ]
                link [ _rel "mask-icon"
                       _href "safari-pinned-tab.svg"
                       _color "#5bbad5" ]
                link [ _rel "stylesheet"
                       _href $"/bundle.css?v={cssHash}" ]
                script [ _async
                         _defer
                         attr "data-website-id" logsId
                         _src "https://logs.opusclassical.net/umami.js" ] []
                script [ _src $"/bundle.js?v={jsHash}"
                         _type "module"
                         _defer ] []
            ]
            body [] [
                Partials.header
                main [ _class "main"; attr "role" "main" ] content
                Partials.footer
            ]
        ]
