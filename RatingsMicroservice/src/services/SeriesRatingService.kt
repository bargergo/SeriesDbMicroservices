package hu.bme.aut.ratings.services

import hu.bme.aut.ratings.database.DatabaseConnection.dbQuery
import hu.bme.aut.ratings.dtos.AverageOfRatingsResponse
import hu.bme.aut.ratings.dtos.SeriesRatingData
import hu.bme.aut.ratings.model.SeriesRating
import hu.bme.aut.ratings.model.SeriesRatings
import hu.bme.aut.ratings.model.toSeriesRating
import org.jetbrains.exposed.sql.*

class SeriesRatingService {

    suspend fun findByUserIdAndSeriesId(userId: Int?, seriesId: String?): List<SeriesRating> = dbQuery {
        var query = SeriesRatings.selectAll()
        if (userId != null)
            query = query.andWhere {SeriesRatings.userId eq userId }
        if (seriesId != null)
            query = query.andWhere { SeriesRatings.seriesId eq seriesId }
        query.map { it.toSeriesRating() }
    }

    suspend fun findById(id: Int): SeriesRating? = dbQuery {
        SeriesRatings.select { SeriesRatings.id eq id }
            .map { it.toSeriesRating() }
            .firstOrNull()
    }

    suspend fun getAverage(seriesId: String): AverageOfRatingsResponse = dbQuery {
        val avgColumn = SeriesRatings.rating.avg()
        val countColumn = SeriesRatings.id.count()
        val result = SeriesRatings.slice(
            avgColumn,
            countColumn
        ).select { SeriesRatings.seriesId eq seriesId }.first()
        AverageOfRatingsResponse(
            result[avgColumn]?.toFloat() ?: 0f,
            result[countColumn]
        )
    }



    suspend fun insert(data: SeriesRatingData, userId: Int) = dbQuery {
        SeriesRatings.insert {
            it[SeriesRatings.userId] = userId
            it[seriesId] = data.seriesId
            it[rating] = data.rating
            it[opinion] = data.opinion
        }
    }

    suspend fun update(id: Int, data: SeriesRatingData) = dbQuery {
        SeriesRatings.update({ SeriesRatings.id eq id }) {
            it[seriesId] = data.seriesId
            it[rating] = data.rating
            it[opinion] = data.opinion
        }
    }

    suspend fun delete(id: Int) = dbQuery {
        SeriesRatings.deleteWhere { SeriesRatings.id eq id }
    }


}