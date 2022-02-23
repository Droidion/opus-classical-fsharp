/// Business logic for Genre.
module Site.Domain.Genre

open FSharp.Json
open Site.Domain.Work
open Site.Postgres

/// Genre of the work, like Symphony, or String Quartet, or Choral music.
type Genre =
    { name: string
      icon: string // e.g. 🐕
      works: Work list } // List of the works belonging to the genre.

/// Returns all genres
let listGenres (composerId: int) : Genre list =
    let sql = "select genres_and_works_by_composer(@ComposerId) as json"
    let parameters = [ "ComposerId", Sql.int composerId ] |> Some

    let json =
        query (sql, parameters, jsonMapper) |> Async.RunSynchronously

    json.Head |> Json.deserialize<Genre list>
