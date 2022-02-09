module Site.Domain.Period

open Site.Helpers
open Site.Postgres
open Site.Domain.Composer
open Site.Redis
open FSharp.Json
open Giraffe

/// Period when composer lived and worked, e.g. Late Baroque or Romanticism. 
type Period = {
    id: int
    name: string
    yearStart: int
    yearEnd: int option
    slug: string // Unique period readable text id, to be used in URLs.
    composers: Composer list
}

/// Select composers grouped by music periods
let private periodsAndComposers = "select json from periods_composers"

let private redisKey = "opusclassical:periods"

/// Returns all periods
let listPeriods () : Period list =
    match retrieveRedis redisKey with
    | Some c -> Json.deserialize<Period list> c
    | None ->
        let request = { Sql = periodsAndComposers; Parameters = None }

        match querySingleTextCell request with
        | Some json ->
            storeRedis(redisKey, json, expire.Long) |> ignore
            json |> Json.deserialize<Period list>
        | None -> []
        