package hu.bme.aut.controllers

import hu.bme.aut.services.SeriesRatingService
import io.ktor.application.call
import io.ktor.http.HttpStatusCode
import io.ktor.response.respond
import io.ktor.routing.Route
import io.ktor.routing.get
import io.ktor.routing.route

fun Route.seriesRatings(service: SeriesRatingService) {
    route("/SeriesRatings") {

        get("/") {
            call.respond(HttpStatusCode.OK, service.findAll())
        }
    }
}