package hu.bme.aut.ratings

import com.zaxxer.hikari.HikariConfig
import com.zaxxer.hikari.HikariDataSource
import hu.bme.aut.ratings.database.DatabaseFactory
import hu.bme.aut.ratings.dtos.EpisodeRatingData
import hu.bme.aut.ratings.models.EpisodeRatings
import hu.bme.aut.ratings.services.EpisodeRatingService
import hu.bme.aut.ratings.utils.getenvOrDefault
import kotlinx.coroutines.runBlocking
import org.jetbrains.exposed.sql.deleteAll
import org.jetbrains.exposed.sql.insert
import org.jetbrains.exposed.sql.transactions.transaction
import org.junit.BeforeClass
import org.junit.jupiter.api.Assertions
import kotlin.test.BeforeTest
import kotlin.test.Test
import kotlin.test.assertEquals

class EpisodeRatingServiceTest {
    val episodeRatingService = EpisodeRatingService()

    companion object {

         fun hikari(): HikariDataSource {
            val config = HikariConfig("/hikari.properties")
            config.jdbcUrl = "jdbc:mysql://${getenvOrDefault("MYSQL_HOST", "localhost")}:${getenvOrDefault("MYSQL_PORT", "3306")}/ratings"
            config.username = getenvOrDefault("MYSQL_USER", "root")
            config.password = getenvOrDefault("MYSQL_PASSWORD", "helloworld")
            config.validate()
            return HikariDataSource(config)
        }

        @BeforeClass
        @JvmStatic
        internal fun beforeAll() {
            DatabaseFactory.init(hikari())
        }


    }

    @BeforeTest
    internal fun beforeEach() {
        transaction {
            EpisodeRatings.deleteAll()
        }
    }

    @Test
    fun databaseIsEmpty() {
        val size = runBlocking {
            val result = episodeRatingService.findByUserIdAndSeriesIdAndSeasonIdAndEpisodeId(null, null, null, null)
            result.size
        }
        Assertions.assertEquals(0, size)
    }

    @Test
    fun insertIntoDatabase() {
        // Arrange
        transaction {
            EpisodeRatings.insert {
                it[userId] = 1
                it[seriesId] = "data.seriesId"
                it[seasonId] = 2
                it[episodeId] = 3
                it[rating] = 5
                it[opinion] = "data.opinion"
            }
        }
        // Act
        runBlocking {
            episodeRatingService.insert(EpisodeRatingData(
                    userId = 12,
                    seriesId = "asd",
                    seasonId = 2,
                    episodeId = 3,
                    rating = 2,
                    opinion = "asd"
            ))
        }

        // Assert
        val size = runBlocking {
            val result = episodeRatingService.findByUserIdAndSeriesIdAndSeasonIdAndEpisodeId(null, null, null, null)
            result.size
        }
        Assertions.assertEquals(2, size)
    }
}
