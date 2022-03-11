/// HTML snippets.
module Site.Templates.Partials

open Site.Domain.Work
open Site.Templates.Helpers
open Falco.Markup

let private title (work: Work) : XmlNode =
    Elem.div [] [
        Elem.span [] [ Text.raw work.title ]
        if work.no.IsSome then
            Elem.span [] [
                Text.raw $" No. {work.no.Value}"
            ]
        if work.nickname.IsSome then
            Elem.tag "cite" [] [ Text.raw $" {work.nickname.Value}" ]
        if work.key.IsSome then
            Elem.span [] [
                Text.raw $" in {work.key.Value}"
            ]
    ]

let private subtitle (work: Work) : XmlNode =
    let subtitle =
        [ formatCatalogueName (work.catalogueName, work.catalogueNumber, work.cataloguePostfix)
          formatYearsRangeLoose work.yearStart work.yearFinish
          work.averageMinutes |> formatWorkLength ]
        |> Seq.filter (fun x -> x <> "")
        |> String.concat "<span class=\"vertical-separator\"></span>"

    Elem.div [ Attr.class' "card__subtitle" ] [
        Text.raw subtitle
    ]


/// Work card
let workCard (work: Work) =
    Elem.div [ Attr.class' "card" ] [
        title work
        subtitle work
    ]
