/// Saturn controllers.
module Site.Controllers

open FSharp.Json
open Giraffe
open Microsoft.AspNetCore.Http
open Saturn
open Site.Domain.Composer
open Site.Domain.ComposerSearchResult
open Site.Domain.Period
open Site.Domain.Recording
open Site.Domain.Work
open Site.Helpers
open Site.Templates.Pages

/// Index page controller.
let periodsController =
    let handler ctx =
        let periods = listPeriods ()
        Index.view periods |> Controller.renderHtml ctx

    controller { index handler }

/// Work page controller.
let workController composerSlug =
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

        let recordings = recordings.Head |> Json.deserialize<Recording list>

        match composer, work with
        | Some c, work when work.Length >= 1 -> Work.view c (List.head work) recordings childWorks
        | _, _ -> NotFound.view
        |> Controller.renderHtml ctx

    controller { show handler }

/// Composer page controller.
let composerController =
    let handler ctx slug =
        let composer = slug |> getComposer

        match composer with
        | Some c -> Composer.view c
        | None -> NotFound.view
        |> Controller.renderHtml ctx

    controller {
        subController "/work" workController
        show handler
    }

/// About page controller.
let aboutController =
    controller { index (fun ctx -> About.view |> Controller.renderHtml ctx) }

/// Search API controller.
let searchController =
    let handler (ctx: HttpContext) =
        match ctx.Request.Query.TryGetValue "q" with
        | true, x ->
            searchComposers(x.ToString(), 5)
            |> Async.RunSynchronously
            |> Controller.json ctx
        | _ ->
            ctx.SetStatusCode 400
            Controller.text ctx ""

    controller { index handler }
