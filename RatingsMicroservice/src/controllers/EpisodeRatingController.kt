package hu.bme.aut.ratings.controllers

import hu.bme.aut.ratings.dtos.AverageOfRatingsResponse
import hu.bme.aut.ratings.dtos.EpisodeRatingData
import hu.bme.aut.ratings.dtos.EpisodeRatingInfo
import hu.bme.aut.ratings.models.EpisodeRating
import hu.bme.aut.ratings.services.EpisodeRatingService
import io.ktor.application.call
import io.ktor.http.HttpStatusCode
import io.ktor.http.HttpStatusCode.Companion.NotFound
import io.ktor.http.HttpStatusCode.Companion.OK
import io.ktor.request.receive
import io.ktor.response.respond
import io.ktor.routing.*

fun Route.episodeRatings(service: EpisodeRatingService) {
    route("/api/EpisodeRatings") {

        get("/") {
            val userId = call.request.queryParameters["userId"]?.toIntOrNull()
            val seriesId = call.request.queryParameters["seriesId"]
            val seasonId = call.request.queryParameters["seasonId"]?.toIntOrNull()
            val episodeId = call.request.queryParameters["episodeId"]?.toIntOrNull()
            val ratings: List<EpisodeRatingInfo> = service.findByUserIdAndSeriesIdAndSeasonIdAndEpisodeId(userId, seriesId, seasonId, episodeId)
                .map { it.toEpisodeRatingInfo() }
            call.respond(OK, ratings)
        }

        get("/{id}") {
            val id = call.parameters["id"]?.toIntOrNull()
            checkNotNull(id) { "The id parameter must be an integer" }
            val rating: EpisodeRatingInfo? = service.findById(id)?.toEpisodeRatingInfo()
            if (rating == null)
                call.respond(NotFound)
            else
                call.respond(OK, rating)
        }

        get("/Series/{seriesId}/Average") {
            val seriesId = call.parameters["seriesId"]
            checkNotNull(seriesId)
            val result: AverageOfRatingsResponse = service.getAverageForSeries(seriesId)
            call.respond(OK, result)
        }

        get("/Series/{seriesId}/Season/{seasonId}/Average") {
            val seriesId = call.parameters["seriesId"]
            val seasonId = call.parameters["seasonId"]?.toIntOrNull()
            checkNotNull(seriesId)
            checkNotNull(seasonId)
            val result: AverageOfRatingsResponse = service.getAverageForSeason(seriesId, seasonId)
            call.respond(OK, result)
        }

        get("/Series/{seriesId}/Season/{seasonId}/Episode/{episodeId}/Average") {
            val seriesId = call.parameters["seriesId"]
            val seasonId = call.parameters["seasonId"]?.toIntOrNull()
            val episodeId = call.parameters["episodeId"]?.toIntOrNull()
            checkNotNull(seriesId)
            checkNotNull(seasonId)
            checkNotNull(episodeId)
            val result: AverageOfRatingsResponse = service.getAverageForEpisode(seriesId, seasonId, episodeId)
            call.respond(OK, result)
        }

        post("/") {
            val ratingData = call.receive<EpisodeRatingData>()
            service.insert(ratingData)
            call.respond(HttpStatusCode.Created)
        }

        put("/{id}") {
            val id = call.parameters["id"]?.toIntOrNull()
            checkNotNull(id) { "The id parameter must be an integer" }
            val ratingData = call.receive<EpisodeRatingData>()
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

fun EpisodeRating.toEpisodeRatingInfo() = EpisodeRatingInfo(
    this.id,
    this.userId,
    this.seriesId,
    this.seasonId,
    this.episodeId,
    this.rating,
    this.opinion
)