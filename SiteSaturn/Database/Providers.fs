/// Endpoints for retrieving data
module SiteSaturn.Database.Providers

open System
open System.Collections.Generic
open Giraffe
open SiteSaturn.Database
open SiteSaturn.Models
open FSharp.Json
open Postgres

type private RedisExpiration = { Soon: TimeSpan; Long: TimeSpan; }
let private expire: RedisExpiration = {
    Soon = TimeSpan(0, 1, 0)
    Long = TimeSpan(1, 0, 0)
}

type private RedisKeys = {
    Periods: string
    Composer: string
}
let private redisKeys: RedisKeys = {
    Periods = "opusclassical:periods"
    Composer = "opusclassical:composer:"
}

/// Queries database for a single text cell (with supposed JSON value there)
let private querySingleTextCell (sql: string) (parameters: IDictionary<string, obj> option): string option =
    query<string option> sql parameters extractSingleCell |> Async.RunSynchronously

/// Returns all periods
let listPeriods () : Period list =
    let sql = SqlRequests.periodsAndComposers
    match Redis.retrieveRedis redisKeys.Periods with
    | Some c -> Json.deserialize<Period list> c
    | None ->
        match querySingleTextCell sql None with
        | Some json ->
            Redis.storeRedis redisKeys.Periods json expire.Long |> ignore
            json |> Json.deserialize<Period list>
        | None -> []

/// Returns composer
let getComposer (slug: string) : Composer option =
    let sql = SqlRequests.composerBySlug
    let parameters = dict [ "ComposerSlug", box slug ]
    let redisKey = redisKeys.Composer + slug

    match Redis.retrieveRedis redisKey with
    | Some c -> Json.deserialize<Composer> c |> Some
    | None ->
        match querySingleTextCell sql (Some parameters) with
        | Some json ->
            Redis.storeRedis redisKey json expire.Soon |> ignore
            json |> Json.deserialize<Composer> |> Some
        | None -> None

/// Searches for composers by last name
let searchComposers (searchQuery: string) (limit: int) : Async<ComposerSearchResult list> =
    let sql = SqlRequests.searchComposersByLastName
    let parameters =
        dict [ "SearchQuery", box searchQuery
               "Limit", box limit ]

    query<ComposerSearchResult list> sql (Some parameters) composerSearchResultMapper

/// Returns works by work id
let getWorks (id: int) : Async<Work list> =
    let sql = SqlRequests.workById
    let parameters = dict [ "Id", box id ]
    query<Work list> sql (Some parameters) workMapper

/// Returns child works by its parent id
let getChildWorks (idParent: int) : Async<Work list> =
    let sql = SqlRequests.childWorks
    let parameters = dict [ "Id", box idParent ]
    query<Work list> sql (Some parameters) workMapper

/// Returns all genres
let listGenres (composerId: int) : Genre list =
    let sql = SqlRequests.genresAndWorksByComposer
    let parameters = dict [ "ComposerId", box composerId ]
    let json = querySingleTextCell sql (Some parameters)

    match json.IsSome with
    | true -> json.Value |> Json.deserialize<Genre list>
    | false -> []

/// Returns recordings
let listRecordings (workId: int) : Async<string option> =
    let parameters = dict [ "WorkId", box workId ]
    let sql = SqlRequests.recordingsByWork
    query<string option> sql (Some parameters) extractSingleCell
