package hu.bme.aut.ratings.database

import com.zaxxer.hikari.HikariDataSource
import hu.bme.aut.ratings.model.SeriesRatings
import hu.bme.aut.ratings.models.EpisodeRatings
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import org.jetbrains.exposed.sql.Database
import org.jetbrains.exposed.sql.SchemaUtils
import org.jetbrains.exposed.sql.transactions.transaction

object DatabaseConnection {

    fun connect(hikariDataSource: HikariDataSource) {
        Database.connect(hikariDataSource)
                .also {
                    while (true) {
                        try {
                            transaction {
                                SchemaUtils.createMissingTablesAndColumns(
                                        SeriesRatings,
                                        EpisodeRatings
                                )
                            }
                            break
                        } catch (e: Throwable) {
                            println(e.localizedMessage)
                            println("Waiting for db...")

                            Thread.sleep(1000)
                        }
                    }
                }
    }

    suspend fun <T> dbQuery(block: () -> T): T =
        withContext(Dispatchers.IO) {
            transaction { block() }
        }
}