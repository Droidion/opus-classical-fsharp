/// HTML for Error page.
module Site.Templates.Pages.Error

open Giraffe.ViewEngine
open Site.Templates

let private errorPage (pageTitle: string) : XmlNode list =
    [ h1 [] [ str pageTitle ]
      p [] [
          str "Error happened. I am sorry."
      ] ]

let view: XmlNode =
    let pageTitle = "Error"
    let pageDescription = "There was an error"
    errorPage pageTitle |> App.view (pageTitle, pageDescription)
