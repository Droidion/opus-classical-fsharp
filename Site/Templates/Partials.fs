/// HTML snippets.
module Site.Templates.Partials

open Site.Domain.Work
open Site.Templates.Helpers
open Falco.Markup

let private title (work: Work) : XmlNode =
    Elem.div [] [
        Elem.span [] [ Text.raw work.title ]
        if work.no.IsSome then
            Elem.span [] [ Text.raw $" No. {work.no.Value}" ]
        if work.nickname.IsSome then
            Elem.tag "cite" [] [
                Text.raw $" {work.nickname.Value}"
            ]
        if work.key.IsSome then
            Elem.span [] [ Text.raw $" in {work.key.Value}" ]
    ]

let private subtitle (work: Work) : XmlNode =
    Elem.div [ Attr.class' "card__subtitle" ] [
        if work.catalogueName.IsSome && work.catalogueNumber.IsSome then
            Elem.span [] [
                Text.raw $"{work.catalogueName.Value} {work.catalogueNumber.Value}"
                if work.cataloguePostfix.IsSome then
                    Text.raw work.cataloguePostfix.Value
            ]

            Elem.span [ Attr.class' "vertical-separator" ] []
        if work.yearStart.IsSome || work.yearFinish.IsSome then
            Elem.span [] [
                Text.raw (formatYearsRangeLoose work.yearStart work.yearFinish)
            ]

            Elem.span [ Attr.class' "vertical-separator" ] []
        if work.averageMinutes.IsSome then
            Elem.span [] [
                work.averageMinutes |> formatWorkLength |> Text.raw
            ]
    ]


/// Work card
let workCard (work: Work) =
    Elem.div [ Attr.class' "card" ] [
        title work
        subtitle work
    ]
