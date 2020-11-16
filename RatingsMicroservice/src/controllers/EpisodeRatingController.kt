package hu.bme.aut.ratings.controllers

import com.papsign.ktor.openapigen.route.info
import com.papsign.ktor.openapigen.route.path.normal.*
import com.papsign.ktor.openapigen.route.response.respond
import com.papsign.ktor.openapigen.route.route
import hu.bme.aut.ratings.dtos.*
import hu.bme.aut.ratings.models.EpisodeRating
import hu.bme.aut.ratings.models.NotAuthorizedException
import hu.bme.aut.ratings.services.EpisodeRatingService
import hu.bme.aut.ratings.utils.extractToken
import hu.bme.aut.ratings.utils.id
import io.ktor.features.*

fun NormalOpenAPIRoute.episodeRatings(service: EpisodeRatingService) {
    route("/api/public/EpisodeRatings") {

        get<GetEpisodeRatingsQueryParams, List<EpisodeRatingInfo>>(
                info("Get Episodes Ratings Endpoint", "This is a Get Episodes Ratings Endpoint"),
                id("GetEpisodeRatings"),
                example = listOf(
                        EpisodeRatingInfo(1, 2, "5e9215f27773ca0066637c26", 1, 1, 5, "Not good, not terrible")
                )
        ) { params ->
            val userId = params.userId
            val seriesId = params.seriesId
            val seasonId = params.seasonId
            val episodeId = params.episodeId
            val ratings: List<EpisodeRatingInfo> =
                    service.findByUserIdAndSeriesIdAndSeasonIdAndEpisodeId(userId, seriesId, seasonId, episodeId)
                            .map { it.toEpisodeRatingInfo() }
            respond(ratings)
        }

        // {id}
        get<EpisodeRatingIdParam, EpisodeRatingInfo>(
                id("GetEpisodeRating")
        ) { params ->
            val id = params.id
            checkNotNull(id) { "The id parameter must be an integer" }
            val rating: EpisodeRatingInfo? = service.findById(id)?.toEpisodeRatingInfo()
            if (rating == null)
                throw NotFoundException()
            else
                respond(rating)
        }

        // Series/{seriesId}/Average
        get<GetAverageRatingForSeriesParams, AverageOfRatingsResponse>(
                id("GetAverageRatingForSeries")
        ) { params ->
            val seriesId = params.seriesId
            val result: AverageOfRatingsResponse = service.getAverageForSeries(seriesId)
            respond(result)
        }

        // Series/{seriesId}/Season/{seasonId}/Average
        get<GetAverageRatingForSeasonParams, AverageOfRatingsResponse>(
                id("GetAverageRatingForSeason")
        ) { params ->
            val seriesId = params.seriesId
            val seasonId = params.seasonId
            val result: AverageOfRatingsResponse = service.getAverageForSeason(seriesId, seasonId)
            respond(result)
        }

        // Series/{seriesId}/Season/{seasonId}/Episode/{episodeId}/Average
        get<GetAverageRatingForEpisodeParams, AverageOfRatingsResponse>(
                id("GetAverageRatingForEpisode")
        ) { params ->
            val seriesId = params.seriesId
            val seasonId = params.seasonId
            val episodeId = params.episodeId
            val result: AverageOfRatingsResponse = service.getAverageForEpisode(seriesId, seasonId, episodeId)
            respond(result)
        }
    }

    route("/api/protected/EpisodeRatings") {
        post<HeaderParam, Created201Response, EpisodeRatingData>(
                id("CreateEpisodeRating")
        ) { params, ratingData ->
            if (params.Authorization != null) {
                val claims = extractToken(params.Authorization)
                val userId = claims.get("userid")
                val role = claims.get("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                if (userId != ratingData.userId.toString() && role?.contains("ADMINISTRATOR") == false)
                    throw NotAuthorizedException("Not authorized")
            }
            else {
                throw NotAuthorizedException("Not authorized")
            }
            service.insert(ratingData)
            respond(Created201Response())
        }

        // {id}
        put<EpisodeRatingIdParam, NoContent204Response, EpisodeRatingData>(
                id("UpdateEpisodeRating")
        ) { params, ratingData ->
            val id = params.id
            val rating: EpisodeRatingInfo? = service.findById(id)?.toEpisodeRatingInfo()
            if (rating == null)
                throw NotFoundException()
            if (params.Authorization != null) {
                val claims = extractToken(params.Authorization)
                val userId = claims.get("userid")
                val role = claims.get("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                if (userId != rating.userId.toString() && role?.contains("ADMINISTRATOR") == false)
                    throw NotAuthorizedException("Not authorized")
            }
            else {
                throw NotAuthorizedException("Not authorized")
            }
            service.update(id, ratingData)
            respond(NoContent204Response())
        }

        // {id}
        delete<EpisodeRatingIdParam, NoContent204Response>(
                id("DeleteEpisodeRating")
        ) { params ->
            val id = params.id
            val rating: EpisodeRatingInfo? = service.findById(id)?.toEpisodeRatingInfo()
            if (params.Authorization != null) {
                val claims = extractToken(params.Authorization)
                val userId = claims.get("userid")
                val role = claims.get("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                if (userId != rating?.userId.toString() && role?.contains("ADMINISTRATOR") == false)
                    throw NotAuthorizedException("Not authorized")
            }
            else {
                throw NotAuthorizedException("Not authorized")
            }
            service.delete(id)
            respond(NoContent204Response())
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