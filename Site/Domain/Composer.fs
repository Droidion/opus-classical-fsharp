module Site.Domain.Composer

open Microsoft.AspNetCore.Http
open Site.Helpers
open Site.Redis
open Site.Postgres
open FSharp.Json
open Giraffe

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

/// Select Composer by its slug name
let private composerBySlug = "select composer_by_slug(@ComposerSlug) as json"

/// Returns composer
let getComposer (slug: string) : Composer option =
    let redisKey = "opusclassical:composer:" + slug

    match retrieveRedis redisKey with
    | Some c -> Json.deserialize<Composer> c |> Some
    | None ->
        let request =
            { Sql = composerBySlug
              Parameters = dict [ "ComposerSlug", box slug ] |> Some }

        match querySingleTextCell request with
        | Some json ->
            storeRedis (redisKey, json, expire.Soon) |> ignore
            json |> Json.deserialize<Composer> |> Some
        | None -> None


