module SiteSaturn.Templates.Pages.Composer

open Giraffe.ViewEngine
open SiteSaturn.Models
open SiteSaturn.Templates
open SiteSaturn.Database.Providers

let view (composer: Composer) =

    let genres =
        Genres.list composer.id |> Async.RunSynchronously

    let pageTitle = composer.lastName
    let pageDescription = $"List of important compositions by {composer.firstName} {composer.lastName}."

    [ h1 [] [
        str $"{composer.firstName} {composer.lastName}"
      ]
      div [ _class "header-subtitle" ] [
          span [] [
              str (String.concat ", " composer.countries)
          ]
          span [ _class "vertical-separator" ] []
          span [] [ str (Partials.formatYearsRangeStrict composer.yearBorn composer.yearDied) ]
          if composer.wikipediaLink.IsSome then
              span [ _class "vertical-separator" ] []

              a [ _href composer.wikipediaLink.Value ] [
                  str "Wikipedia"
              ]
          if composer.imslpLink.IsSome then
              span [ _class "vertical-separator" ] []

              a [ _href composer.imslpLink.Value ] [
                  str "IMSLP"
              ]
      ]

      for genre in genres do
          h2 [] [
              str $"{genre.icon} {genre.name}"
          ]

          hr []

          div [ _class "card-list" ] [
              for work in genre.works do
                  a [ _href $"/composer/{composer.slug}/work/{work.id}" ] [
                      Partials.workCard work
                  ]
          ] ]
    |> App.view (pageTitle, pageDescription)
