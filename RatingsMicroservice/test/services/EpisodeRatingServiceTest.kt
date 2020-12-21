package hu.bme.aut.ratings.services

import com.zaxxer.hikari.HikariConfig
import com.zaxxer.hikari.HikariDataSource
import hu.bme.aut.ratings.database.DatabaseConnection
import hu.bme.aut.ratings.dtos.EpisodeRatingData
import hu.bme.aut.ratings.models.EpisodeRating
import hu.bme.aut.ratings.models.EpisodeRatings
import hu.bme.aut.ratings.models.toEpisodeRating
import hu.bme.aut.ratings.utils.getenvOrDefault
import kotlinx.coroutines.runBlocking
import org.jetbrains.exposed.sql.deleteAll
import org.jetbrains.exposed.sql.insert
import org.jetbrains.exposed.sql.select
import org.jetbrains.exposed.sql.selectAll
import org.jetbrains.exposed.sql.transactions.transaction
import org.junit.BeforeClass
import org.junit.jupiter.api.Assertions
import kotlin.test.BeforeTest
import kotlin.test.Test

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
            DatabaseConnection.connect(hikari())
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
    fun insert_IntoNotEmptyDatabase_Succeeds() {
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
                    seriesId = "asd",
                    seasonId = 2,
                    episodeId = 3,
                    rating = 2,
                    opinion = "asd"
            ), 12)
        }

        // Assert
        val size = transaction {
            val result = EpisodeRatings.selectAll()
            result.count()
        }
        Assertions.assertEquals(2, size)
    }

    @Test
    fun delete_ExistingEntity_Succeeds() {
        // Arrange
        val idToDelete: Int = transaction {
            EpisodeRatings.insert {
                it[userId] = 1
                it[seriesId] = "data.seriesId"
                it[seasonId] = 2
                it[episodeId] = 3
                it[rating] = 5
                it[opinion] = "data.opinion"
            }
            EpisodeRatings.insert {
                it[userId] = 2
                it[seriesId] = "data.seriesId"
                it[seasonId] = 2
                it[episodeId] = 3
                it[rating] = 5
                it[opinion] = "data.opinion"
            }
            EpisodeRatings.select { EpisodeRatings.userId eq 2 }.first()[EpisodeRatings.id].value
        }
        // Act
        runBlocking {
            episodeRatingService.delete(idToDelete)
        }

        // Assert
        val size = transaction {
            val result = EpisodeRatings.selectAll()
            result.count()
        }
        Assertions.assertEquals(1, size)
    }

    @Test
    fun update_ExistingEntity_Succeeds() {
        // Arrange
        val idToUpdate: Int = transaction {
            EpisodeRatings.insert {
                it[userId] = 1
                it[seriesId] = "data.seriesId"
                it[seasonId] = 2
                it[episodeId] = 3
                it[rating] = 5
                it[opinion] = "data.opinion"
            }
            EpisodeRatings.insert {
                it[userId] = 2
                it[seriesId] = "data.seriesId"
                it[seasonId] = 2
                it[episodeId] = 3
                it[rating] = 5
                it[opinion] = "data.opinion"
            }
            EpisodeRatings.select { EpisodeRatings.userId eq 2 }.first()[EpisodeRatings.id].value
        }
        // Act
        runBlocking {
            episodeRatingService.update(idToUpdate,
                    EpisodeRatingData(
                            "data.seriesId",
                            2,
                            3,
                            10,
                            "It's a good show"
                    )
            )
        }

        // Assert
        val updatedEpisodeRating: EpisodeRating = transaction {
            EpisodeRatings
                    .select {EpisodeRatings.id eq idToUpdate}
                    .single().toEpisodeRating()
        }
        Assertions.assertEquals(10, updatedEpisodeRating.rating)
        Assertions.assertEquals("It's a good show", updatedEpisodeRating.opinion)

        Assertions.assertEquals("data.seriesId", updatedEpisodeRating.seriesId)
        Assertions.assertEquals(2, updatedEpisodeRating.seasonId)
        Assertions.assertEquals(3, updatedEpisodeRating.episodeId)
    }
}
