package hu.bme.aut.controllers

import hu.bme.aut.services.SeriesRatingService
import io.ktor.application.call
import io.ktor.http.HttpStatusCode
import io.ktor.http.HttpStatusCode.Companion.NotFound
import io.ktor.http.HttpStatusCode.Companion.OK
import io.ktor.response.respond
import io.ktor.routing.Route
import io.ktor.routing.get
import io.ktor.routing.route

fun Route.seriesRatings(service: SeriesRatingService) {
    route("/SeriesRatings") {

        get("/") {
            call.respond(OK, service.findAll())
        }

        get("/{id}") {
            val id = call.parameters["id"]
            checkNotNull(id)
            val rating = service.findById(id.toInt())
            if (rating == null)
                call.respond(NotFound)
            else
                call.respond(OK, rating)
        }
    }
}