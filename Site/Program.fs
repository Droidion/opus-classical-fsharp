open Microsoft.AspNetCore.Builder
open Saturn
open Sentry
open Site
open System
open Falco.HostBuilder
open Falco.Extensions
open Site.Controllers

[<EntryPoint>]
let main args =
    use __ =
        SentrySdk.Init(Environment.GetEnvironmentVariable("SentryDsn"))

    webHost args {
        use_ifnot FalcoExtensions.IsDevelopment HstsBuilderExtensions.UseHsts
        use_https
        use_compression
        use_static_files
        add_antiforgery

        use_if    FalcoExtensions.IsDevelopment DeveloperExceptionPageExtensions.UseDeveloperExceptionPage
        use_ifnot FalcoExtensions.IsDevelopment (FalcoExtensions.UseFalcoExceptionHandler exceptionController)
        not_found notFoundController

        endpoints Router.endpoints
    }
    0
