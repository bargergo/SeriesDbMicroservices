package hu.bme.aut.models

import org.jetbrains.exposed.dao.id.IntIdTable
import org.jetbrains.exposed.sql.ResultRow

object EpisodeRatings : IntIdTable("EpisodeRatings") {
    val userId = EpisodeRatings.integer("User_Id")
    val seriesId = EpisodeRatings.varchar("Series_Id", 24)
    val seasonId = EpisodeRatings.integer("Season_Id")
    val episodeId = EpisodeRatings.integer("Episode_Id")
    val rating = EpisodeRatings.integer("Rating")
    val opinion = EpisodeRatings.varchar("Opinion", 255)
}

data class EpisodeRating(
    val id: Int,
    val userId: Int,
    val seriesId: String,
    val seasonId: Int,
    val episodeId: Int,
    val rating: Int,
    val opinion: String
)

fun ResultRow.toEpisodeRating() = EpisodeRating(
    id = this[EpisodeRatings.id].value,
    userId = this[EpisodeRatings.userId],
    seriesId = this[EpisodeRatings.seriesId],
    seasonId = this[EpisodeRatings.seasonId],
    episodeId = this[EpisodeRatings.episodeId],
    rating = this[EpisodeRatings.rating],
    opinion = this[EpisodeRatings.opinion]
)