/// Endpoints for retrieving data
module SiteSaturn.Database.Providers

open System
open Giraffe
open SiteSaturn.Database
open SiteSaturn.Models
open FSharp.Json
open Postgres

/// Returns all periods
let listPeriods : Period list =
    let sql = SqlRequests.periodsAndComposers
    let redisKey = "opusclassical:periods"
    let cached = Redis.retrieveRedis redisKey

    match cached with
    | Some c -> Json.deserialize<Period list> c
    | None ->
        let json =
            query<string option> sql None extractSingleCell |> Async.RunSynchronously

        match json.IsSome with
        | true ->
            Redis.storeRedis redisKey json.Value (TimeSpan(1, 0, 0)) |> ignore
            json.Value |> Json.deserialize<Period list>
        | false -> []

/// Returns composer
let getComposer (slug: string) : Composer option =
    let data = dict [ "ComposerSlug", box slug ]
    let sql = SqlRequests.composerBySlug
    let redisKey = $"opusclassical:composer:{slug}"
    let cached = Redis.retrieveRedis redisKey

    match cached with
    | Some c -> Json.deserialize<Composer> c |> Some
    | None ->
        let json =
            query<string option> sql (Some data) extractSingleCell
            |> Async.RunSynchronously

        match json.IsSome with
        | true ->
            Redis.storeRedis redisKey json.Value (TimeSpan(0, 1, 0)) |> ignore
            json.Value |> Json.deserialize<Composer> |> Some
        | false -> None

/// Returns works by work id
let getWorks (id: int) : Async<Work list> =
    let data = dict [ "Id", box id ]
    let sql = SqlRequests.workById
    query<Work list> sql (Some data) workMapper

/// Returns child works by its parent id
let getChildWorks (idParent: int) : Async<Work list> =
    let data = dict [ "Id", box idParent ]
    let sql = SqlRequests.childWorks
    query<Work list> sql (Some data) workMapper

/// Returns all genres
let listGenres (composerId: int) : Genre list =
    let data = dict [ "ComposerId", box composerId ]
    let sql = SqlRequests.genresAndWorksByComposer

    let json =
        query<string option> sql (Some data) extractSingleCell
        |> Async.RunSynchronously

    match json.IsSome with
    | true -> json.Value |> Json.deserialize<Genre list>
    | false -> []

/// Returns recordings
let listRecordings (workId: int) : Async<string option> =
    let data = dict [ "WorkId", box workId ]
    let sql = SqlRequests.recordingsByWork
    query<string option> sql (Some data) extractSingleCell
