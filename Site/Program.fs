open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.StaticFiles
open Microsoft.Extensions.Primitives
open Sentry
open Site
open System
open Falco.HostBuilder
open Falco.Extensions
open Site.Controllers

/// Middleware that adds caching header to static assets
let private staticFilesMiddleware (app: IApplicationBuilder) : IApplicationBuilder =
    app.UseStaticFiles(
        StaticFileOptions(
            OnPrepareResponse =
                System.Action<StaticFileResponseContext>(fun ctx -> ctx.Context.Response.Headers.Add("Cache-Control", StringValues("public, max-age=604800")))
        )
    )

[<EntryPoint>]
let main args =
    use __ =
        SentrySdk.Init(Environment.GetEnvironmentVariable("SentryDsn"))

    webHost args {
        use_ifnot FalcoExtensions.IsDevelopment HstsBuilderExtensions.UseHsts
        use_compression
        use_middleware staticFilesMiddleware
        add_antiforgery

        use_if FalcoExtensions.IsDevelopment DeveloperExceptionPageExtensions.UseDeveloperExceptionPage
        use_ifnot FalcoExtensions.IsDevelopment (FalcoExtensions.UseFalcoExceptionHandler exceptionController)
        not_found notFoundController

        endpoints Router.endpoints
    }

    0
