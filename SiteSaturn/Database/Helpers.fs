/// Database-related helpers
module SiteSaturn.Database.Helpers

open System
open System.Data
open Dapper
open Npgsql
open System.Collections.Generic
open SiteSaturn.Models

/// Database connection
let private connectionString = Environment.GetEnvironmentVariable("DbConnectionString")

/// Converts null string value returned from Postgres to option
let private extractNullableString (reader: IDataReader) (index: int) =
    match reader.IsDBNull(index) with
    | true -> None
    | false -> reader.GetString(index) |> Some

/// Converts null int value returned from Postgres to option
let private extractNullableInt (reader: IDataReader) (index: int) =
    match reader.IsDBNull(index) with
    | true -> None
    | false -> reader.GetInt32(index) |> Some

/// Extracts json as single returned string cell
let extractor (reader: IDataReader) =
    match reader.Read() with
    | true -> reader.GetString(0) |> Some
    | false -> None

/// Maps musical work data returned from Dapper to F# model
let workMapper (reader: IDataReader) : Work list =
    [ while reader.Read() do
          yield
              { id = reader.GetInt32 0
                title = reader.GetString 1
                yearStart = extractNullableInt reader 2
                yearFinish = extractNullableInt reader 3
                averageMinutes = extractNullableInt reader 4
                catalogueName = extractNullableString reader 5
                catalogueNumber = extractNullableInt reader 6
                cataloguePostfix = extractNullableString reader 7
                key = extractNullableString reader 8
                no = extractNullableInt reader 9
                nickname = extractNullableString reader 10 } ]

/// Makes simple SELECT to the database
let query<'a> (sql: string) (parameters: IDictionary<string, obj> option) (mapper: IDataReader -> 'a) : Async<'a> =
    async {
        use conn = new NpgsqlConnection(connectionString)

        let data =
            match parameters with
            | Some d -> d
            | None -> null

        use! reader = conn.ExecuteReaderAsync(sql, data) |> Async.AwaitTask

        let mapRes = mapper reader
        return mapRes
    }
