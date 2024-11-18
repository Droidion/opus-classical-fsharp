module OpusClassicalWeb.Environment

open DotNetEnv

let connectionString: string =
    Env.Load(".env") |> ignore
    Env.GetString ("CONNECTION_STRING", "")