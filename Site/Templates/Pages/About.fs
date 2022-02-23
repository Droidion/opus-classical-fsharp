/// HTML for About page.
module Site.Templates.Pages.About

open Falco.Markup
open Site.Templates

let private howToUse: XmlNode =
    Elem.ul [] [
        Elem.li [] [
            Text.raw "Choose a composer: not every composer in history, but not only the most famous ones."
        ]
        Elem.li [] [
            Text.raw "Choose a work: not everything that was written, but not only the greatest hits."
        ]
        Elem.li [] [
            Text.raw "Choose a recording among several well-respected and newer ones."
        ]
        Elem.li [] [
            Text.raw "Click the icon for your streaming app to open the work you're examining."
        ]
    ]

let private supportedFunctionality: XmlNode =
    Elem.ul [] [
        Elem.li [] [
            Text.raw "Mobile and desktop web versions."
        ]
        Elem.li [] [
            Text.raw "Direct links to Spotify and Tidal."
        ]
        Elem.li [] [
            Text.raw "Direct links tested on MacOS, Windows and iOS."
        ]
    ]

let private support: XmlNode =
    Elem.p [] [
        Text.raw "Source code is available at "
        Elem.a [ Attr.href "https://github.com/Droidion/composers" ] [
            Text.raw "Github"
        ]
        Text.raw ". Create issues there. Or hit me up on "
        Elem.a [ Attr.href "https://twitter.com/droidion" ] [
            Text.raw "Twitter"
        ]
        Text.raw "."
    ]

let private futurePlans: XmlNode =
    Elem.ul [] [
        Elem.li [] [ Text.raw "120 composers" ]
        Elem.li [] [
            Text.raw "User accounts allowing users to add more recordings for private use"
        ]
        Elem.li [] [
            Text.raw "Public and private API"
        ]
        Elem.li [] [ Text.raw "Admin UI" ]
    ]

let private aboutPage (pageTitle: string) : XmlNode list =
    [ Elem.h1 [] [ Text.raw pageTitle ]
      Elem.p [] [
          Text.raw "Opus Classical is a curated catalogue of classical music composers, works and recordings. Opus Classical is currently in Early Preview."
      ]
      Elem.h2 [] [ Text.raw "How to use" ]
      howToUse
      Elem.h2 [] [
          Text.raw "Supported functionality"
      ]
      supportedFunctionality
      Elem.h2 [] [ Text.raw "Support" ]
      support
      Elem.h2 [] [ Text.raw "Future plans" ]
      futurePlans ]

let view: XmlNode =
    let pageTitle = "About"

    let pageDescription =
        "What is this service all about. Its current status, support, and future plans."

    aboutPage pageTitle |> App.view (pageTitle, pageDescription)
