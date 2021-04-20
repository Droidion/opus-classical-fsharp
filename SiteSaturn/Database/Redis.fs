/// Operations with Redis
module SiteSaturn.Database.Redis

open System
open StackExchange.Redis

/// Connection pool
let private redis =
    ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("RedisConnectionString"))

/// Single DB connection
let private db = redis.GetDatabase()

/// Save key-value to Redis
let storeRedis (key: string) (value: string) (life: TimeSpan) : bool =
    db.StringSet(RedisKey key, RedisValue value, life)

/// Get key-value from Redis
let retrieveRedis (key: string) : string option =
    match db.StringGet(RedisKey key) with
    | value when value.HasValue -> value |> string |> Some
    | _ -> None
