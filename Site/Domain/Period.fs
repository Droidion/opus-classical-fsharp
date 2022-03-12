/// Business logic for Period.
module Site.Domain.Period

open FSharp.Json
open Site.Domain.Composer
open Site.Postgres
open Site.Redis

/// Redis key for caching main page content
let redisKey = "opusclassical:periods"

/// Period when composer lived and worked, e.g. Late Baroque or Romanticism.
type Period =
    { id: int
      name: string
      yearStart: int
      yearEnd: int option
      slug: string // Unique period readable text id, to be used in URLs.
      composers: Composer list }

/// Returns all periods
let listPeriods () : Period list =
    match retrieveRedis redisKey with
    | Some c -> Json.deserialize<Period list> c
    | None ->
        let sql = "select json from periods_composers"
        let parameters = None

        let json =
            query (sql, parameters, jsonMapper) |> Async.RunSynchronously

        storeRedis (redisKey, json.Head, expire.Long) |> ignore

        json.Head |> Json.deserialize<Period list>
