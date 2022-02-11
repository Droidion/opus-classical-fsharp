/// Business logic for Composer.
module Site.Domain.Composer

open FSharp.Json
open Giraffe
open Microsoft.AspNetCore.Http
open Site.Helpers
open Site.Postgres
open Site.Redis

/// Composer, like Bach or Beethoven
type Composer =
    { id: int
      firstName: string
      lastName: string
      yearBorn: int
      yearDied: int option
      countries: string list // Countries composer is associated with.
      slug: string // Unique composer readable text id, to be used in URLs.
      wikipediaLink: string option
      imslpLink: string option
      enabled: bool } // Show this composer on the main page

/// Returns composer by URL slug.
let getComposer (slug: string) : Composer option =
    let redisKey = "opusclassical:composer:" + slug

    match retrieveRedis redisKey with
    | Some c -> Json.deserialize<Composer> c |> Some
    | None ->
        let sql = "select composer_by_slug(@ComposerSlug) as json"
        let parameters = [ "ComposerSlug", Sql.text slug ] |> Some

        let json =
            query (sql, parameters, jsonMapper) |> Async.RunSynchronously

        storeRedis (redisKey, json.Head, expire.Long) |> ignore
        json.Head |> Json.deserialize<Composer> |> Some
