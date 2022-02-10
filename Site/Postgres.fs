/// Operations with Postgres
module Site.Postgres

open Dapper
open Npgsql
open Sentry
open System
open System.Collections.Generic
open System.Data

type PgRequest = {
    Sql: string
    Parameters: IDictionary<string, obj> option
}

/// Converts F# option to C# nullable
let optionalToNullable<'T, 'N when 'T: null> (opt: 'T option) =
    match opt with
    | Some d -> d
    | None -> null

let private connectionString = Environment.GetEnvironmentVariable("DbConnectionString")

/// Converts null string value returned from Postgres to option
let extractNullableString (reader: IDataReader) (index: int) : string option =
    match reader.IsDBNull(index) with
    | true -> None
    | false -> reader.GetString(index) |> Some

/// Converts null int value returned from Postgres to option
let extractNullableInt (reader: IDataReader) (index: int) : int option =
    match reader.IsDBNull(index) with
    | true -> None
    | false -> reader.GetInt32(index) |> Some

/// Extracts json as single returned string cell 
let extractSingleCell (reader: IDataReader) : string option =
    match reader.Read() with
    | true -> reader.GetString(0) |> Some
    | false -> None

/// Makes simple SELECT to the database
let query<'a> (request: PgRequest) (resultMapper: IDataReader -> 'a) : Async<'a> =
    async {
        try
            use conn = new NpgsqlConnection(connectionString)
            let parameters = optionalToNullable request.Parameters
            use! reader = conn.ExecuteReaderAsync(request.Sql, parameters) |> Async.AwaitTask
            return resultMapper reader
        with ex ->
            SentrySdk.AddBreadcrumb "Problem with making Postgres request"
            SentrySdk.CaptureException(ex) |> ignore
            return raise ex
    }

/// Queries database for a single text cell (with supposed JSON value there)
let querySingleTextCell (request: PgRequest) : string option =
    query<string option> request extractSingleCell
    |> Async.RunSynchronously