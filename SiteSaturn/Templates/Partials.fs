module SiteSaturn.Templates.Partials

open Giraffe.ViewEngine
open SiteSaturn.Models

let mutable coversUrl = ""

/// Checks if given string is a 4 digits number, like "1234" (not "-123", "123", or "12345")
let private validDigits (str: string) : bool = str.Length = 4 && str.[0] <> '-'

/// Checks if two given string have the same first two letters, like "1320" and "1399"
let private centuryEqual (year1: int) (year2: int) : bool =
    let str1 = string year1
    let str2 = string year2
    str1.[..1] = str2.[..1]

/// Formats the range of two years into the string, e.g. "1720–95", or "1720–1805", or "1720–"
/// Start year and dash are always present
/// It's supposed to be used for lifespans, meaning we always have birth, but may not have death
let formatYearsRangeStrict (startYear: int) (finishYear: int option) : string =
    match (startYear, finishYear) with
    | start, _ when start |> string |> validDigits |> not -> ""
    | start, None -> $"{start}–"
    | start, Some finish when finish |> string |> validDigits |> not -> $"{start}–"
    | start, Some finish when centuryEqual start finish -> $"{start}–{(string finish).[2..3]}"
    | start, Some finish -> $"{start}–{finish}"
    | _, _ -> ""

/// Formats the range of two years into a string, e.g. "1720–95", or "1720–1805", or "1720"
/// Both years can be present or absent, so it's a more generic, loose form
let public formatYearsRangeLoose (startYear: int option) (finishYear: int option) : string =
    match (startYear, finishYear) with
    | Some start, None when string start |> validDigits -> string start
    | None, Some finish -> string finish
    | Some start, Some finish when
        string start |> validDigits
        && string finish |> validDigits |> not -> string start
    | Some start, Some finish when
        string start |> validDigits |> not
        && string finish |> validDigits -> string finish
    | Some start, Some finish when centuryEqual start finish -> $"{start}–{(string finish).[2..3]}"
    | Some start, Some finish -> $"{start}–{finish}"
    | _, _ -> ""

/// Formats minutes into a string with hours and minutes, like "2h 35m"
let public formatWorkLength (lengthInMinutes: int option) : string =
    let length =
        if lengthInMinutes.IsSome then
            lengthInMinutes.Value
        else
            0

    let hours = length / 60
    let minutes = length % 60

    match (hours, minutes) with
    | 0, 0 -> ""
    | h, m when h < 0 || m < 0 -> ""
    | 0, m -> $"{m}m"
    | h, 0 -> $"{h}h"
    | h, m -> $"{h}h {m}m"

let header =
    header [ _class "header" ] [
        a [ _href "/" ] [
            div [ _class "brand" ] [
                img [ _alt "Opus Classical logo"
                      _class "brand__logo"
                      _width "72"
                      _height "72"
                      _src "/img/composers-logo.png" ]
                div [ _class "brand__name" ] [
                    str "Opus Classical"
                ]
            ]
        ]
        nav [ _class "menu" ] [
            div [ _class "menu__item" ] [
                a [ _href "/about" ] [ str "About" ]
            ]
        ]
    ]

let footer =
    footer [] [
        a [ _title "Github repository"
            _href "https://github.com/Droidion/composers" ] [
            img [ _alt "Github repository logo"
                  _class "footer-logo"
                  _src "/img/github-logo.svg" ]
        ]
    ]

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
                formatYearsRangeStrict composer.yearBorn composer.yearDied
                |> str
            ]
        ]
    ]

let workCard (work: Work) =
    div [ _class "card" ] [
        div [] [
            span [] [ str work.title ]
            if work.no.IsSome then
                span [] [ str $" No. {work.no.Value}" ]
            if work.nickname.IsSome then
                cite [] [ str $" {work.nickname.Value}" ]
            if work.key.IsSome then
                span [] [ str $" in {work.key.Value}" ]
        ]
        div [ _class "card__subtitle" ] [
            if work.catalogueName.IsSome
               && work.catalogueNumber.IsSome then
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
                if recording.yearStart.IsSome
                   || recording.yearFinish.IsSome then
                    span [] [
                        str (formatYearsRangeLoose recording.yearStart recording.yearFinish)
                    ]

                    span [ _class "vertical-separator" ] []
                span [] [
                    recording.length
                    |> Some
                    |> formatWorkLength
                    |> str
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
