open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open OpusClassicalWeb.Config
open OpusClassicalWeb.Router

let configureApp (app: IApplicationBuilder) = app.UseGiraffe(webApp ())

let configureServices (services: IServiceCollection) = services.AddGiraffe() |> ignore

[<EntryPoint>]
let main _ =
    loadConfig ()
    let config = getConfig ()

    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .Configure(configureApp)
                .ConfigureServices(configureServices)
                .UseUrls
                $"http://0.0.0.0:%d{config.Port}"
            |> ignore)
        .Build()
        .Run()

    0
