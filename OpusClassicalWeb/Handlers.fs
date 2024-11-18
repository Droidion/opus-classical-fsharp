module OpusClassicalWeb.Handlers

open Falco
open OpusClassicalWeb.Views
open OpusClassicalWeb.Database

let homeHandler: HttpHandler =
    fun ctx ->
        let periods = getAllPeriods ()
        let html = rootLayout ("Opus Classical") (homePage periods)
        Response.ofHtml html ctx
