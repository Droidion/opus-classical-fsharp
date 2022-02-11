/// HTML for Not Found page.
module Site.Templates.Pages.NotFound

open Giraffe.ViewEngine
open Site.Templates

let private notFoundPage (pageTitle: string) : XmlNode list = [ h1 [] [ str pageTitle ]; p [] [ str "Page not found." ] ]

let view: XmlNode =
    let pageTitle = "Not Found"
    let pageDescription = "Page Not Found"
    notFoundPage pageTitle |> App.view (pageTitle, pageDescription)
