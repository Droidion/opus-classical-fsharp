/// HTML for index page.
module Site.Templates.Pages.Index

open Falco.Markup
open Site.Domain.Composer
open Site.Domain.Period
open Site.Templates
open Site.Templates.Helpers

let private composerCard (composer: Composer) =
    let composerDisabled =
        if composer.enabled then
            ""
        else
            "disabled"

    Elem.div [ Attr.class' $"card {composerDisabled}" ] [
        Elem.div [] [
            Elem.span [] [
                Text.raw $"{composer.lastName}, "
            ]
            Elem.span [ Attr.class' "card__light-text" ] [
                Text.raw composer.firstName
            ]
        ]
        Elem.div [ Attr.class' "card__subtitle" ] [
            Elem.span [] [
                composer.countries |> String.concat ", " |> Text.raw
            ]
            Elem.span [ Attr.class' "vertical-separator" ] []
            Elem.span [] [
                formatYearsRangeStrict composer.yearBorn composer.yearDied |> Text.raw
            ]
        ]
    ]

let private periodTitle (period: Period) : XmlNode =
    Elem.h2 [] [
        let yearEnd =
            match period.yearEnd with
            | Some y -> string y
            | None -> ""

        Text.raw $"{period.name}, {period.yearStart}–{yearEnd}"
    ]

let private periodComposers (period: Period) : XmlNode =
    Elem.div [ Attr.class' "card-list" ] [
        for composer in period.composers do
            if composer.enabled then
                Elem.a [ Attr.href $"/composer/{composer.slug}" ] [
                    composerCard composer
                ]
            else
                composerCard composer
    ]

let private indexPage (pageTitle: string) (periods: Period list) : XmlNode list =
    [ Elem.h1 [] [ Text.raw pageTitle ]
      for period in periods do
          periodTitle period
          Elem.hr []
          periodComposers period ]

let view (periods: Period list) : XmlNode =
    let pageTitle = "Composers"
    let pageDescription = "List of classical music composers grouped by periods."
    indexPage pageTitle periods |> App.view (pageTitle, pageDescription)
