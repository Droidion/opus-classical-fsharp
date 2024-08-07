module OpusClassicalWeb.Config

open FsConfig
open DotEnv.Core

type Config = {
    Port: int
    DatabaseUrl: string
}

let config: Config option ref = ref None

let loadConfig (): unit =
    EnvLoader().Load() |> ignore
    config.Value <-
        match EnvConfig.Get<Config>() with
        | Ok config -> Some config
        | Error error -> 
          match error with
          | NotFound envVarName -> 
            failwithf $"Environment variable %s{envVarName} not found"
          | BadValue (envVarName, value) ->
            failwithf $"Environment variable %s{envVarName} has invalid value %s{value}"
          | NotSupported msg -> 
            failwith msg

let getConfig (): Config =
    match config.Value with
    | Some config -> config
    | None -> printfn "Config not loaded"; exit 1
