module SiteSaturn.Controllers

open Saturn
open SiteSaturn.Templates.Pages
open SiteSaturn.Database.Providers
open FSharp.Json
open SiteSaturn.Models

let index =
    let handler ctx =
        let periods = listPeriods ()
        Index.view periods |> Controller.renderHtml ctx

    controller { index handler }

let about =
    let handler ctx = About.view |> Controller.renderHtml ctx
    controller { index handler }

let work composerSlug =
    let handler ctx workId =
        let composer = composerSlug |> getComposer

        let parallelData =
            async {
                let! work = Async.StartChild(workId |> getWorks)
                let! recordings = Async.StartChild(workId |> listRecordings)
                let! childWorks = Async.StartChild(workId |> getChildWorks)
                let! workResult = work
                let! recordingsResult = recordings
                let! childWorksResult = childWorks
                return workResult, recordingsResult, childWorksResult
            }

        let work, recordings, childWorks = parallelData |> Async.RunSynchronously

        let recordings =
            match recordings.IsSome with
            | true -> recordings.Value |> Json.deserialize<Recording list>
            | false -> []

        let view =
            match composer, work with
            | Some c, work when work.Length >= 1 -> Work.view c (List.head work) recordings childWorks
            | _, _ -> NotFound.view

        Controller.renderHtml ctx view

    controller { show handler }

let composer =
    let handler ctx slug =
        let composer = slug |> getComposer

        let view =
            match composer with
            | Some c -> Composer.view c
            | None -> NotFound.view

        Controller.renderHtml ctx view

    controller {
        subController "/work" work
        show handler
    }
