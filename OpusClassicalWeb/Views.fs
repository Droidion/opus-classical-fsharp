module OpusClassicalWeb.Views

open Falco.Markup
open OpusClassicalWeb.Models
open OpusClassicalWeb.Svg

let footer: XmlNode =
    Elem.footer
        [ Attr.class'
              "flex items-center justify-center w-full h-16 px-4 bg-black/20 dark:bg-mineshaft dark:xl:bg-codgray max-w-screen-xl xl:bg-white" ]
        [ Elem.a
              [ Attr.class' "mx-3"
                Attr.title "Buy me a coffee"
                Attr.rel "noopener noreferrer"
                Attr.href "https://www.buymeacoffee.com/zunh" ]
              [ Elem.img
                    [ Attr.alt "Buy me a coffee"
                      Attr.class' "h-8"
                      Attr.width "128"
                      Attr.height "36"
                      Attr.src "/images/bmc-button.svg" ] ]

          Elem.a
              [ Attr.class' "mx-3"
                Attr.title "Github repository"
                Attr.rel "noopener noreferrer"
                Attr.href "https://github.com/Droidion/opus-classical-fsharp" ]
              [ githubIcon ] ]


let header: XmlNode =
    Elem.header
        [ Attr.class'
              "flex items-center justify-between w-full h-16 px-4 bg-black/20 top-0 z-10 max-w-screen-xl dark:bg-mineshaft dark:xl:bg-codgray xl:sticky xl:h-24 xl:bg-white" ]
        [ Elem.a
              [ Attr.class' "hover:no-underline"; Attr.href "" ]
              [ Elem.div
                    [ Attr.class' "flex items-center" ]
                    [ opusclassicalIcon
                      Elem.div
                          [ Attr.class' "ml-2 xl:ml-4" ]
                          [ Elem.div
                                [ Attr.class' "font-serif text-lg font-medium xl:text-3xl" ]
                                [ Text.raw "Opus Classical" ]
                            Elem.div
                                [ Attr.class' "text-xs xl:text-base" ]
                                [ Text.raw "Catalogue for streaming classical music" ] ] ] ]

          Elem.nav [ Attr.class' "flex items-center menu" ] [ Text.raw "TODO" ] ]

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
          Elem.body
              []
              [ Elem.script [ Attr.src "/js/theme-init.js" ] []
                Elem.div
                    [ Attr.class' "grid justify-items-center min-h-screen w-full grid-rows-[auto_1fr_auto]" ]
                    [ header
                      Elem.main
                          [ Attr.class' "main flex flex-col w-full max-w-screen-xl overflow-auto px-4 pb-4" ]
                          content
                      footer ] ] ]

let private periodBlock (periodWithComposers: PeriodWithComposers) : XmlNode list =
    [ Elem.h2 [] [ Text.raw periodWithComposers.Period.Name ]
      yield!
          periodWithComposers.Composers
          |> List.map (fun composer -> Elem.div [] [ Text.raw composer.LastName ]) ]


let composersPage (periodsWithComposers: PeriodWithComposers list) : XmlNode list =
    [ Elem.h1 [] [ Text.raw "Sample App" ]
      yield! periodsWithComposers |> List.map periodBlock |> List.concat ]
