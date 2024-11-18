module OpusClassicalWeb.Views

open Falco.Markup
open OpusClassicalWeb.Models

let rootLayout (periods: Period list) : XmlNode =
    Elem.html
        [ Attr.lang "en" ]
        [ Elem.head [] [ Elem.link [ Attr.rel "stylesheet"; Attr.href "/css/output.css" ] ]
          Elem.body
              [ Attr.class' "bg-red-500" ]
              [ Elem.h1 [] [ Text.raw "Sample App" ]
                Elem.ul [] (periods |> List.map (fun period -> Elem.li [] [ Text.raw period.Name ])) ] ]
