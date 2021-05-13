module Site.Templates.Pages.NotFound

open Giraffe.ViewEngine
open Site.Templates

let view =
    let pageTitle = "Not Found"
    let pageDescription = "Page Not Found"

    [ h1 [] [ str pageTitle ]; p [] [ str "Page not found." ] ]
    |> App.view (pageTitle, pageDescription)
