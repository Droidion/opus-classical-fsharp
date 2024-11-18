module OpusClassicalWeb.Svg

open Falco.Markup
open Falco.Markup.Svg

let githubIcon: XmlNode =
    Elem.svg
        [ Attr.viewBox (sprintf "%i %i %f %f" 0 0 438.549 438.549)
          Attr.xmlns "http://www.w3.org/2000/svg"
          Attr.height "36"
          Attr.width "36"
          Attr.class' "icon h-8 w-12" ]
        [ Elem.path
              [ Attr.d
                    "m409.132 114.573c-19.608-33.596-46.205-60.194-79.798-79.8-33.598-19.607-70.277-29.408-110.063-29.408-39.781 0-76.472 9.804-110.063 29.408-33.596 19.605-60.192 46.204-79.8 79.8-19.605 33.595-29.408 70.281-29.408 110.057 0 47.78 13.94 90.745 41.827 128.906 27.884 38.164 63.906 64.572 108.063 79.227 5.14.954 8.945.283 11.419-1.996 2.475-2.282 3.711-5.14 3.711-8.562 0-.571-.049-5.708-.144-15.417-.098-9.709-.144-18.179-.144-25.406l-6.567 1.136c-4.187.767-9.469 1.092-15.846 1-6.374-.089-12.991-.757-19.842-1.999-6.854-1.231-13.229-4.086-19.13-8.559-5.898-4.473-10.085-10.328-12.56-17.556l-2.855-6.57c-1.903-4.374-4.899-9.233-8.992-14.559-4.093-5.331-8.232-8.945-12.419-10.848l-1.999-1.431c-1.332-.951-2.568-2.098-3.711-3.429-1.142-1.331-1.997-2.663-2.568-3.997-.572-1.335-.098-2.43 1.427-3.289s4.281-1.276 8.28-1.276l5.708.853c3.807.763 8.516 3.042 14.133 6.851 5.614 3.806 10.229 8.754 13.846 14.842 4.38 7.806 9.657 13.754 15.846 17.847 6.184 4.093 12.419 6.136 18.699 6.136s11.704-.476 16.274-1.423c4.565-.952 8.848-2.383 12.847-4.285 1.713-12.758 6.377-22.559 13.988-29.41-10.848-1.14-20.601-2.857-29.264-5.14-8.658-2.286-17.605-5.996-26.835-11.14-9.235-5.137-16.896-11.516-22.985-19.126-6.09-7.614-11.088-17.61-14.987-29.979-3.901-12.374-5.852-26.648-5.852-42.826 0-23.035 7.52-42.637 22.557-58.817-7.044-17.318-6.379-36.732 1.997-58.24 5.52-1.715 13.706-.428 24.554 3.853 10.85 4.283 18.794 7.952 23.84 10.994 5.046 3.041 9.089 5.618 12.135 7.708 17.705-4.947 35.976-7.421 54.818-7.421s37.117 2.474 54.823 7.421l10.849-6.849c7.419-4.57 16.18-8.758 26.262-12.565 10.088-3.805 17.802-4.853 23.134-3.138 8.562 21.509 9.325 40.922 2.279 58.24 15.036 16.18 22.559 35.787 22.559 58.817 0 16.178-1.958 30.497-5.853 42.966-3.9 12.471-8.941 22.457-15.125 29.979-6.191 7.521-13.901 13.85-23.131 18.986-9.232 5.14-18.182 8.85-26.84 11.136-8.662 2.286-18.415 4.004-29.263 5.146 9.894 8.562 14.842 22.077 14.842 40.539v60.237c0 3.422 1.19 6.279 3.572 8.562 2.379 2.279 6.136 2.95 11.276 1.995 44.163-14.653 80.185-41.062 108.068-79.226 27.88-38.161 41.825-81.126 41.825-128.906-.01-39.771-9.818-76.454-29.414-110.049z" ]
              [ Elem.title [] [ Text.raw "Github icon" ] ] ]

let opusclassicalIcon: XmlNode =
    Elem.svg
        [ Attr.viewBox (sprintf "%i %i %f %f" 0 0 248.99 242.17)
          Attr.xmlns "http://www.w3.org/2000/svg"
          Attr.height "72"
          Attr.width "72"
          Attr.class' "logo icon h-9 w-9 xl:h-16 xl:w-16" ]
        [ Elem.title [] [ Text.raw "Main Logo Icon" ]
          Elem.path
              [ Attr.d
                    "m159.14,5.33c-4.04,3.07-8.01,5.97-11.83,9.06-1.75,1.42-3.33,1.82-5.64,1.35-4.95-1.02-9.99-1.72-15.02-2.18-5.94-.54-11.79.65-17.6,1.77-11.79,2.27-23.03,6.09-33.33,12.37-17.28,10.53-27.82,25.67-31.35,45.68-2.93,16.6-4.53,33.3-4.07,50.16.29,10.54,1.2,21.04,4,31.28.12.44.13,1.04-.06,1.43-1.5,3.01-3.06,5.99-4.79,9.33-6.87-11.69-9.88-23.87-9.67-36.95-5.03-1.94-8.93-4.91-9.19-10.84-.26-5.93,3.35-9.26,8.17-11.7-.46-5.84.44-11.6,1.13-17.36.78-6.46,1.67-12.91,2.76-19.32,1.53-9.06,5.05-17.43,9.59-25.36C54.92,21.88,73.99,8.32,98.79,2.86c19.83-4.36,39.58-3.78,59.1,2.02.36.11.72.26,1.24.45Z" ]
              []
          Elem.path
              [ Attr.d
                    "m220.55,70.79c5.8,1.96,11.29,4.38,15.37,9.06,5.4,6.19,9.66,13.11,12.65,20.79.25.63.46,1.35.42,2.01-.44,9.04-.84,18.08-1.44,27.11-.29,4.3-.96,8.59-1.5,12.87-.65,5.21-2.89,9.7-6.46,13.48-6.6,6.99-14.62,11.51-24.07,13.42-.53.11-1.1.17-1.63.1-1.52-.2-1.95-1.04-1.33-2.41,3.52-7.67,7.04-15.37,8.79-23.67.81-3.83,1.11-7.76,1.54-11.66.1-.9.36-1.38,1.17-1.79,3.75-1.95,6.65-4.74,7.92-8.88,1.87-6.07-.73-12.61-6.59-16.51-1.07-.71-1.53-1.44-1.63-2.7-.48-6.49-.99-12.99-1.63-19.47-.38-3.84-1.03-7.66-1.59-11.75Z" ]
              []
          Elem.path
              [ Attr.d
                    "m28.26,70.83c-2.07,10.63-2.66,21.14-3.41,31.64-.07.95-.42,1.47-1.22,1.99-3.72,2.43-6.39,5.69-7.24,10.15-1.13,5.91.98,10.73,5.88,13.93,2.59,1.69,3.65,3.4,3.81,6.49.46,8.89,3.39,17.19,6.93,25.27,1.06,2.41,2.14,4.8,3.23,7.2.58,1.28.28,1.98-1.13,2.11-.92.08-1.9-.02-2.8-.25-9.69-2.5-18.03-7.21-24.45-15.03-3.52-4.29-4.8-9.4-5.47-14.74C.89,127.64.33,115.62,0,103.59c-.03-1.2.22-2.48.67-3.6,2.96-7.41,7.12-14.1,12.35-20.11,4.01-4.61,9.4-7.03,15.24-9.05Z" ]
              []
          Elem.path
              [ Attr.d
                    "m184.06,35.8c3.76-2.63,7.33-5.13,11.1-7.77,1.51,1.8,3.06,3.52,4.48,5.35,7.01,9.05,12.28,19.05,15.26,30.1,1.25,4.62,1.76,9.46,2.4,14.23.95,7.19,1.78,14.4,2.53,21.61.23,2.2.04,4.45.04,6.73,5.12,2.55,8.79,6.16,8.19,12.38-.53,5.52-4.31,8.34-9.16,10.16.23,12.95-2.74,25.07-9.51,36.72-1.69-3.29-3.23-6.26-4.72-9.25-.16-.31-.12-.79-.02-1.16,3.33-12.45,4.13-25.17,4.09-37.98-.04-16.1-1.63-32.06-5.03-47.8-2.78-12.9-9.15-23.85-18.88-32.8-.18-.17-.41-.29-.75-.54Z" ]
              []
          Elem.path
              [ Attr.d
                    "m55.05,184.14c.3,1.08.71,2.14.9,3.23.95,5.68,4.35,9.32,9.43,11.64,5.44,2.48,11.21,3.71,17.09,4.48,5.22.69,10.5,1.03,15.68,1.92,2.84.49,5.67,1.53,8.28,2.79,3.99,1.94,6.31,5.4,6.59,9.82.32,5.05.25,10.13.23,15.2,0,2.38-.29,4.76-.47,7.14-.04.58-.16,1.16-.24,1.75-1.55-.36-1.99-1.1-2.1-3.14-.17-3.33-.26-6.66-.58-9.98-.2-2.07-.71-4.1-1.16-6.14-.76-3.47-2.97-5.8-6.25-6.82-3.97-1.23-8.03-2.25-12.1-3.06-7.01-1.4-14.09-2.48-21.1-3.89-2.11-.42-4.17-1.34-6.1-2.32-7.59-3.85-10.23-12.06-9.11-19.43.16-1.06.3-2.12.45-3.19l.56-.02Z" ]
              []
          Elem.path
              [ Attr.d
                    "m193.32,184.19c.57,2.96.91,5.93.53,8.96-1.26,10.11-8.33,14.62-16.44,16.25-8.18,1.64-16.43,2.94-24.62,4.51-2.47.47-4.9,1.25-7.28,2.07-3.32,1.15-5.66,3.38-6.36,6.91-.63,3.21-1.12,6.45-1.49,9.7-.26,2.27-.15,4.57-.33,6.85-.1,1.19-.22,2.55-2.16,2.72-.22-3.63-.59-7.19-.62-10.75-.04-4.62,0-9.25.37-13.84.37-4.45,2.97-7.58,6.89-9.65,3.96-2.09,8.25-2.8,12.64-3.19,7.62-.68,15.26-1.39,22.59-3.71,2.68-.85,5.32-1.97,7.79-3.32,3.72-2.02,6.02-5.25,6.88-9.46.28-1.39.72-2.74,1.08-4.11l.54.07Z" ]
              []
          Elem.path
              [ Attr.d
                    "m158.74,152.4c.18,2.42-.34,4.18-1.43,5.79-3.99,5.92-9.58,9.84-16.17,12.19-9.33,3.33-18.57,2.59-27.53-1.59-1.02-.48-1.97-1.16-2.87-1.85-.91-.7-.66-1.49.34-1.76,1.08-.29,2.26-.46,3.37-.37,14.4,1.15,27.73-2.07,40.03-9.61.89-.54,1.68-1.24,2.54-1.83.46-.31.97-.54,1.73-.95Z" ]
              []
          Elem.path
              [ Attr.d
                    "m93,115.37c-5.15-.25-9.89-.98-13.85-4.16-1.87-1.5-3.34-3.3-3.33-5.88,0-2.1.78-2.62,2.72-1.92,6.21,2.26,12.58,3.8,19.21,3.56,3.26-.12,6.49-.88,9.74-1.23.96-.1,1.97.21,2.95.33-.45.85-.75,1.84-1.37,2.54-3.41,3.78-7.8,5.71-12.77,6.45-1.18.18-2.37.23-3.3.32Z" ]
              []
          Elem.path
              [ Attr.d
                    "m155.6,115.35c-5.61-.18-10.72-1.66-14.97-5.45-.88-.78-1.64-1.73-2.32-2.7-.76-1.1-.44-1.71.92-1.6,1.97.16,3.94.48,5.89.84,6.61,1.24,13.06.38,19.45-1.35,2.01-.54,3.98-1.22,5.93-1.96.94-.36,1.64-.08,1.71.71.12,1.33.21,2.81-.27,4-1.08,2.67-3.41,4.2-5.93,5.38-3.3,1.55-6.84,2-10.41,2.13Z" ]
              []
          Elem.path
              [ Attr.d
                    "m79.42,76.24c1.68-7.32,8.59-13.66,16.3-14.86,1.63-.25,3.39-.25,4.99.11,2.35.53,3.17,2.5,2.27,4.75-1.04,2.62-3.15,4.27-5.8,4.23-5.91-.09-10.85,2.49-15.89,4.94-.53.26-1.06.54-1.59.81-.04.02-.1.01-.28.03Z" ]
              []
          Elem.path
              [ Attr.d
                    "m169.49,76.3c-1.57-.78-3.05-1.65-4.64-2.27-2.94-1.16-5.91-2.27-8.93-3.21-1.15-.36-2.46-.14-3.7-.25-2.83-.26-5.03-1.53-6.19-4.21-1.01-2.34-.1-4.44,2.39-4.91,1.75-.33,3.68-.31,5.43.04,8.1,1.62,12.87,6.98,15.7,14.4.03.07-.03.18-.08.42Z" ]
              []
          Elem.path
              [ Attr.d
                    "m166.02,23.68c4.22-2.99,8.22-5.84,12.26-8.63.33-.23,1.06-.22,1.41,0,1.63,1.03,3.2,2.17,4.9,3.35-.33.31-.53.56-.79.74-3.5,2.45-6.99,4.91-10.52,7.31-.41.28-1.16.46-1.55.28-1.86-.89-3.66-1.93-5.71-3.04Z" ]
              [] ]