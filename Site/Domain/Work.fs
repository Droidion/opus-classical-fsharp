/// Business logic for Work.
module Site.Domain.Work

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
let private workById =
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
let private childWorks =
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

/// Maps Postgres response to F# type
let private mapper (read: RowReader) : Work =
    { id = read.int "id"
      title = read.text "title"
      yearStart = read.intOrNone "year_start"
      yearFinish = read.intOrNone "year_finish"
      averageMinutes = read.intOrNone "average_minutes"
      catalogueName = read.textOrNone "catalogue_name"
      catalogueNumber = read.intOrNone "catalogue_number"
      cataloguePostfix = read.textOrNone "catalogue_postfix"
      key = read.textOrNone "key"
      no = read.intOrNone "no"
      nickname = read.textOrNone "nickname" }

/// Returns works by work id
let getWorks (id: int) : Async<Work list> =
    query<Work> (workById, [ "Id", Sql.int id ] |> Some, mapper)

/// Returns child works by its parent id
let getChildWorks (idParent: int) : Async<Work list> =
    query<Work> (childWorks, [ "Id", Sql.int idParent ] |> Some, mapper)
