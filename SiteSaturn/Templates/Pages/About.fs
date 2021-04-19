module SiteSaturn.Templates.Pages.About

open Giraffe.ViewEngine
open SiteSaturn.Templates

let view =
    let pageTitle = "About"
    let pageDescription = "What is this service all about. Its current status, support, and future plans."

    [ h1 [] [ str pageTitle ]
      p [] [
          str "Opus Classical is a curated catalogue of classical music composers, works and recordings. Opus Classical is currently in Early Preview."
      ]

      h2 [] [ str "How to use" ]
      ul [] [
          li [] [
              str "Choose a composer: not every composer in history, but not only the most famous ones."
          ]
          li [] [
              str "Choose a work: not everything that was written, but not only the greatest hits."
          ]
          li [] [
              str "Choose a recording among several well-respected and newer ones."
          ]
          li [] [
              str "Click the icon for your streaming app to open the work you're examining."
          ]
      ]

      h2 [] [ str "Supported functionality" ]
      ul [] [
          li [] [
              str "Mobile and desktop web versions."
          ]
          li [] [
              str "Direct links to Spotify and Tidal."
          ]
          li [] [
              str "Direct links tested on MacOS, Windows and iOS."
          ]
      ]

      h2 [] [ str "Support" ]
      p [] [
          str "Source code is available at "
          a [ _href "https://github.com/Droidion/composers" ] [
              str "Github"
          ]
          str ". Create issues there. Or hit me up on "
          a [ _href "https://twitter.com/droidion" ] [
              str "Twitter"
          ]
          str "."
      ]

      h2 [] [ str "Future plans" ]
      ul [] [
          li [] [ str "120 composers" ]
          li [] [
              str "User accounts allowing users to add more recordings for private use"
          ]
          li [] [ str "Public and private API" ]
          li [] [ str "Admin UI" ]
      ]

      ]
    |> App.view (pageTitle, pageDescription)
