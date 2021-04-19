namespace SiteSaturn.Database

open System.Data
open Dapper
open Npgsql
open System.Collections.Generic

/// Database-related helpers
module Helpers =

    /// Database connection
    let mutable conn = ""

    // Makes simple SELECT to the database
    let query<'a>
        (sql: string)
        (data: IDictionary<string, obj> option)
        (mapper: IDataReader -> 'a)
        : Async<'a> =
        async {
            use conn = new NpgsqlConnection(conn)

            let data =
                match data with
                | Some d -> d
                | None -> null

            use! reader =
                conn.ExecuteReaderAsync(sql, data)
                |> Async.AwaitTask

            return mapper reader
        }