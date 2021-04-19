namespace SiteSaturn.Database.Providers

open System.Data
open Giraffe
open SiteSaturn.Database
open SiteSaturn.Models
open FSharp.Json

module Helpers =
    let extractorList<'a> (reader: IDataReader) =
        let hasValue = reader.Read()
        if hasValue then
            let res = reader.GetString(0)
            Json.deserialize<'a list> res
        else
            []
            
    let extractorSingle<'a> (reader: IDataReader) =
        let hasValue = reader.Read()
        if hasValue then
            let res = reader.GetString(0)
            res |> Json.deserialize<'a> |> Some
        else
            None
            
    let workMapper (reader: IDataReader) : Work list =
        [ while reader.Read() do
              yield {
                  id = reader.GetInt32(0)
                  title = reader.GetString(1)
                  yearStart = if reader.IsDBNull(2) then None else reader.GetInt32(2) |> Some
                  yearFinish = if reader.IsDBNull(3) then None else reader.GetInt32(3) |> Some
                  averageMinutes = if reader.IsDBNull(4) then None else reader.GetInt32(4) |> Some
                  catalogueName = if reader.IsDBNull(5) then None else reader.GetString(5) |> Some
                  catalogueNumber = if reader.IsDBNull(6) then None else reader.GetInt32(6) |> Some
                  cataloguePostfix = if reader.IsDBNull(7) then None else reader.GetString(7) |> Some
                  key = if reader.IsDBNull(8) then None else reader.GetString(8) |> Some
                  no = if reader.IsDBNull(9) then None else reader.GetInt32(9) |> Some
                  nickname = if reader.IsDBNull(10) then None else reader.GetString(10) |> Some
              } ]
    

module Periods =
    open Helpers
    
    let list : Async<Period list> =
        let sql = SqlRequests.periodsAndComposers
        query<Period list> sql None extractorList<Period>

module Composers =
    open Helpers
    
    let list (slug: string) : Async<Composer option> =
        let data = dict ["ComposerSlug", box slug]
        let sql = SqlRequests.composerBySlug
        query<Composer option> sql (Some data) extractorSingle<Composer>
        
module Works =
    open Helpers
    
    let get (id: int) : Async<Work list> =
        let data = dict ["Id", box id]
        let sql = SqlRequests.workById
        query<Work list> sql (Some data) workMapper
        
    let getChild (idParent: int) : Async<Work list> =
        let data = dict ["Id", box idParent]
        let sql = SqlRequests.childWorks
        query<Work list> sql (Some data) workMapper
       
module Genres =
    open Helpers
    
    let list (composerId: int) : Async<Genre list> =
        let data = dict ["ComposerId", box composerId]
        let sql = SqlRequests.genresAndWorksByComposer
        query<Genre list> sql (Some data) extractorList<Genre>
        
module Recordings =
    open Helpers
    
    let list (workId: int) : Async<Recording list> =
        let data = dict ["WorkId", box workId]
        let sql = SqlRequests.recordingsByWork
        query<Recording list> sql (Some data) extractorList<Recording>