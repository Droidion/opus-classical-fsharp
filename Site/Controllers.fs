/// Falco controllers.
module Site.Controllers

open FSharp.Json
open Falco
open Microsoft.AspNetCore.Http
open Site.Domain.Composer
open Site.Domain.ComposerSearchResult
open Site.Domain.Period
open Site.Domain.Recording
open Site.Domain.Work
open Site.Templates.Pages

/// Response headers for adding to all responses.
let private headers =
    [ ("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload")
      ("Content-Security-Policy",
       "default-src 'none'; manifest-src 'self'; connect-src 'self' https://logs.opusclassical.net; script-src 'self' https://logs.opusclassical.net; style-src 'self'; img-src 'self' https://static.zunh.dev")
      ("Referrer-Policy", "no-referrer")
      ("Permissions-Policy", "geolocation=(), microphone=()")
      ("Cache-Control", "private, max-age=0") ]

/// Index page controller.
let periodsController: HttpHandler =
    let routeMap (_: RouteCollectionReader) = listPeriods () |> Index.view

    Response.withHeaders headers
    >> Request.mapRoute routeMap Response.ofHtml

/// Composer page controller.
let composerController: HttpHandler =
    let routeMap (route: RouteCollectionReader) =
        let slug = route.GetString "slug" ""
        let composer = slug |> getComposer

        match composer with
        | Some c -> Composer.view c
        | None -> NotFound.view

    Response.withHeaders headers
    >> Request.mapRoute routeMap Response.ofHtml

/// Work page controller.
let workController: HttpHandler =
    let routeMap (route: RouteCollectionReader) =
        let slug = route.GetString "slug" ""
        let workId = route.GetInt "workId" 0
        let composer = getComposer slug

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

    Response.withHeaders headers
    >> Request.mapRoute routeMap Response.ofHtml

/// About page controller.
let aboutController: HttpHandler = Response.withHeaders headers >> Response.ofHtml About.view

/// Search API controller.
let searchController: HttpHandler =
    let routeMap (query: QueryCollectionReader) =
        let query = query.TryGet "q"

        match query with
        | Some q -> searchComposers (q, 5) |> Async.RunSynchronously
        | None -> []

    Response.withHeaders headers
    >> Request.mapQuery routeMap Response.ofJson

/// Error page controller.
let exceptionController: HttpHandler = Response.withHeaders headers >> Response.ofHtml Error.view

/// Page not found controller
let notFoundController: HttpHandler =
    Response.withHeaders headers >> Response.ofHtml NotFound.view
