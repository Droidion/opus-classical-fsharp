/// Saturn controllers for web pages and API endpoints
module Site.Controllers

open Microsoft.AspNetCore.Http
open Saturn
open Site.Templates.Pages
open Site.Database.Providers
open FSharp.Json
open Site.Models
open Giraffe

/// Index page controller
let indexPageController: HttpHandler =
    let handler ctx =
        let periods = listPeriods ()
        Index.view periods |> Controller.renderHtml ctx

    controller { index handler }

/// Search API controller
let searchApiController: HttpHandler =
    let handler (ctx: HttpContext) =
        match ctx.Request.Query.TryGetValue "q" with
        | true, x ->
            searchComposers (x.ToString()) 5
            |> Async.RunSynchronously
            |> Controller.json ctx
        | _ ->
            ctx.SetStatusCode 400
            Controller.text ctx ""

    controller { index handler }

/// About page controller
let aboutPageController: HttpHandler =
    let handler ctx = About.view |> Controller.renderHtml ctx
    controller { index handler }

/// Controller for the Musical Work page 
let workPageController (composerSlug: string): HttpHandler =
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

/// Composer page controller
let composerPageController: HttpHandler =
    let handler ctx slug =
        let composer = slug |> getComposer

        let view =
            match composer with
            | Some c -> Composer.view c
            | None -> NotFound.view

        Controller.renderHtml ctx view

    controller {
        subController "/work" workPageController
        show handler
    }
