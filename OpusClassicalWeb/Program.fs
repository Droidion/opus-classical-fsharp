open Falco
open Falco.Routing
open Falco.HostBuilder
open Microsoft.Extensions.Logging
open OpusClassicalWeb.Handlers

let configureLogging (log : ILoggingBuilder) : ILoggingBuilder =
    log.ClearProviders() |> ignore
    log.AddConsole() |> ignore
    log

let router : HttpEndpoint list =
    [
        get "/" homeHandler
    ]

[<EntryPoint>]
let main (args : string array) : int =
    webHost args {
        add_antiforgery
        use_static_files
        logging configureLogging
        endpoints router
    }
    0