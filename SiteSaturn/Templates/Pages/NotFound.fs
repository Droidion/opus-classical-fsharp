module SiteSaturn.Templates.Pages.NotFound

open Giraffe.ViewEngine
open SiteSaturn.Templates

let view =
    let pageTitle = "Not Found"
    let pageDescription = "Page Not Found"

    [ h1 [] [ str pageTitle ]; p [] [ str "Page not found." ] ]
    |> App.view (pageTitle, pageDescription)
