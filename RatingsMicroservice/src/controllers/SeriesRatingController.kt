package hu.bme.aut.ratings.controllers

import com.fasterxml.jackson.databind.ObjectMapper
import com.papsign.ktor.openapigen.route.info
import com.papsign.ktor.openapigen.route.path.normal.*
import com.papsign.ktor.openapigen.route.response.respond
import com.papsign.ktor.openapigen.route.route
import hu.bme.aut.ratings.dtos.*
import hu.bme.aut.ratings.model.SeriesRating
import hu.bme.aut.ratings.services.RabbitService
import hu.bme.aut.ratings.services.SeriesRatingService
import hu.bme.aut.ratings.utils.id
import io.ktor.features.NotFoundException
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

fun NormalOpenAPIRoute.seriesRatings(service: SeriesRatingService) {
    route("/api/SeriesRatings") {

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

        post<Unit, Created201Response, SeriesRatingData>(
            id("CreateSeriesRating")
        ) { _, seriesData ->
            service.insert(seriesData)
            val updatedAverage = service.getAverage(seriesData.seriesId)
            publishSeriesRatingChangedEvent(
                SeriesRatingChangedEvent(
                    seriesData.seriesId,
                    updatedAverage.count,
                    updatedAverage.average
                )
            )
            respond(Created201Response())


        }

        // {id}
        put<SeriesRatingIdParam, NoContent204Response, SeriesRatingData>(
            id("UpdateSeriesRating")
        ) { params, seriesData ->
            val id = params.id
            val rating: SeriesRatingInfo? = service.findById(id)?.toSeriesRatingInfo()
            if (rating == null)
                throw NotFoundException()
            service.update(id, seriesData)
            respond(NoContent204Response())
        }

        // {id}
        delete<SeriesRatingIdParam, NoContent204Response>(
            id("DeleteSeriesRating")
        ) { params ->
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
        val connection = RabbitService().connectionFactory.newConnection()
        val channel = connection.createChannel()
        channel.basicPublish("SeriesRatingUpdateQueue", "routingKey", null, mapper.writeValueAsBytes(message))
        channel.close()
        connection.close()
    }
}