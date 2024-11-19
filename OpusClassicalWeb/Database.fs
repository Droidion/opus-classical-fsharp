module OpusClassicalWeb.Database

open Npgsql.FSharp
open OpusClassicalWeb.Models
open OpusClassicalWeb.Environment

let private periodMapper (read: RowReader) : Period =
    { Id = read.int "id"
      Name = read.string "name"
      YearStart = read.int "year_start"
      YearEnd = read.intOrNone "year_end"
      Slug = read.string "slug" }

let private allPeriodsQuery =
    "SELECT id, name, year_start, year_end, slug FROM periods"

let getAllPeriods () : Period list =
    connectionString
    |> Sql.connect
    |> Sql.query allPeriodsQuery
    |> Sql.execute periodMapper

let private composerMapper (read: RowReader) : Composer =
    { Id = read.int "id"
      FirstName = read.string "first_name"
      LastName = read.string "last_name"
      YearBorn = read.int "year_born"
      YearDied = read.intOrNone "year_died"
      PeriodId = read.int "period_id"
      Slug = read.string "slug"
      WikipediaLink = read.stringOrNone "wikipedia_link"
      ImslpLink = read.stringOrNone "imslp_link"
      Enabled = read.bool "enabled"
      Countries = read.string "countries" }

let private allComposersQuery =
    "SELECT id, first_name, last_name, year_born, year_died, period_id, slug, wikipedia_link, imslp_link, enabled, countries FROM composers_with_countries ORDER BY last_name"

let private composerBySlug: string =
    "SELECT id, first_name, last_name, year_born, year_died, period_id, slug, wikipedia_link, imslp_link, enabled, countries FROM composers_with_countries WHERE slug = @slug"

let getAllComposers () : Composer list =
    connectionString
    |> Sql.connect
    |> Sql.query allComposersQuery
    |> Sql.execute composerMapper

let getComposerBySlug (slug: string) : Composer option =
    connectionString
    |> Sql.connect
    |> Sql.query composerBySlug
    |> Sql.parameters [ "slug", Sql.string slug ]
    |> Sql.execute composerMapper
    |> List.tryHead

let getPeriodsWithComposers () : PeriodWithComposers list =
    let periods = getAllPeriods ()
    let composers = getAllComposers ()

    periods
    |> List.map (fun period ->
        { Period = period
          Composers = composers |> List.filter (fun composer -> composer.PeriodId = period.Id) })
