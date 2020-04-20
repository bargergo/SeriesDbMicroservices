package hu.bme.aut.ratings.model

import org.jetbrains.exposed.dao.id.IntIdTable
import org.jetbrains.exposed.sql.ResultRow

object SeriesRatings : IntIdTable("SeriesRatings") {
    val userId = integer("User_Id")
    val seriesId = varchar("Series_Id", 24)
    val rating = integer("Rating")
    val opinion = varchar("Opinion", 255)
}

data class SeriesRating(
    val id: Int,
    val userId: Int,
    val seriesId: String,
    val rating: Int,
    val opinion: String
)

fun ResultRow.toSeriesRating() = SeriesRating(
    id = this[SeriesRatings.id].value,
    userId = this[SeriesRatings.userId],
    seriesId = this[SeriesRatings.seriesId],
    rating = this[SeriesRatings.rating],
    opinion = this[SeriesRatings.opinion]
)