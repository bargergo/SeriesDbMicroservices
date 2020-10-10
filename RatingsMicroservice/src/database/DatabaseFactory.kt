package hu.bme.aut.ratings.database

import com.zaxxer.hikari.HikariDataSource
import hu.bme.aut.ratings.model.SeriesRatings
import hu.bme.aut.ratings.models.EpisodeRatings
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import org.jetbrains.exposed.sql.Database
import org.jetbrains.exposed.sql.SchemaUtils
import org.jetbrains.exposed.sql.transactions.transaction

object DatabaseFactory {

    fun init(hikariDataSource: HikariDataSource) {
        Database.connect(hikariDataSource)
        transaction {
            SchemaUtils.createMissingTablesAndColumns(
                SeriesRatings,
                EpisodeRatings
            )
        }
    }

    suspend fun <T> dbQuery(block: () -> T): T =
        withContext(Dispatchers.IO) {
            transaction { block() }
        }
}