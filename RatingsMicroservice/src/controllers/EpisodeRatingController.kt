package hu.bme.aut.controllers

import hu.bme.aut.dtos.AverageOfRatingsResponse
import hu.bme.aut.models.EpisodeRating
import hu.bme.aut.services.EpisodeRatingService
import io.ktor.application.call
import io.ktor.http.HttpStatusCode
import io.ktor.http.HttpStatusCode.Companion.NotFound
import io.ktor.http.HttpStatusCode.Companion.OK
import io.ktor.request.receive
import io.ktor.response.respond
import io.ktor.routing.*

fun Route.episodeRatings(service: EpisodeRatingService) {
    route("EpisodeRatings") {

        get("/") {
            val userId = call.request.queryParameters["userId"]?.toIntOrNull()
            val seriesId = call.request.queryParameters["seriesId"]
            val seasonId = call.request.queryParameters["seasonId"]?.toIntOrNull()
            val episodeId = call.request.queryParameters["episodeId"]?.toIntOrNull()
            val ratings = service.findByUserIdAndSeriesIdAndSeasonIdAndEpisodeId(userId, seriesId, seasonId, episodeId)
            call.respond(OK, ratings)
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

        get("/Series/{seriesId}/Average") {
            val seriesId = call.parameters["seriesId"]
            checkNotNull(seriesId)
            val averageOfRatings: Float = service.getAverageForSeries(seriesId)
            call.respond(OK, AverageOfRatingsResponse(averageOfRatings))
        }

        get("/Series/{seriesId}/Season/{seasonId}/Average") {
            val seriesId = call.parameters["seriesId"]
            val seasonId = call.parameters["seasonId"]?.toIntOrNull()
            checkNotNull(seriesId)
            checkNotNull(seasonId)
            val averageOfRatings: Float = service.getAverageForSeason(seriesId, seasonId)
            call.respond(OK, AverageOfRatingsResponse(averageOfRatings))
        }

        get("/Series/{seriesId}/Season/{seasonId}/Episode/{episodeId}/Average") {
            val seriesId = call.parameters["seriesId"]
            val seasonId = call.parameters["seasonId"]?.toIntOrNull()
            val episodeId = call.parameters["episodeId"]?.toIntOrNull()
            checkNotNull(seriesId)
            checkNotNull(seasonId)
            checkNotNull(episodeId)
            val averageOfRatings: Float = service.getAverageForEpisode(seriesId, seasonId, episodeId)
            call.respond(OK, AverageOfRatingsResponse(averageOfRatings))
        }

        post("/") {
            val ratingData = call.receive<EpisodeRating>()
            service.insert(ratingData)
            call.respond(HttpStatusCode.Created)
        }

        put("/{id}") {
            val id = call.parameters["id"]?.toIntOrNull()
            checkNotNull(id) { "The id parameter must be an integer" }
            val ratingData = call.receive<EpisodeRating>()
            service.update(id, ratingData)
            call.respond(HttpStatusCode.NoContent)
        }

        delete("/{id}") {
            val id = call.parameters["id"]?.toIntOrNull()
            checkNotNull(id) { "The id parameter must be an integer" }
            service.delete(id)
            call.respond(HttpStatusCode.NoContent)
        }
    }
}