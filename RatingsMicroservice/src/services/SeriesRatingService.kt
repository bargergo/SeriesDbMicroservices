package hu.bme.aut.services

import hu.bme.aut.database.DatabaseFactory.dbQuery
import hu.bme.aut.model.SeriesRating
import hu.bme.aut.model.SeriesRatings
import org.jetbrains.exposed.sql.ResultRow
import org.jetbrains.exposed.sql.selectAll

class SeriesRatingService {

    suspend fun findAll() = dbQuery {
        SeriesRatings.selectAll().map { toSeriesRating(it) }
    }

    private fun toSeriesRating(it: ResultRow) = SeriesRating(
        id = it[SeriesRatings.id].value,
        userId = it[SeriesRatings.userId],
        seriesId = it[SeriesRatings.seriesId],
        rating = it[SeriesRatings.rating]
    )
}