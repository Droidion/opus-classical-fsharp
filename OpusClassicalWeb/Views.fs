module OpusClassicalWeb.Views

open Falco.Markup
open OpusClassicalWeb.Models

let rootLayout (title: string) (content: XmlNode list) : XmlNode =
    Elem.html
        [ Attr.lang "en" ]
        [ Elem.head
              []
              [ Elem.meta [ Attr.charset "utf-8" ]
                Elem.meta [ Attr.name "viewport"; Attr.content "width=device-width, initial-scale=1" ]
                Elem.title [] [ Text.raw title ]
                Elem.meta
                    [ Attr.name "description"
                      Attr.content "Catalogue for streaming classical music." ]
                Elem.meta [ Attr.name "msapplication-TileColor"; Attr.content "#da532c" ]
                Elem.meta [ Attr.name "color-scheme"; Attr.content "dark light" ]
                Elem.meta
                    [ Attr.name "theme-color"
                      Attr.media "(prefers-color-scheme: light)"
                      Attr.content "#ffffff" ]
                Elem.meta
                    [ Attr.name "theme-color"
                      Attr.media "(prefers-color-scheme: dark)"
                      Attr.content "#1a1a1a" ]
                Elem.link
                    [ Attr.rel "apple-touch-icon"
                      Attr.type' "image/png"
                      Attr.sizes "180x180"
                      Attr.href "/images/apple-touch-icon.png" ]
                Elem.link
                    [ Attr.rel "icon"
                      Attr.type' "image/png"
                      Attr.sizes "32x32"
                      Attr.href "/images/favicon-32x32.png" ]
                Elem.link
                    [ Attr.rel "icon"
                      Attr.type' "image/png"
                      Attr.sizes "16x16"
                      Attr.href "/images/favicon-16x16.png" ]
                Elem.link
                    [ Attr.rel "apple-touch-icon-precomposed"
                      Attr.type' "image/png"
                      Attr.sizes "180x180"
                      Attr.href "/images/apple-touch-icon.png" ]
                Elem.link [ Attr.rel "icon"; Attr.type' "image/x-icon"; Attr.href "/images/favicon.ico" ]
                Elem.script [ Attr.type' "module"; Attr.src "/js/theme-switcher.js"; Attr.defer ] []
                Elem.link [ Attr.rel "stylesheet"; Attr.href "/css/output.css" ] ]
          Elem.body [] [ Elem.script [ Attr.src "/js/theme-init.js" ] []; yield! content ] ]

let homePage (periods: Period list) : XmlNode list =
    [ Elem.h1 [] [ Text.raw "Sample App" ]
      Elem.ul [] (periods |> List.map (fun period -> Elem.li [] [ Text.raw period.Name ])) ]
