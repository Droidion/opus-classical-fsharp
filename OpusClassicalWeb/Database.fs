module OpusClassicalWeb.Database

open OpusClassicalWeb.Config
open Npgsql.FSharp

type Country = {
    Id: int
    Name: string
}

let getAllCountries () : Country list =
    getConfig().DatabaseUrl
    |> Sql.connect
    |> Sql.query "SELECT * FROM countries"
    |> Sql.execute (fun read ->
        {
            Id = read.int "id"
            Name = read.text "name"
        })