module SiteSaturn.Controllers

open Saturn
open SiteSaturn.Templates.Pages
open SiteSaturn.Database.Providers

let index =
    let handler ctx =
        let periods = Periods.list |> Async.RunSynchronously
        Index.view periods |> Controller.renderHtml ctx

    controller { index handler }

let about =
    let handler ctx = About.view |> Controller.renderHtml ctx
    controller { index handler }

let work composerSlug =
    let handler ctx workId =
        let composer =
            composerSlug
            |> Composers.list
            |> Async.RunSynchronously

        let work =
            workId |> Works.get |> Async.RunSynchronously

        let recordings =
            workId
            |> Recordings.list
            |> Async.RunSynchronously

        let childWorks =
            workId |> Works.getChild |> Async.RunSynchronously

        let view =
            match composer, work with
            | Some c, work when work.Length >= 1 -> Work.view c (List.head work) recordings childWorks
            | _, _ -> NotFound.view

        Controller.renderHtml ctx view

    controller { show handler }

let composer =
    let handler ctx slug =
        let composer =
            slug |> Composers.list |> Async.RunSynchronously

        let view =
            match composer with
            | Some c -> Composer.view c
            | None -> NotFound.view

        Controller.renderHtml ctx view

    controller {
        subController "/work" work
        show handler
    }
