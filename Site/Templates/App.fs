/// Top-level HTML layout.
module Site.Templates.App

open FSharpPlus
open System
open System.IO
open System.Security.Cryptography
open System.Text
open Falco.Markup

/// Top-level layout template
let private computeFileHash (path: string) : string =
    File.ReadAllText(path)
    |> Encoding.UTF8.GetBytes
    |> (SHA256.Create()).ComputeHash
    |> BitConverter.ToString
    |> String.replace "-" ""

let private cssHash = computeFileHash "wwwroot/bundle.css"
let private jsHash = computeFileHash "wwwroot/bundle.js"
let private logsId = Environment.GetEnvironmentVariable("UmamiId")

let private headerLogo: XmlNode =
    Elem.a [ Attr.class' "logo-link"; Attr.href "/" ] [
        Elem.div [ Attr.class' "brand" ] [
            Elem.img [ Attr.alt "Opus Classical logo"
                       Attr.class' "brand__logo"
                       Attr.width "72"
                       Attr.height "72"
                       Attr.src "/img/composers-logo.png" ]
            Elem.div [ Attr.class' "brand__title" ] [
                Elem.div [ Attr.class' "brand__name" ] [
                    Text.raw "Opus Classical"
                ]
                Elem.div [ Attr.class' "brand__description" ] [
                    Text.raw "Catalogue for streaming classical music"
                ]
            ]
        ]
    ]

let private headerMenu: XmlNode =
    Elem.nav [ Attr.class' "menu" ] [
        Elem.div [ Attr.class' "menu__item"; Attr.id "searchBlock" ] []
        Elem.div [ Attr.class' "menu__item" ] [
            Elem.a [ Attr.href "/about" ] [
                Text.raw "About"
            ]
        ]
    ]

let private header: XmlNode =
    Elem.header [ Attr.class' "header" ] [
        headerLogo
        headerMenu
    ]

let private footer: XmlNode =
    Elem.footer [] [
        Elem.a [ Attr.title "Buy me a coffee"
                 Attr.href "https://www.buymeacoffee.com/zunh" ] [
            Elem.img [ Attr.alt "Buy me a coffee"
                       Attr.class' "buy-coffee-button"
                       Attr.src "/img/bmc-button.svg" ]
        ]
        
        Elem.a [ Attr.title "Github repository"
                 Attr.href "https://github.com/Droidion/composers" ] [
            Elem.img [ Attr.alt "Github repository logo"
                       Attr.class' "footer-logo"
                       Attr.src "/img/github-logo.svg" ]
        ]
    ]

let private _meta (name: string) (content: string) : XmlNode =
    Elem.meta [ Attr.name name
                Attr.content content ]

let private iconLink (rel: string) (iconType: string) (sizes: string) (href: string) : XmlNode =
    Elem.link [ Attr.rel rel
                Attr.type' iconType
                Attr.create "sizes" sizes
                Attr.href href ]

let private maskLink (rel: string) (href: string) (color: string) : XmlNode =
    Elem.link [ Attr.rel rel
                Attr.href href
                Attr.create "color" color ]

let private headContent (pageTitle: string) (pageDescription: string) : XmlNode list =
    [ Elem.title [] [
        Text.raw $"{pageTitle} | Opus Classical"
      ]
      Elem.meta [ Attr.charset "utf-8" ]
      _meta "description" pageDescription
      _meta "viewport" "width=device-width, initial-scale=1.0"
      _meta "msapplication-TileColor" "#da532c"
      _meta "theme-color" "#ffffff"
      iconLink "apple-touch-icon" "image/png" "180x180" "/apple-touch-icon.png"
      iconLink "icon" "image/png" "32x32" "/favicon-32x32.png"
      iconLink "icon" "image/png" "16x16" "/favicon-16x16.png"
      maskLink "mask-icon" "/safari-pinned-tab.svg" "#fff"
      maskLink "mask-icon" "safari-pinned-tab.svg" "#5bbad5"
      Elem.link [ Attr.rel "manifest"
                  Attr.href "/site.webmanifest" ]
      Elem.link [ Attr.rel "stylesheet"
                  Attr.href $"/bundle.css?v={cssHash}" ]
      Elem.script [ Attr.createBool "async"
                    Attr.createBool "defer"
                    Attr.create "data-website-id" logsId
                    Attr.src "https://logs.opusclassical.net/umami.js" ] []
      Elem.script [ Attr.src $"/bundle.js?v={jsHash}"
                    Attr.type' "module"
                    Attr.createBool "defer" ] [] ]

/// Renders HTML
let view (pageTitle: string, pageDescription: string) (content: XmlNode list) : XmlNode =
    Elem.html [ Attr.lang "en" ] [
        Elem.head [] (headContent pageTitle pageDescription)
        Elem.body [] [
            header
            Elem.main [ Attr.class' "main"; Attr.create "role" "main" ] content
            footer
        ]
    ]
