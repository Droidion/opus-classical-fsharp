module OpusClassicalWeb.Handlers

open Falco
open OpusClassicalWeb.Views
open OpusClassicalWeb.Database

let homeHandler: HttpHandler =
    fun ctx ->
        let periods = getAllPeriods ()
        Response.ofHtml (rootLayout periods) ctx