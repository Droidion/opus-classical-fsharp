namespace SiteSaturn.Database.Providers

open System
open System.Data
open Giraffe
open SiteSaturn.Database
open SiteSaturn.Models
open FSharp.Json

module Helpers =
    let extractor (reader: IDataReader) =
        let hasValue = reader.Read()
        if hasValue then
            reader.GetString(0) |> Some
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
    
    let list : Period list =
        let sql = SqlRequests.periodsAndComposers
        let redisKey = "opusclassical:periods"
        let cached = Redis.retrieveRedis redisKey
        match cached with
        | Some c -> Json.deserialize<Period list> c
        | None ->
            let json = query<string option> sql None extractor |> Async.RunSynchronously
            if json.IsSome then 
                Redis.storeRedis redisKey json.Value (TimeSpan(1, 0, 0)) |> ignore
                json.Value |> Json.deserialize<Period list>
            else
                []
        

module Composers =
    open Helpers
    
    let get (slug: string) : Composer option =
        let data = dict ["ComposerSlug", box slug]
        let sql = SqlRequests.composerBySlug
        let redisKey = $"opusclassical:composer:{slug}"
        let cached = Redis.retrieveRedis redisKey
        match cached with
        | Some c -> Json.deserialize<Composer> c |> Some
        | None ->
            let json = query<string option> sql (Some data) extractor |> Async.RunSynchronously
            if json.IsSome then
                Redis.storeRedis redisKey json.Value (TimeSpan(0, 1, 0)) |> ignore
                json.Value |> Json.deserialize<Composer> |> Some
            else
                None
        
        
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
    
    let list (composerId: int) : Genre list =
        let data = dict ["ComposerId", box composerId]
        let sql = SqlRequests.genresAndWorksByComposer
        let json = query<string option> sql (Some data) extractor |> Async.RunSynchronously
        if json.IsSome then
            json.Value |> Json.deserialize<Genre list>
        else
            []
        
module Recordings =
    open Helpers
    
    let list (workId: int) : Recording list =
        let data = dict ["WorkId", box workId]
        let sql = SqlRequests.recordingsByWork
        let json = query<string option> sql (Some data) extractor |> Async.RunSynchronously
        if json.IsSome then
            json.Value |> Json.deserialize<Recording list>
        else
            []