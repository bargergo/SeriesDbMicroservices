package hu.bme.aut.ratings.controllers

import com.papsign.ktor.openapigen.route.info
import com.papsign.ktor.openapigen.route.path.normal.*
import com.papsign.ktor.openapigen.route.response.respond
import com.papsign.ktor.openapigen.route.route
import hu.bme.aut.ratings.dtos.*
import hu.bme.aut.ratings.model.SeriesRating
import hu.bme.aut.ratings.services.SeriesRatingService
import io.ktor.features.NotFoundException

fun NormalOpenAPIRoute.seriesRatings(service: SeriesRatingService) {
    route("/api/SeriesRatings") {

        get<GetSeriesRatingsQueryParams, List<SeriesRatingInfo>>(
            info("Get Episodes Ratings Endpoint", "This is a Get Episodes Ratings Endpoint"),
            example = listOf(
                SeriesRatingInfo(1, 2, "5e9215f27773ca0066637c26", 1, "Not good, not terrible")
            )
        ) { params ->
            val userId = params.userId
            val seriesId = params.seriesId
            val ratings: List<SeriesRatingInfo> = service.findByUserIdAndSeriesId(userId, seriesId)
                .map { it.toSeriesRatingInfo() }
            respond(ratings)
        }

        // {id}
        get<SeriesRatingIdParam, SeriesRatingInfo> { params ->
            val id = params.id
            val rating: SeriesRatingInfo? = service.findById(id)?.toSeriesRatingInfo()
            if (rating == null)
                throw NotFoundException()
            else
                respond(rating)
        }

        // Series/{seriesId}/Average
        get<GetAverageRatingForSeriesParams, AverageOfRatingsResponse> { params ->
            val seriesId = params.seriesId
            val result: AverageOfRatingsResponse = service.getAverage(seriesId)
            respond(result)
        }

        post<Unit, Created201Response, SeriesRatingData> { _, seriesData ->
            service.insert(seriesData)
            respond(Created201Response())
        }

        // {id}
        put<SeriesRatingIdParam, NoContent204Response, SeriesRatingData> { params, seriesData ->
            val id = params.id
            service.update(id, seriesData)
            respond(NoContent204Response())
        }

        // {id}
        delete<SeriesRatingIdParam, NoContent204Response> { params ->
            val id = params.id
            service.delete(id)
            respond(NoContent204Response())
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