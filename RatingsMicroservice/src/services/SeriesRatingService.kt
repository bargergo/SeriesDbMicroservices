package hu.bme.aut.services

import hu.bme.aut.database.DatabaseFactory.dbQuery
import hu.bme.aut.model.SeriesRating
import hu.bme.aut.model.SeriesRatings
import hu.bme.aut.model.toSeriesRating
import org.jetbrains.exposed.sql.*

class SeriesRatingService {

    suspend fun findAll() = dbQuery {
        SeriesRatings.selectAll().map { it.toSeriesRating() }
    }

    suspend fun findById(id: Int) = dbQuery {
        SeriesRatings.select { SeriesRatings.id eq id }
            .map { it.toSeriesRating() }
            .firstOrNull()
    }

    suspend fun findAllBySeriesId(seriesId: String) = dbQuery {
        SeriesRatings.select { SeriesRatings.seriesId eq seriesId }.map { it.toSeriesRating() }
    }

    suspend fun findAllByUserId(userId: Int) = dbQuery {
        SeriesRatings.select { SeriesRatings.userId eq userId }.map { it.toSeriesRating() }
    }

    suspend fun insert(data: SeriesRating) = dbQuery {
        SeriesRatings.insert {
            it[userId] = data.userId
            it[seriesId] = data.seriesId
            it[rating] = data.rating
        }
    }

    suspend fun update(id: Int, data: SeriesRating) = dbQuery {
        SeriesRatings.update({ SeriesRatings.id eq id }) {
            it[userId] = data.userId
            it[seriesId] = data.seriesId
            it[rating] = data.rating
        }
    }

    suspend fun delete(id: Int) = dbQuery {
        SeriesRatings.deleteWhere { SeriesRatings.id eq id }
    }


}