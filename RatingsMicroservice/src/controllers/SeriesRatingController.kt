package hu.bme.aut.ratings.controllers

import com.fasterxml.jackson.databind.ObjectMapper
import com.papsign.ktor.openapigen.route.info
import com.papsign.ktor.openapigen.route.path.normal.*
import com.papsign.ktor.openapigen.route.response.respond
import com.papsign.ktor.openapigen.route.route
import hu.bme.aut.ratings.dtos.*
import hu.bme.aut.ratings.model.SeriesRating
import hu.bme.aut.ratings.models.NotAuthorizedException
import hu.bme.aut.ratings.services.RabbitService
import hu.bme.aut.ratings.services.SeriesRatingService
import hu.bme.aut.ratings.utils.extractToken
import hu.bme.aut.ratings.utils.id
import io.ktor.features.NotFoundException
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

fun NormalOpenAPIRoute.seriesRatings(service: SeriesRatingService) {
    route("/api/public/SeriesRatings") {

        get<GetSeriesRatingsQueryParams, List<SeriesRatingInfo>>(
                info("Get Episodes Ratings Endpoint", "This is a Get Episodes Ratings Endpoint"),
                id("GetSeriesRatings"),
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
        get<SeriesRatingIdParam, SeriesRatingInfo>(
                id("GetSeriesRating")
        ) { params ->
            val id = params.id
            val rating: SeriesRatingInfo? = service.findById(id)?.toSeriesRatingInfo()
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
            val result: AverageOfRatingsResponse = service.getAverage(seriesId)
            respond(result)

        }
    }

    route("/api/protected/SeriesRatings") {

        post<HeaderParam, Created201Response, SeriesRatingData>(
            id("CreateSeriesRating")
        ) { param, seriesData ->
            if (param.Authorization != null) {
                val claims = extractToken(param.Authorization)
                val userId = claims.get("userid")
                if (userId == null)
                    throw NotAuthorizedException("Not authorized")
                service.insert(seriesData, Integer.parseInt(userId))
                val updatedAverage = service.getAverage(seriesData.seriesId)
                publishSeriesRatingChangedEvent(
                        SeriesRatingChangedEvent(
                                seriesData.seriesId,
                                updatedAverage.count,
                                updatedAverage.average
                        )
                )
                respond(Created201Response())
            } else {
                throw NotAuthorizedException("Not authorized")
            }
        }

        // {id}
        put<SeriesRatingIdParam, NoContent204Response, SeriesRatingData>(
            id("UpdateSeriesRating")
        ) { params, seriesData ->
            val id = params.id
            val rating: SeriesRatingInfo? = service.findById(id)?.toSeriesRatingInfo()
            if (rating == null)
                throw NotFoundException()
            if (params.Authorization != null) {
                val claims = extractToken(params.Authorization)
                val userId = claims.get("userid")
                val role = claims.get("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                if (userId != rating.userId.toString() && role?.contains("ADMINISTRATOR") != true)
                    throw NotAuthorizedException("Not authorized")
            }
            else {
                throw NotAuthorizedException("Not authorized")
            }
            service.update(id, seriesData)
            val updatedAverage = service.getAverage(seriesData.seriesId)
            publishSeriesRatingChangedEvent(
                SeriesRatingChangedEvent(
                    seriesData.seriesId,
                    updatedAverage.count,
                    updatedAverage.average
                )
            )
            respond(NoContent204Response())
        }

        // {id}
        delete<SeriesRatingIdParam, NoContent204Response>(
            id("DeleteSeriesRating")
        ) { params ->
            val id = params.id
            val rating: SeriesRatingInfo? = service.findById(id)?.toSeriesRatingInfo()
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
            if (rating != null) {
                val updatedAverage = service.getAverage(rating.seriesId)
                publishSeriesRatingChangedEvent(
                    SeriesRatingChangedEvent(
                        rating.seriesId,
                        updatedAverage.count,
                        updatedAverage.average
                    )
                )
            }
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


data class MassTransitCompatibleMessage(
    val destinationAddress: String,
    val headers: Any,
    val message: Any,
    val messageType: List<String>
)

suspend fun publishSeriesRatingChangedEvent(event: SeriesRatingChangedEvent) {
    val mapper = ObjectMapper()
    val message = MassTransitCompatibleMessage(
        "rabbitmq://message-queue/SeriesRatingUpdateQueue",
        Unit,
        event,
        arrayListOf("urn:message:SeriesAndEpisodes.MessageQueue:SeriesRatingChangedEvent",
            "urn:message:SeriesAndEpisodes.MessageQueue:ISeriesRatingChangedEvent")
    )
    withContext(Dispatchers.IO) {
        val channel = RabbitService.connection.createChannel()
        channel.basicPublish("SeriesRatingUpdateQueue", "routingKey", null, mapper.writeValueAsBytes(message))
        channel.close()
    }
}