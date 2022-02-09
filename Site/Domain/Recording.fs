module Site.Domain.Recording

open Site.Domain.Performer
open Site.Domain.Streamer
open Site.Helpers
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

/// Select recordings of certain work
let recordingsByWork = "select recordings_by_work(@WorkId) as json"

/// Returns recordings
let listRecordings (workId: int) : Async<string option> =
    let request =
        { Sql = recordingsByWork
          Parameters = dict [ "WorkId", box workId ] |> Some }

    query<string option> request extractSingleCell
