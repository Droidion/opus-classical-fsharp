open System.IO
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.StaticFiles
open Microsoft.Extensions.Primitives
open Saturn
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Builder
open Site
open System

type CacheControl =
    | NoCacheControl
    | CacheControl of string

let useStaticFiles cache (app: IApplicationBuilder) =
    match cache with
    | NoCacheControl -> app.UseStaticFiles()
    | CacheControl value ->
        let handler (ctx: StaticFileResponseContext) =
            ctx.Context.Response.Headers.Add("Cache-Control", StringValues(value))

        let action = System.Action<StaticFileResponseContext>(handler)
        app.UseStaticFiles(StaticFileOptions(OnPrepareResponse = action))

let setWebRootPath path (builder: IWebHostBuilder) =
    let p = Path.Combine(Directory.GetCurrentDirectory(), path)

    builder.UseWebRoot(p)

/// Saturn app
let app =
    application {
        use_router Router.main
        url "http://0.0.0.0:5000"
        app_config (useStaticFiles (CacheControl "public, max-age=604800"))
        webhost_config (setWebRootPath "static")
        use_gzip
        memory_cache

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
    run app
    0
