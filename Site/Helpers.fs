/// Generic system-wide helpers
module Site.Helpers

open FSharp.Json
open Sentry

/// Sends exception to Sentry
let exToSentry (ex: exn) (comment: string) =
    SentrySdk.AddBreadcrumb comment
    SentrySdk.CaptureException(ex) |> ignore
    None

// create config with JSON field naming setting
let jsonConfig = JsonConfig.create(jsonFieldNaming = Json.snakeCase)