module OpusClassicalWeb.Environment

open DotNetEnv

Env.Load(".env") |> ignore

let connectionString: string = Env.GetString("CONNECTION_STRING", "")
