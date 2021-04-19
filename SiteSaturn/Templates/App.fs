namespace SiteSaturn.Templates

open Giraffe.ViewEngine

/// Top-level layout template
module App =

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
                       _href "/css/site.css" ]
                link [ _rel "preload"
                       attr "as" "font"
                       _type "font/woff2"
                       _href "/fonts/domine-v11-latin-600.woff2"
                       _crossorigin "true" ]
                link [ _rel "preload"
                       attr "as" "font"
                       _type "font/woff2"
                       _href "/fonts/roboto-v20-latin-regular.woff2"
                       _crossorigin "true" ]
                link [ _rel "preload"
                       attr "as" "font"
                       _type "font/woff2"
                       _href "/fonts/roboto-v20-latin-italic.woff2"
                       _crossorigin "true" ]
                link [ _rel "preload"
                       attr "as" "font"
                       _type "font/woff2"
                       _href "/fonts/roboto-v20-latin-300.woff2"
                       _crossorigin "true" ]
            ]
            body [] [
                Partials.header
                main [ _class "main"; attr "role" "main" ] content
                Partials.footer
            ]
        ]
