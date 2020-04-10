package hu.bme.aut.model

import org.jetbrains.exposed.dao.id.IntIdTable
import org.jetbrains.exposed.sql.ReferenceOption

object SeriesRatings : IntIdTable("SeriesRatings") {
    val userId = integer("User_Id")
    val seriesId = varchar("Series_Id", 24)
    val rating = integer("Rating")
}

data class SeriesRating(
    val id: Int,
    val userId: Int,
    val seriesId: String,
    val rating: Int
)