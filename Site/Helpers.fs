/// Generic system-wide helpers
module Site.Helpers

open System.Collections.Generic
open Sentry

/// Sends exception to Sentry
let exToSentry (ex: exn) (comment: string) =
    SentrySdk.AddBreadcrumb comment
    SentrySdk.CaptureException(ex) |> ignore
    None
    
type MaybeBuilder() =
    member this.Bind(x, f) =
        match x with
        | None -> None
        | Some a -> f a

    member this.Return(x) =
        Some x

/// maybe computation
let maybe = MaybeBuilder()

/// Converts F# option to C# nullable
let optionalToNullable<'T, 'N when 'T: null> (opt: 'T option) =
    match opt with
    | Some d -> d
    | None -> null
    
type PgRequest = {
    Sql: string
    Parameters: IDictionary<string, obj> option
}