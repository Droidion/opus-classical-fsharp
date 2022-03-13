/// HTML for Work page.
module Site.Templates.Pages.Work

open Falco.Markup
open Site.Domain.Composer
open Site.Domain.Performer
open Site.Domain.Recording
open Site.Domain.Streamer
open Site.Domain.Work
open Site.Templates
open Site.Templates.Helpers
open Site.Templates.Partials

let private performerTitle (performer: Performer) : XmlNode =
    Elem.div [ Attr.class' "card__title" ] [
        Elem.span [] [
            Text.raw $"""{performer.firstName |> Option.defaultValue ""} {performer.lastName} """
        ]
        if performer.instrument.IsSome then
            Elem.span [ Attr.class' "card__instrument" ] [
                Text.raw performer.instrument.Value
            ]
    ]

let private performerSubtitle (recording: Recording) : XmlNode =
    Elem.div [ Attr.class' "card__subtitle" ] [
        if recording.label.IsSome then
            Elem.span [] [
                Text.raw recording.label.Value
            ]

            Elem.span [ Attr.class' "vertical-separator" ] []
        if recording.yearStart.IsSome || recording.yearFinish.IsSome then
            Elem.span [] [
                Text.raw (formatYearsRangeLoose recording.yearStart recording.yearFinish)
            ]

            Elem.span [ Attr.class' "vertical-separator" ] []
        Elem.span [] [
            recording.length |> Some |> formatWorkLength |> Text.raw
        ]
    ]

let private streamerCard (streamer: Streamer) : XmlNode =
    let loggedClick = streamer.name.Replace(" ", "_") + "_" + streamer.link.Replace("/", "_")
    Elem.div [ Attr.class' "card__streamer-el" ] [
        Elem.a [ Attr.href $"{streamer.prefix}{streamer.link}"
                 Attr.class' $"umami--click--{loggedClick}" ] [
            Elem.img [ Attr.src $"/img/{streamer.icon}"
                       Attr.height "24"
                       Attr.width "24"
                       Attr.alt "Streaming service logo" ]
        ]
    ]

let private recordingCard (recording: Recording) =
    Elem.div [ Attr.class' "card illustrated" ] [
        Elem.img [ Attr.class' "cover"
                   Attr.src $"{coversUrl}{recording.coverName}"
                   Attr.alt "Cover" ]
        Elem.div [] [
            for performer in recording.performers do
                performerTitle performer
            performerSubtitle recording

            Elem.div [ Attr.class' "card__streamers" ] [
                for streamer in recording.streamers do
                    streamerCard streamer
            ]
        ]
    ]

let private workTitle (work: Work) : XmlNode list =
    [ Text.raw work.title
      if work.no.IsSome then
          Text.raw $" No. {work.no.Value}"
      if work.catalogueName.IsSome && work.catalogueNumber.IsSome then
          Text.raw $""", {work.catalogueName.Value} {work.catalogueNumber.Value}{work.cataloguePostfix |> Option.defaultValue ""}"""
      if work.nickname.IsSome then
          Text.raw $": {work.nickname.Value}" ]

let private workSubtitle (composer: Composer) (work: Work) : XmlNode =
    let yearsComposed = formatYearsRangeLoose work.yearStart work.yearFinish

    Elem.div [ Attr.class' "header-subtitle" ] [
        Elem.span [] [
            Elem.a [ Attr.href $"/composer/{composer.slug}" ] [
                Text.raw $"{composer.firstName} {composer.lastName}"
            ]
        ]
        if yearsComposed <> "" then
            Elem.span [] [
                Text.raw (", " + formatYearsRangeLoose work.yearStart work.yearFinish)
            ]
    ]

let private workPage (composer: Composer) (work: Work) (recordings: Recording list) (childWorks: Work list) : XmlNode list =
    [ Elem.h1 [] (workTitle work)
      workSubtitle composer work
      if childWorks.Length > 0 then
          Elem.h2 [] [
              Text.raw "Individual Works"
          ]

          Elem.hr []

          Elem.div [ Attr.class' "card-list" ] [
              for work in childWorks do
                  workCard work
          ]
      Elem.h2 [] [
          Text.raw "Recommended Recordings"
      ]
      Elem.hr []
      Elem.div [ Attr.class' "card-list full-width" ] [
          for recording in recordings do
              recordingCard recording
      ] ]

let view (composer: Composer) (work: Work) (recordings: Recording list) (childWorks: Work list) : XmlNode =

    let pageTitle = $"{work.title} - {composer.lastName}"

    let pageDescription =
        $"List of good {work.title} recordings composed by {composer.firstName} {composer.lastName} with direct links to the streaming apps."

    workPage composer work recordings childWorks
    |> App.view (pageTitle, pageDescription)
