/// Endpoints for retrieving data
module Site.Database.Providers

open System
open Giraffe
open Site.Database
open Site.Models
open Site.Helpers
open FSharp.Json
open Postgres

type private RedisExpiration = { Soon: TimeSpan; Long: TimeSpan }
let private expire : RedisExpiration = { Soon = TimeSpan(0, 1, 0); Long = TimeSpan(1, 0, 0) }

type private RedisKeys = { Periods: string; Composer: string }

let private redisKeys : RedisKeys =
    { Periods = "opusclassical:periods"
      Composer = "opusclassical:composer:" }

/// Queries database for a single text cell (with supposed JSON value there)
let private querySingleTextCell (request: PgRequest) : string option =
    query<string option> request extractSingleCell
    |> Async.RunSynchronously

/// Returns all periods
let listPeriods () : Period list =
    match Redis.retrieveRedis redisKeys.Periods with
    | Some c -> Json.deserialize<Period list> c
    | None ->
        let request = { Sql = SqlRequests.periodsAndComposers; Parameters = None }

        match querySingleTextCell request with
        | Some json ->
            Redis.storeRedis redisKeys.Periods json expire.Long |> ignore
            json |> Json.deserialize<Period list>
        | None -> []

/// Returns composer
let getComposer (slug: string) : Composer option =
    let redisKey = redisKeys.Composer + slug

    match Redis.retrieveRedis redisKey with
    | Some c -> Json.deserialize<Composer> c |> Some
    | None ->
        let request =
            { Sql = SqlRequests.composerBySlug
              Parameters = dict [ "ComposerSlug", box slug ] |> Some }

        match querySingleTextCell request with
        | Some json ->
            Redis.storeRedis redisKey json expire.Soon |> ignore
            json |> Json.deserialize<Composer> |> Some
        | None -> None

/// Searches for composers by last name
let searchComposers (searchQuery: string) (limit: int) : Async<ComposerSearchResult list> =
    let request =
        { Sql = SqlRequests.searchComposersByLastName
          Parameters =
              dict [ "SearchQuery", box searchQuery
                     "Limit", box limit ]
              |> Some }

    query<ComposerSearchResult list> request composerSearchResultMapper

/// Returns works by work id
let getWorks (id: int) : Async<Work list> =
    let request =
        { Sql = SqlRequests.workById
          Parameters = dict [ "Id", box id ] |> Some }

    query<Work list> request workMapper

/// Returns child works by its parent id
let getChildWorks (idParent: int) : Async<Work list> =
    let request =
        { Sql = SqlRequests.childWorks
          Parameters = dict [ "Id", box idParent ] |> Some }

    query<Work list> request workMapper

/// Returns all genres
let listGenres (composerId: int) : Genre list =
    let request =
        { Sql = SqlRequests.genresAndWorksByComposer
          Parameters = dict [ "ComposerId", box composerId ] |> Some }

    match querySingleTextCell request with
    | Some json -> Json.deserialize<Genre list> json
    | None -> []

/// Returns recordings
let listRecordings (workId: int) : Async<string option> =
    let request =
        { Sql = SqlRequests.recordingsByWork
          Parameters = dict [ "WorkId", box workId ] |> Some }

    query<string option> request extractSingleCell
