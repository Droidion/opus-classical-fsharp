/// Business logic for searching for composers.
module Site.Domain.ComposerSearchResult

open Site.Postgres
open System.Data

/// Search result for a composer.
type ComposerSearchResult =
    { id: int
      firstName: string
      lastName: string
      slug: string
      rating: float }

/// Fuzzy search composers by last name with limiting results
let private searchComposersByLastName =
    "select id, first_name, last_name, slug, last_name_score from search_composers_by_last_name(@SearchQuery, @Limit)"

/// Maps Postgres response to F# type
let private mapper (read: RowReader) : ComposerSearchResult =
    { id = read.int "id"
      firstName = read.text "first_name"
      lastName = read.text "last_name"
      slug = read.text "slug"
      rating = read.double "last_name_score" }

/// Searches for composers by last name and output items limit
let searchComposers (searchQuery: string, limit: int) : Async<ComposerSearchResult list> =
    let parameters =
        [ "SearchQuery", Sql.text searchQuery
          "Limit", Sql.int limit ]
        |> Some

    query<ComposerSearchResult>(searchComposersByLastName, parameters, mapper)
    