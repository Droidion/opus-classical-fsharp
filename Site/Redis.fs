/// Operations with Redis
module Site.Redis

open System
open StackExchange.Redis
open Site.Helpers

type RedisExpiration = { Soon: TimeSpan; Long: TimeSpan }
let expire : RedisExpiration = { Soon = TimeSpan(0, 1, 0); Long = TimeSpan(1, 0, 0) }

/// Connection pool
let private redisPool : ConnectionMultiplexer option =
    try
        let pool =
            ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("RedisConnectionString"))

        Some pool
    with ex -> exToSentry ex "Problem with creating Redis connection pool"

/// Single DB connection
let private redisConn : IDatabase option =
    match redisPool with
    | Some r ->
        try
            let db = r.GetDatabase()
            Some db
        with ex -> exToSentry ex "Problem with opening Redis database"
    | None -> None

/// Save key-value to Redis
let storeRedis (key: string, value: string, life: TimeSpan) : bool =
    match redisConn with
    | Some db -> db.StringSet(RedisKey key, RedisValue value, life)
    | None -> false

/// Get key-value from Redis
let retrieveRedis (key: string) : string option =
    match redisConn with
    | Some db ->
        let value = db.StringGet(RedisKey key)

        match value.HasValue with
        | true -> value |> string |> Some
        | false -> None
    | None -> None
