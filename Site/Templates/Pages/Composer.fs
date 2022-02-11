/// HTML for Composer page.
module Site.Templates.Pages.Composer

open Giraffe.ViewEngine
open Site.Domain.Composer
open Site.Domain.Genre
open Site.Templates
open Site.Templates.Helpers

let private composerHeaderSubtitle (composer: Composer) : XmlNode =
    div [ _class "header-subtitle" ] [
        // Countries
        span [] [
            str (String.concat ", " composer.countries)
        ]
        span [ _class "vertical-separator" ] []
        // Life years
        span [] [
            str (formatYearsRangeStrict composer.yearBorn composer.yearDied)
        ]
        // Wikipedia link
        if composer.wikipediaLink.IsSome then
            span [ _class "vertical-separator" ] []

            a [ _href composer.wikipediaLink.Value ] [
                str "Wikipedia"
            ]
        // IMSLP Link
        if composer.imslpLink.IsSome then
            span [ _class "vertical-separator" ] []

            a [ _href composer.imslpLink.Value ] [
                str "IMSLP"
            ]
    ]

let private works (composer: Composer) (genre: Genre): XmlNode =
    div [ _class "card-list" ] [
        for work in genre.works do
            a [ _href $"/composer/{composer.slug}/work/{work.id}" ] [
                Partials.workCard work
            ]
    ]

let private composerPage (composer: Composer) (genres: Genre list) : XmlNode list =
    [ h1 [] [
        str $"{composer.firstName} {composer.lastName}"
      ]

      composerHeaderSubtitle composer

      for genre in genres do
          h2 [] [
              str $"{genre.icon} {genre.name}"
          ]

          hr []

          works composer genre ]

let view (composer: Composer) : XmlNode =
    let genres = listGenres composer.id
    let pageTitle = composer.lastName

    let pageDescription =
        $"List of important compositions by {composer.firstName} {composer.lastName}."

    composerPage composer genres |> App.view (pageTitle, pageDescription)
