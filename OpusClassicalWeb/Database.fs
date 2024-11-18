module OpusClassicalWeb.Database

open Npgsql.FSharp
open OpusClassicalWeb.Models
open OpusClassicalWeb.Environment

let allPeriodsQuery = "SELECT id, name, year_start, year_end, slug FROM periods"

let allPeriodsMapper (read : RowReader) : Period =
    {
        Id = read.int "id"
        Name = read.string "name"
        YearStart = read.int "year_start"
        YearEnd = read.intOrNone "year_end"
        Slug = read.string "slug"
    }

let getAllPeriods () : Period list =
    connectionString
    |> Sql.connect
    |> Sql.query allPeriodsQuery
    |> Sql.execute allPeriodsMapper