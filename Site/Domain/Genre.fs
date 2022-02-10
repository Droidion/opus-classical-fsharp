module Site.Domain.Genre

open FSharp.Json
open Site.Domain.Work
open Site.Helpers
open Site.Postgres

/// Genre of the work, like Symphony, or String Quartet, or Choral music.
type Genre = {
    name: string
    icon: string // e.g. 🐕
    works: Work list // List of the works belonging to the genre.
}

/// Select works grouped by genres by composer Id
let genresAndWorksByComposer = "select genres_and_works_by_composer(@ComposerId) as json"

/// Returns all genres
let listGenres (composerId: int) : Genre list =
    let request =
        { Sql = genresAndWorksByComposer
          Parameters = dict [ "ComposerId", box composerId ] |> Some }

    match querySingleTextCell request with
    | Some json -> Json.deserializeEx<Genre list> jsonConfig json 
    | None -> []