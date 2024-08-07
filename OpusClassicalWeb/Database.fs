module OpusClassicalWeb.Database

open OpusClassicalWeb.Config
open Npgsql.FSharp

type Country = { Id: int; Name: string }

let mapToCountry (read: RowReader) : Country =
    { Id = read.int "id"
      Name = read.text "name" }

let getAllCountries () : Result<Country list, string> =
    try
        let countries =
            getConfig().DatabaseUrl
            |> Sql.connect
            |> Sql.query "SELECT * FROM countries"
            |> Sql.execute mapToCountry

        Ok countries
    with
    | :? Npgsql.NpgsqlException as ex -> Error $"Database error: {ex.Message}"
    | ex -> Error $"An unexpected error occurred: {ex.Message}"
