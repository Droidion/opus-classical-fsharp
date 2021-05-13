module Site.Templates.Pages.Index

open Giraffe.ViewEngine
open Site.Models
open Site.Templates

let view (periods: Period list) =
    let pageTitle = "Composers"
    let pageDescription = "List of classical music composers grouped by periods."

    [ h1 [] [ str pageTitle ]
      for period in periods do
          h2 [] [
              let yearEnd =
                  match period.yearEnd with
                  | Some y -> string y
                  | None -> ""

              str $"{period.name}, {period.yearStart}–{yearEnd}"
          ]

          hr []

          div [ _class "card-list" ] [
              for composer in period.composers do
                  if composer.enabled then
                      a [ _href $"/composer/{composer.slug}" ] [
                          Partials.composerCard composer
                      ]
                  else
                      Partials.composerCard composer
          ] ]
    |> App.view (pageTitle, pageDescription)
