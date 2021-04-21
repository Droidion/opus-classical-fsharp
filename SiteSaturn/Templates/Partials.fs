/// HTML snippets
module SiteSaturn.Templates.Partials

open Giraffe.ViewEngine
open SiteSaturn.Models
open SiteSaturn.Templates.Helpers

/// Header with menu
let header =
    header [ _class "header" ] [
        a [ _class "logo-link"; _href "/" ] [
            div [ _class "brand" ] [
                img [ _alt "Opus Classical logo"
                      _class "brand__logo"
                      _width "72"
                      _height "72"
                      _src "/img/composers-logo.png" ]
                div [ _class "brand__title" ] [
                    div [ _class "brand__name" ] [
                        str "Opus Classical"
                    ]
                    div [ _class "brand__description" ] [
                        str "Catalogue for streaming classical music"
                    ]
                ]
            ]
        ]
        nav [ _class "menu" ] [
            div [ _class "menu__item" ] [
                a [ _href "/about" ] [ str "About" ]
            ]
        ]
    ]

/// Footer
let footer =
    footer [] [
        a [ _title "Github repository"
            _href "https://github.com/Droidion/composers" ] [
            img [ _alt "Github repository logo"
                  _class "footer-logo"
                  _src "/img/github-logo.svg" ]
        ]
    ]

/// Composer card
let composerCard (composer: Composer) =
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

/// Work card
let workCard (work: Work) =
    div [ _class "card" ] [
        div [] [
            span [] [ str work.title ]
            if work.no.IsSome then
                span [] [ str $" No. {work.no.Value}" ]
            if work.nickname.IsSome then
                cite [] [
                    str $" {work.nickname.Value}"
                ]
            if work.key.IsSome then
                span [] [ str $" in {work.key.Value}" ]
        ]
        div [ _class "card__subtitle" ] [
            if work.catalogueName.IsSome && work.catalogueNumber.IsSome then
                span [] [
                    str $"{work.catalogueName.Value} {work.catalogueNumber.Value}"
                    if work.cataloguePostfix.IsSome then
                        str work.cataloguePostfix.Value
                ]

                span [ _class "vertical-separator" ] []
            span [] [
                str (formatYearsRangeLoose work.yearStart work.yearFinish)
            ]
            if work.averageMinutes.IsSome then
                span [ _class "vertical-separator" ] []

                span [] [
                    work.averageMinutes |> formatWorkLength |> str
                ]
        ]
    ]

/// Recording card
let recordingCard (recording: Recording) =
    div [ _class "card illustrated" ] [
        img [ _class "cover"
              _src $"{coversUrl}{recording.coverName}"
              _alt "Cover" ]
        div [] [
            for performer in recording.performers do
                div [ _class "card__title" ] [
                    span [] [
                        str $"""{performer.firstName |> Option.defaultValue ""} {performer.lastName} """
                    ]
                    if performer.instrument.IsSome then
                        span [ _class "card__instrument" ] [
                            str performer.instrument.Value
                        ]
                ]
            div [ _class "card__subtitle" ] [
                if recording.label.IsSome then
                    span [] [ str recording.label.Value ]
                    span [ _class "vertical-separator" ] []
                if recording.yearStart.IsSome || recording.yearFinish.IsSome then
                    span [] [
                        str (formatYearsRangeLoose recording.yearStart recording.yearFinish)
                    ]

                    span [ _class "vertical-separator" ] []
                span [] [
                    recording.length |> Some |> formatWorkLength |> str
                ]
            ]

            div [ _class "card__streamers" ] [
                for streamer in recording.streamers do
                    div [ _class "card__streamer-el" ] [
                        a [ _href $"{streamer.prefix}{streamer.link}" ] [
                            img [ _src $"/img/{streamer.icon}"
                                  _height "24"
                                  _width "24"
                                  _alt "Streaming service logo" ]
                        ]
                    ]
            ]
        ]
    ]
