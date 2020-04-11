package hu.bme.aut.controllers

import hu.bme.aut.model.SeriesRating
import hu.bme.aut.services.SeriesRatingService
import io.ktor.application.call
import io.ktor.http.HttpStatusCode.Companion.Created
import io.ktor.http.HttpStatusCode.Companion.NoContent
import io.ktor.http.HttpStatusCode.Companion.NotFound
import io.ktor.http.HttpStatusCode.Companion.OK
import io.ktor.request.receive
import io.ktor.response.respond
import io.ktor.routing.*

fun Route.seriesRatings(service: SeriesRatingService) {
    route("/SeriesRatings") {

        get("/") {
            call.respond(OK, service.findAll())
        }

        get("/{id}") {
            val id = call.parameters["id"]?.toIntOrNull()
            checkNotNull(id) { "The id parameter must be an integer" }
            val rating = service.findById(id)
            if (rating == null)
                call.respond(NotFound)
            else
                call.respond(OK, rating)
        }

        get("/find") {
            val userId = call.request.queryParameters["userId"]?.toIntOrNull()
            val seriesId = call.request.queryParameters["seriesId"]
            val ratings = service.findByUserIdOrSeriesId(userId, seriesId)
            call.respond(OK, ratings)
        }

        post("/") {
            val seriesData = call.receive<SeriesRating>()
            service.insert(seriesData)
            call.respond(Created)
        }

        put("/{id}") {
            val id = call.parameters["id"]?.toIntOrNull()
            checkNotNull(id) { "The id parameter must be an integer" }
            val seriesData = call.receive<SeriesRating>()
            service.update(id, seriesData)
            call.respond(NoContent)
        }

        delete("/{id}") {
            val id = call.parameters["id"]?.toIntOrNull()
            checkNotNull(id) { "The id parameter must be an integer" }
            service.delete(id)
            call.respond(NoContent)
        }
    }
}