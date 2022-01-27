module Site.Templates.Pages.Work

open Giraffe.ViewEngine
open Site.Models
open Site.Templates
open Site.Templates.Partials
open Site.Templates.Helpers

let private performerTitle (performer: Performer) : XmlNode =
    div [ _class "card__title" ] [
        span [] [
            str $"""{performer.firstName |> Option.defaultValue ""} {performer.lastName} """
        ]
        if performer.instrument.IsSome then
            span [ _class "card__instrument" ] [
                str performer.instrument.Value
            ]
    ]

let private performerSubtitle (recording: Recording) : XmlNode =
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

let private streamerCard (streamer: Streamer) : XmlNode =
    div [ _class "card__streamer-el" ] [
        a [ _href $"{streamer.prefix}{streamer.link}" ] [
            img [ _src $"/img/{streamer.icon}"
                  _height "24"
                  _width "24"
                  _alt "Streaming service logo" ]
        ]
    ]

let private recordingCard (recording: Recording) =
    div [ _class "card illustrated" ] [
        img [ _class "cover"
              _src $"{coversUrl}{recording.coverName}"
              _alt "Cover" ]
        div [] [
            for performer in recording.performers do
                performerTitle performer
            performerSubtitle recording

            div [ _class "card__streamers" ] [
                for streamer in recording.streamers do
                    streamerCard streamer
            ]
        ]
    ]

let private workTitle (work: Work) : XmlNode list =
    [ str work.title
      if work.no.IsSome then
          str $" No. {work.no.Value}"
      if work.catalogueName.IsSome && work.catalogueNumber.IsSome then
          str $""", {work.catalogueName.Value} {work.catalogueNumber.Value}{work.cataloguePostfix |> Option.defaultValue ""}"""
      if work.nickname.IsSome then
          str $": {work.nickname.Value}" ]

let private workSubtitle (composer: Composer) (work: Work) : XmlNode =
    div [ _class "header-subtitle" ] [
        span [] [
            a [ _href $"/composer/{composer.slug}" ] [
                str $"{composer.firstName} {composer.lastName}"
            ]
            str ", "
        ]
        span [] [
            str (formatYearsRangeLoose work.yearStart work.yearFinish)
        ]
    ]

let private workPage (composer: Composer) (work: Work) (recordings: Recording list) (childWorks: Work list) : XmlNode list =
    [ h1 [] (workTitle work)
      workSubtitle composer work
      if childWorks.Length > 0 then
          h2 [] [ str "Individual Works" ]
          hr []

          div [ _class "card-list" ] [
              for work in childWorks do
                  workCard work
          ]
      h2 [] [ str "Recommended Recordings" ]
      hr []
      div [ _class "card-list full-width" ] [
          for recording in recordings do
              recordingCard recording
      ] ]

let view (composer: Composer) (work: Work) (recordings: Recording list) (childWorks: Work list) : XmlNode =

    let pageTitle = $"{work.title} - {composer.lastName}"

    let pageDescription =
        $"List of good {work.title} recordings composed by {composer.firstName} {composer.lastName} with direct links to the streaming apps."

    workPage composer work recordings childWorks
    |> App.view (pageTitle, pageDescription)
