module Site.Templates.Pages.Index

open Giraffe.ViewEngine
open Site.Models
open Site.Templates
open Site.Templates.Helpers

let private composerCard (composer: Composer) =
    let composerDisabled =
        if composer.enabled then
            ""
        else
            "disabled"

    div [ _class $"card {composerDisabled}" ] [
        div [] [
            span [] [ str $"{composer.lastName}, " ]
            span [ _class "card__light-text" ] [
                str composer.firstName
            ]
        ]
        div [ _class "card__subtitle" ] [
            span [] [
                composer.countries |> String.concat ", " |> str
            ]
            span [ _class "vertical-separator" ] []
            span [] [
                formatYearsRangeStrict composer.yearBorn composer.yearDied |> str
            ]
        ]
    ]

let private periodTitle (period: Period): XmlNode =
    h2 [] [
        let yearEnd =
            match period.yearEnd with
            | Some y -> string y
            | None -> ""

        str $"{period.name}, {period.yearStart}–{yearEnd}"
    ]

let private periodComposers (period: Period): XmlNode =
    div [ _class "card-list" ] [
        for composer in period.composers do
            if composer.enabled then
                a [ _href $"/composer/{composer.slug}" ] [
                    composerCard composer
                ]
            else
                composerCard composer
    ]

let private indexPage (pageTitle: string) (periods: Period list): XmlNode list =
    [ h1 [] [ str pageTitle ]
      for period in periods do
          periodTitle period
          hr []
          periodComposers period ]

let view (periods: Period list): XmlNode =
    let pageTitle = "Composers"
    let pageDescription = "List of classical music composers grouped by periods."
    indexPage pageTitle periods |> App.view (pageTitle, pageDescription)
