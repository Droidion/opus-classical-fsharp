module OpusClassicalWeb.Handlers

open Falco
open OpusClassicalWeb.Views
open OpusClassicalWeb.Database

let composersHandler: HttpHandler =
    fun ctx ->
        let periodsWithComposers = getPeriodsWithComposers ()

        let html =
            rootLayout "Composers | Opus Classical" (composersPage periodsWithComposers)

        Response.ofHtml html ctx
