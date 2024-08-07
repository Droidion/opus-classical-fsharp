module OpusClassicalWeb.Router

open Giraffe
open Giraffe.ViewEngine
open OpusClassicalWeb.Database

let indexView =
    let countries = 
        match getAllCountries() with
        | Ok countriesList -> countriesList
        | Error _ -> []

    html [] [
        head [] [
            title [] [ str "Giraffe Sample" ]
        ]
        body [] [
            h1 [] [ str "I |> F#" ]
            p [ _class "some-css-class"; _id "someId" ] [
                str "Hello World"
            ]
            ul [] [
                for country in countries do
                    li [] [
                        str $"ID: %d{country.Id}, Name: %s{country.Name}"
                    ]
            ]
        ]
    ]

let webApp () =
    choose [
        route "/ping"   >=> text "pong"
        route "/"       >=> htmlView indexView ]