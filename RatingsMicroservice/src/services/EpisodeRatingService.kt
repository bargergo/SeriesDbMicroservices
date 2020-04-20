package hu.bme.aut.ratings.services

import hu.bme.aut.ratings.database.DatabaseFactory.dbQuery
import hu.bme.aut.ratings.dtos.AverageOfRatingsResponse
import hu.bme.aut.ratings.models.EpisodeRating
import hu.bme.aut.ratings.models.EpisodeRatings
import hu.bme.aut.ratings.models.toEpisodeRating
import org.jetbrains.exposed.sql.*

class EpisodeRatingService {

    suspend fun findByUserIdAndSeriesIdAndSeasonIdAndEpisodeId(userId: Int?, seriesId: String?, seasonId: Int?, episodeId: Int?) = dbQuery {
        var query = EpisodeRatings.selectAll()
        if (userId != null)
            query = query.andWhere { EpisodeRatings.userId eq userId }
        if (seriesId != null) {
            query = query.andWhere { EpisodeRatings.seriesId eq seriesId }
            if (seasonId != null) {
                query = query.andWhere { EpisodeRatings.seasonId eq seasonId }
                if (episodeId != null) {
                    query = query.andWhere { EpisodeRatings.episodeId eq episodeId }
                }
            }
        }
        query.map { it.toEpisodeRating() }
    }

    suspend fun findById(id: Int) = dbQuery {
        EpisodeRatings.select { EpisodeRatings.id eq id }
            .map { it.toEpisodeRating() }
            .firstOrNull()
    }

    suspend fun getAverageForSeries(seriesId: String): AverageOfRatingsResponse = dbQuery {
        val avgColumn = EpisodeRatings.rating.avg()
        val countColumn = EpisodeRatings.id.count()
        val result = EpisodeRatings.slice(
            avgColumn,
            countColumn
        ).select { EpisodeRatings.seriesId eq seriesId }.first()
        AverageOfRatingsResponse(
            result[avgColumn]?.toFloat() ?: 0f,
            result[countColumn]
        )
    }

    suspend fun getAverageForSeason(seriesId: String, seasonId: Int): AverageOfRatingsResponse = dbQuery {
        val avgColumn = EpisodeRatings.rating.avg()
        val countColumn = EpisodeRatings.id.count()
        val result = EpisodeRatings.slice(
            avgColumn,
            countColumn
        ).select { EpisodeRatings.seriesId eq seriesId }
            .andWhere { EpisodeRatings.seasonId eq seasonId }.first()
        AverageOfRatingsResponse(
            result[avgColumn]?.toFloat() ?: 0f,
            result[countColumn]
        )
    }

    suspend fun getAverageForEpisode(seriesId: String, seasonId: Int, episodeId: Int): AverageOfRatingsResponse = dbQuery {
        val avgColumn = EpisodeRatings.rating.avg()
        val countColumn = EpisodeRatings.id.count()
        val result = EpisodeRatings.slice(
            avgColumn,
            countColumn
        ).select { EpisodeRatings.seriesId eq seriesId }
            .andWhere { EpisodeRatings.seasonId eq seasonId }
            .andWhere { EpisodeRatings.episodeId eq episodeId }.first()
        AverageOfRatingsResponse(
            result[avgColumn]?.toFloat() ?: 0f,
            result[countColumn]
        )
    }

    suspend fun insert(data: EpisodeRating) = dbQuery {
        EpisodeRatings.insert {
            it[userId] = data.userId
            it[seriesId] = data.seriesId
            it[rating] = data.rating
            it[opinion] = data.opinion
        }
    }

    suspend fun update(id: Int, data: EpisodeRating) = dbQuery {
        EpisodeRatings.update({ EpisodeRatings.id eq id }) {
            it[userId] = data.userId
            it[seriesId] = data.seriesId
            it[rating] = data.rating
            it[opinion] = data.opinion
        }
    }

    suspend fun delete(id: Int) = dbQuery {
        EpisodeRatings.deleteWhere { EpisodeRatings.id eq id }
    }
}