module Site.Domain.Work

open Site.Helpers
open Site.Postgres
open System.Data

/// Musical work, like Symphony No. 9 by Beethoven
type Work =
    { id: int
      title: string
      yearStart: int option // Year when composer started the work, if known. Can be used without YearFinish if the work was finished in a single year.
      yearFinish: int option // Year when composer finished the work, if known. Can be used without YearStart if the work was finished in a single year.
      averageMinutes: int option // Approximate length of the work in minutes.
      catalogueName: string option // Name of the catalogue of composer's works, like "BWV" for Bach or "Op." for Beethoven.
      catalogueNumber: int option // Catalogue number of the work, like 123 for Op. 123
      cataloguePostfix: string option // Postfix for the number of the work in the catalogue, like b in Op. 123b
      key: string option // e.g. C# minor
      no: int option // Work number in some sequence, like 9 in Symphony No. 9
      nickname: string option } // e.g. Great in Beethoven's Symphony No. 9 Great

/// Select work by its id
let workById =
    "
    select w.id,
           w.title,
           w.year_start,
           w.year_finish,
           w.average_minutes,
           c.name catalogue_name,
           w.catalogue_number ,
           w.catalogue_postfix,
           k.name as key,
           w.no,
           w.nickname
    from works w
             left join catalogues c on w.catalogue_id = c.id
             left join keys k on w.key_id = k.id
    where w.id = @Id"

/// Select works by their parent work Id
let childWorks =
    @"
    select w.id,
           w.title,
           w.year_start,
           w.year_finish,
           w.average_minutes,
           c.name as catalogue_name,
           w.catalogue_number,
           w.catalogue_postfix,
           k.name as key,
           w.no,
           w.nickname
    from works w
             left join catalogues c on w.catalogue_id = c.id
             left join keys k on w.key_id = k.id
    where w.parent_work_id = @Id
    order by sort, year_finish, no, catalogue_number, catalogue_postfix, nickname"

/// Maps musical work data returned from Dapper to F# model
let workMapper (reader: IDataReader) : Work list =
    [ while reader.Read() do
          yield
              { id = reader.GetInt32 0
                title = reader.GetString 1
                yearStart = extractNullableInt reader 2
                yearFinish = extractNullableInt reader 3
                averageMinutes = extractNullableInt reader 4
                catalogueName = extractNullableString reader 5
                catalogueNumber = extractNullableInt reader 6
                cataloguePostfix = extractNullableString reader 7
                key = extractNullableString reader 8
                no = extractNullableInt reader 9
                nickname = extractNullableString reader 10 } ]

/// Returns works by work id
let getWorks (id: int) : Async<Work list> =
    let request =
        { Sql = workById
          Parameters = dict [ "Id", box id ] |> Some }

    query<Work list> request workMapper

/// Returns child works by its parent id
let getChildWorks (idParent: int) : Async<Work list> =
    let request =
        { Sql = childWorks
          Parameters = dict [ "Id", box idParent ] |> Some }

    query<Work list> request workMapper
