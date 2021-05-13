module Site.Templates.Pages.Composer

open Giraffe.ViewEngine
open Site.Models
open Site.Templates
open Site.Database.Providers
open Site.Templates.Helpers

let view (composer: Composer) =

    let genres = listGenres composer.id

    let pageTitle = composer.lastName

    let pageDescription =
        $"List of important compositions by {composer.firstName} {composer.lastName}."

    [ h1 [] [
        str $"{composer.firstName} {composer.lastName}"
      ]
      div [ _class "header-subtitle" ] [
          span [] [
              str (String.concat ", " composer.countries)
          ]
          span [ _class "vertical-separator" ] []
          span [] [
              str (formatYearsRangeStrict composer.yearBorn composer.yearDied)
          ]
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
