module SiteSaturn.Database.Redis

open System
open StackExchange.Redis

let redis =
    ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("DbConnectionString"))

let db = redis.GetDatabase()

let storeRedis (key: string) (value: string) (life: TimeSpan) =
    db.StringSet(RedisKey key, RedisValue value, life)

let retrieveRedis (key: string) =
    let result = db.StringGet(RedisKey key)

    if result.HasValue then
        result |> string |> Some
    else
        None
