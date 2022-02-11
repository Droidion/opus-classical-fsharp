/// Operations with Postgres.
module Site.Postgres

open Sentry
open System
open System.Data
open Npgsql.FSharp

let private connectionString = Environment.GetEnvironmentVariable("DbConnectionString")

/// Makes query to the Postgres database.
let query<'a> (sql: string, parameters: (string * SqlValue) list option, mapper: RowReader -> 'a) : Async<'a list> =
    try
        connectionString
        |> Sql.connect
        |> Sql.query sql
        |> Sql.parameters (
            if parameters.IsSome then
                parameters.Value
            else
                []
        )
        |> Sql.executeAsync mapper
        |> Async.AwaitTask
    with
    | ex ->
        SentrySdk.AddBreadcrumb "Problem with making Postgres request"
        SentrySdk.CaptureException(ex) |> ignore
        raise ex

/// Maps Postgres response to F# type for the case when single cell with JSON is returned. 
let jsonMapper (read: RowReader) : string = read.text "json"
