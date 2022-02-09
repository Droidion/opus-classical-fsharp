module Site.Domain.ComposerSearchResult

open System.Data
open Microsoft.AspNetCore.Http
open Site.Helpers
open Site.Postgres
open Giraffe
open Saturn

type ComposerSearchResult =
    { id: int
      firstName: string
      lastName: string
      slug: string
      rating: float }

/// Fuzzy search composers by last name with limiting results
let searchComposersByLastName =
    "select id, first_name, last_name, slug, last_name_score from search_composers_by_last_name(@SearchQuery, @Limit)"

/// Maps composers search results returned from Dapper to F# model
let composerSearchResultMapper (reader: IDataReader) : ComposerSearchResult list =
    [ while reader.Read() do
          yield
              { id = reader.GetInt32 0
                firstName = reader.GetString 1
                lastName = reader.GetString 2
                slug = reader.GetString 3
                rating = reader.GetDouble 4 } ]

/// Searches for composers by last name
let searchComposers (searchQuery: string) (limit: int) : Async<ComposerSearchResult list> =
    let request =
        { Sql = searchComposersByLastName
          Parameters =
              dict [ "SearchQuery", box searchQuery
                     "Limit", box limit ]
              |> Some }

    query<ComposerSearchResult list> request composerSearchResultMapper


/// Search API controller
let searchApiController: HttpFunc =
    let handler (ctx: HttpContext) =
        match ctx.Request.Query.TryGetValue "q" with
        | true, x ->
            searchComposers (x.ToString()) 5
            |> Async.RunSynchronously
            |> Controller.json ctx
        | _ ->
            ctx.SetStatusCode 400
            Controller.text ctx ""

    handler
