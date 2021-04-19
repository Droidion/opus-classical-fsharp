module SiteSaturn.Templates.Pages.Work

open Giraffe.ViewEngine
open SiteSaturn.Models
open SiteSaturn.Templates
open SiteSaturn.Templates.Partials

let view (composer: Composer) (work: Work) (recordings: Recording list) (childWorks: Work list) =

    let pageTitle = $"{work.title} - {composer.lastName}"
    let pageDescription = $"List of good {work.title} recordings composed by {composer.firstName} {composer.lastName} with direct links to the streaming apps."

    [ h1 [] [
        str work.title
        if work.no.IsSome then
            str $" No. {work.no.Value}"
        if work.catalogueName.IsSome && work.catalogueNumber.IsSome then
            str $""", {work.catalogueName.Value} {work.catalogueNumber.Value}{work.cataloguePostfix |> Option.defaultValue ""}"""
        if work.nickname.IsSome then
            str $": {work.nickname.Value}"
      ]
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
    |> App.view (pageTitle, pageDescription)
