/// HTML snippets
module Site.Templates.Partials

open Giraffe.ViewEngine
open Site.Models
open Site.Templates.Helpers

let private title (work: Work) : XmlNode =
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

let private subtitle (work: Work) : XmlNode =
    div [ _class "card__subtitle" ] [
        if work.catalogueName.IsSome && work.catalogueNumber.IsSome then
            span [] [
                str $"{work.catalogueName.Value} {work.catalogueNumber.Value}"
                if work.cataloguePostfix.IsSome then
                    str work.cataloguePostfix.Value
            ]

            span [ _class "vertical-separator" ] []
        if work.yearStart.IsSome || work.yearFinish.IsSome then
            span [] [
                str (formatYearsRangeLoose work.yearStart work.yearFinish)
            ]

            span [ _class "vertical-separator" ] []
        if work.averageMinutes.IsSome then
            span [] [
                work.averageMinutes |> formatWorkLength |> str
            ]
    ]


/// Work card
let workCard (work: Work) =
    div [ _class "card" ] [
        title work
        subtitle work
    ]
