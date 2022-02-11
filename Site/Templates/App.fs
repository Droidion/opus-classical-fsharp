/// Top-level HTML layout.
module Site.Templates.App

open FSharpPlus
open Giraffe.ViewEngine
open System
open System.IO
open System.Security.Cryptography
open System.Text

/// Top-level layout template
let private computeFileHash (path: string) : string =
    File.ReadAllText(path)
    |> Encoding.UTF8.GetBytes
    |> (SHA256.Create()).ComputeHash
    |> BitConverter.ToString
    |> String.replace "-" ""

let private cssHash = computeFileHash "static/bundle.css"
let private jsHash = computeFileHash "static/bundle.js"
let private logsId = Environment.GetEnvironmentVariable("UmamiId")

let private headerLogo: XmlNode =
    a [ _class "logo-link"; _href "/" ] [
        div [ _class "brand" ] [
            img [ _alt "Opus Classical logo"
                  _class "brand__logo"
                  _width "72"
                  _height "72"
                  _src "/img/composers-logo.png" ]
            div [ _class "brand__title" ] [
                div [ _class "brand__name" ] [
                    str "Opus Classical"
                ]
                div [ _class "brand__description" ] [
                    str "Catalogue for streaming classical music"
                ]
            ]
        ]
    ]

let private headerMenu: XmlNode =
    nav [ _class "menu" ] [
        div [ _class "menu__item"; _id "searchBlock" ] []
        div [ _class "menu__item" ] [
            a [ _href "/about" ] [ str "About" ]
        ]
    ]

let private header: XmlNode =
    header [ _class "header" ] [
        headerLogo
        headerMenu
    ]

let private footer: XmlNode =
    footer [] [
        a [ _title "Github repository"
            _href "https://github.com/Droidion/composers" ] [
            img [ _alt "Github repository logo"
                  _class "footer-logo"
                  _src "/img/github-logo.svg" ]
        ]
    ]

let private _meta (name: string) (content: string) : XmlNode = meta [ _name name; _content content ]

let private iconLink (rel: string) (iconType: string) (sizes: string) (href: string) : XmlNode =
    link [ _rel rel
           _type iconType
           _sizes sizes
           _href href ]

let private maskLink (rel: string) (href: string) (color: string) : XmlNode =
    link [ _rel rel
           _href href
           _color color ]

let private headContent (pageTitle: string) (pageDescription: string) : XmlNode list =
    [ title [] [
        str $"{pageTitle} | Opus Classical"
      ]
      meta [ _charset "utf-8" ]
      _meta "description" pageDescription
      _meta "viewport" "width=device-width, initial-scale=1.0"
      _meta "msapplication-TileColor" "#da532c"
      _meta "theme-color" "#ffffff"
      iconLink "apple-touch-icon" "image/png" "180x180" "/apple-touch-icon.png"
      iconLink "icon" "image/png" "32x32" "/favicon-32x32.png"
      iconLink "icon" "image/png" "16x16" "/favicon-16x16.png"
      maskLink "mask-icon" "/safari-pinned-tab.svg" "#fff"
      maskLink "mask-icon" "safari-pinned-tab.svg" "#5bbad5"
      link [ _rel "manifest"
             _href "/site.webmanifest" ]
      link [ _rel "stylesheet"
             _href $"/bundle.css?v={cssHash}" ]
      script [ _async
               _defer
               attr "data-website-id" logsId
               _src "https://logs.opusclassical.net/umami.js" ] []
      script [ _src $"/bundle.js?v={jsHash}"; _type "module"; _defer ] [] ]

/// Renders HTML
let view (pageTitle: string, pageDescription: string) (content: XmlNode list) : XmlNode =
    html [ _lang "en" ] [
        head [] (headContent pageTitle pageDescription)
        body [] [
            header
            main [ _class "main"; attr "role" "main" ] content
            footer
        ]
    ]
