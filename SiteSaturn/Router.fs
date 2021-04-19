module Site.Router

open Saturn
open SiteSaturn.Controllers

let keepAlive = pipeline {
    set_header "Connection" "Keep-Alive"
    set_header "Keep-Alive" "timeout=10, max=500"
    set_header "Strict-Transport-Security" "max-age=31536000; includeSubDomains"
}

let main =
    router {
        pipe_through keepAlive
        forward "" index
        forward "/about" about
        forward "/composer" composer
    }
