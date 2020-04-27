package hu.bme.aut.ratings.controllers

import hu.bme.aut.ratings.dtos.AverageOfRatingsResponse
import hu.bme.aut.ratings.dtos.SeriesRatingData
import hu.bme.aut.ratings.dtos.SeriesRatingInfo
import hu.bme.aut.ratings.model.SeriesRating
import hu.bme.aut.ratings.services.SeriesRatingService
import io.ktor.application.call
import io.ktor.http.HttpStatusCode.Companion.Created
import io.ktor.http.HttpStatusCode.Companion.NoContent
import io.ktor.http.HttpStatusCode.Companion.NotFound
import io.ktor.http.HttpStatusCode.Companion.OK
import io.ktor.request.receive
import io.ktor.response.respond
import io.ktor.routing.*

fun Route.seriesRatings(service: SeriesRatingService) {
    route("/api/SeriesRatings") {

        get("/") {
            val userId = call.request.queryParameters["userId"]?.toIntOrNull()
            val seriesId = call.request.queryParameters["seriesId"]
            val ratings: List<SeriesRatingInfo> = service.findByUserIdAndSeriesId(userId, seriesId)
                .map { it.toSeriesRatingInfo() }
            call.respond(OK, ratings)
        }

        get("/{id}") {
            val id = call.parameters["id"]?.toIntOrNull()
            checkNotNull(id) { "The id parameter must be an integer" }
            val rating: SeriesRatingInfo? = service.findById(id)?.toSeriesRatingInfo()
            if (rating == null)
                call.respond(NotFound)
            else
                call.respond(OK, rating)
        }

        get("/Series/{seriesId}/Average") {
            val seriesId = call.parameters["seriesId"]
            checkNotNull(seriesId)
            val result: AverageOfRatingsResponse = service.getAverage(seriesId)
            call.respond(OK, result)
        }

        post("/") {
            val seriesData = call.receive<SeriesRatingData>()
            service.insert(seriesData)
            call.respond(Created)
        }

        put("/{id}") {
            val id = call.parameters["id"]?.toIntOrNull()
            checkNotNull(id) { "The id parameter must be an integer" }
            val seriesData = call.receive<SeriesRatingData>()
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

fun SeriesRating.toSeriesRatingInfo() = SeriesRatingInfo(
    this.id,
    this.userId,
    this.seriesId,
    this.rating,
    this.opinion
)