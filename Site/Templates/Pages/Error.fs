/// HTML for Error page.
module Site.Templates.Pages.Error

open Falco.Markup
open Site.Templates

let private errorPage (pageTitle: string) : XmlNode list =
    [ Elem.h1 [] [ Text.raw pageTitle ]
      Elem.p [] [
          Text.raw "Error happened. I am sorry."
      ] ]

let view: XmlNode =
    let pageTitle = "Error"
    let pageDescription = "There was an error"
    errorPage pageTitle |> App.view (pageTitle, pageDescription)
