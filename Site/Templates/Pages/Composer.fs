/// HTML for Composer page.
module Site.Templates.Pages.Composer

open Falco.Markup
open Site.Domain.Composer
open Site.Domain.Genre
open Site.Templates
open Site.Templates.Helpers

let private composerHeaderSubtitle (composer: Composer) : XmlNode =
    Elem.div [ Attr.class' "header-subtitle" ] [
        // Countries
        Elem.span [] [
            Text.raw (String.concat ", " composer.countries)
        ]
        Elem.span [ Attr.class' "vertical-separator" ] []
        // Life years
        Elem.span [] [
            Text.raw (formatYearsRangeStrict composer.yearBorn composer.yearDied)
        ]
        // Wikipedia link
        if composer.wikipediaLink.IsSome then
            Elem.span [ Attr.class' "vertical-separator" ] []

            Elem.a [ Attr.href composer.wikipediaLink.Value ] [
                Text.raw "Wikipedia"
            ]
        // IMSLP Link
        if composer.imslpLink.IsSome then
            Elem.span [ Attr.class' "vertical-separator" ] []

            Elem.a [ Attr.href composer.imslpLink.Value ] [
                Text.raw "IMSLP"
            ]
    ]

let private works (composer: Composer) (genre: Genre): XmlNode =
    Elem.div [ Attr.class' "card-list" ] [
        for work in genre.works do
            Elem.a [ Attr.href $"/composer/{composer.slug}/work/{work.id}" ] [
                Partials.workCard work
            ]
    ]

let private composerPage (composer: Composer) (genres: Genre list) : XmlNode list =
    [ Elem.h1 [] [
        Text.raw $"{composer.firstName} {composer.lastName}"
      ]

      composerHeaderSubtitle composer

      for genre in genres do
          Elem.h2 [] [
              Text.raw $"{genre.icon} {genre.name}"
          ]

          Elem.hr []

          works composer genre ]

let view (composer: Composer) : XmlNode =
    let genres = listGenres composer.id
    let pageTitle = composer.lastName

    let pageDescription =
        $"List of important compositions by {composer.firstName} {composer.lastName}."

    composerPage composer genres |> App.view (pageTitle, pageDescription)
