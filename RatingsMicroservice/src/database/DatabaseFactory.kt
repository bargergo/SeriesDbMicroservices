package hu.bme.aut.database

import com.zaxxer.hikari.HikariConfig
import com.zaxxer.hikari.HikariDataSource
import hu.bme.aut.model.SeriesRatings
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import org.jetbrains.exposed.sql.Database
import org.jetbrains.exposed.sql.SchemaUtils
import org.jetbrains.exposed.sql.insert
import org.jetbrains.exposed.sql.selectAll
import org.jetbrains.exposed.sql.transactions.DEFAULT_ISOLATION_LEVEL
import org.jetbrains.exposed.sql.transactions.DEFAULT_REPETITION_ATTEMPTS
import org.jetbrains.exposed.sql.transactions.ThreadLocalTransactionManager
import org.jetbrains.exposed.sql.transactions.transaction

object DatabaseFactory {

    fun init() {
        Database.connect(hikari())
        transaction {
            SchemaUtils.createMissingTablesAndColumns(
                SeriesRatings
            )
            if (SeriesRatings.selectAll().empty()) {
                SeriesRatings.insert {
                    it[userId] = 1
                    it[seriesId] = "5e8b3f30e605c002df7c498a"
                    it[rating] = 5
                }
            }
        }
    }

    private fun hikari(): HikariDataSource {
        val config = HikariConfig("/hikari.properties")
        config.validate()
        return HikariDataSource(config)
    }

    suspend fun <T> dbQuery(block: () -> T): T =
        withContext(Dispatchers.IO) {
            transaction { block() }
        }
}