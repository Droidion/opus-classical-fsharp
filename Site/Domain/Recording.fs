/// Business logic for Recordings.
module Site.Domain.Recording

open Site.Domain.Performer
open Site.Domain.Streamer
open Site.Postgres

/// Recording of a musical work.
type Recording =
    { id: int
      coverName: string
      yearStart: int option
      yearFinish: int option
      performers: Performer list
      label: string option
      length: int
      streamers: Streamer list }

/// Returns recordings of a given work.
let listRecordings (workId: int) : Async<string list> =
    let sql = "SELECT recordings_by_work(@WorkId) AS json"
    let parameters = [ "WorkId", Sql.int workId ] |> Some
    query(sql, parameters, jsonMapper)
    

