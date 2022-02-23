/// HTML for Not Found page.
module Site.Templates.Pages.NotFound

open Falco.Markup
open Site.Templates

let private notFoundPage (pageTitle: string) : XmlNode list =
    [ Elem.h1 [] [ Text.raw pageTitle ]
      Elem.p [] [ Text.raw "Page not found." ] ]

let view: XmlNode =
    let pageTitle = "Not Found"
    let pageDescription = "Page Not Found"
    notFoundPage pageTitle |> App.view (pageTitle, pageDescription)
