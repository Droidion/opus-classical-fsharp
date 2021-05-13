open System.IO
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.StaticFiles
open Microsoft.Extensions.Primitives
open Saturn
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Builder
open Site
open System
open Sentry
open SiteSaturn.Templates

type private CacheControl =
    | NoCacheControl
    | CacheControl of string

let private configureCors (builder: CorsPolicyBuilder) =
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    |> ignore

let private useStaticFiles cache (app: IApplicationBuilder) =
    match cache with
    | NoCacheControl -> app.UseStaticFiles()
    | CacheControl value ->
        let handler (ctx: StaticFileResponseContext) =
            ctx.Context.Response.Headers.Add("Cache-Control", StringValues(value))

        let action = System.Action<StaticFileResponseContext>(handler)
        app.UseStaticFiles(StaticFileOptions(OnPrepareResponse = action))

let private setWebRootPath path (builder: IWebHostBuilder) =
    let p = Path.Combine(Directory.GetCurrentDirectory(), path)
    builder.UseWebRoot(p)

/// Saturn app
let private app =
    application {
        use_router Router.topRouter
        url "http://0.0.0.0:5000"
        app_config (useStaticFiles (CacheControl "public, max-age=604800"))
        webhost_config (setWebRootPath "static")
        use_gzip
        memory_cache
        use_cors "All" configureCors
        error_handler (fun _ _ -> pipeline { render_html Pages.Error.view })
        app_config
            (fun app ->
                let env = Environment.getWebHostEnvironment app

                if (env.IsDevelopment()) then
                    app.UseDeveloperExceptionPage()
                else
                    app)

    }

[<EntryPoint>]
let main _ =
    use __ =
        SentrySdk.Init(Environment.GetEnvironmentVariable("SentryDsn"))

    run app
    0
