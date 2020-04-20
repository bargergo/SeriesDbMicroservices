package hu.bme.aut.ratings.database

import com.zaxxer.hikari.HikariConfig
import com.zaxxer.hikari.HikariDataSource
import hu.bme.aut.ratings.model.SeriesRatings
import hu.bme.aut.ratings.models.EpisodeRatings
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import org.jetbrains.exposed.sql.Database
import org.jetbrains.exposed.sql.SchemaUtils
import org.jetbrains.exposed.sql.insert
import org.jetbrains.exposed.sql.selectAll
import org.jetbrains.exposed.sql.transactions.transaction

object DatabaseFactory {

    fun init() {
        Database.connect(hikari())
        transaction {
            SchemaUtils.createMissingTablesAndColumns(
                SeriesRatings,
                EpisodeRatings
            )
        }
    }

    private fun hikari(): HikariDataSource {
        val config = HikariConfig("/hikari.properties")
        config.jdbcUrl = getenvCheckNotNull("db__jdbcUrl")
        config.username = getenvCheckNotNull("db__username")
        config.password = getenvCheckNotNull("db__password")
        config.validate()
        return HikariDataSource(config)
    }

    suspend fun <T> dbQuery(block: () -> T): T =
        withContext(Dispatchers.IO) {
            transaction { block() }
        }
}

fun getenvCheckNotNull(param: String): String {
    return checkNotNull(System.getenv(param)) { "Environment variable '${param}' must be set." }
}