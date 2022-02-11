/// Generic system-wide helpers
module Site.Helpers

open Sentry

/// Sends exception to Sentry
let exToSentry (ex: exn) (comment: string) =
    SentrySdk.AddBreadcrumb comment
    SentrySdk.CaptureException(ex) |> ignore
    None
