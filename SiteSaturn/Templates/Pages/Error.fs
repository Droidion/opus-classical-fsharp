module SiteSaturn.Templates.Pages.Error

open Giraffe.ViewEngine
open SiteSaturn.Templates

let view =
    let pageTitle = "Error"
    let pageDescription = "There was an error"

    [ h1 [] [ str pageTitle ]; p [] [ str "Error happened. I am sorry." ] ]
    |> App.view (pageTitle, pageDescription)
