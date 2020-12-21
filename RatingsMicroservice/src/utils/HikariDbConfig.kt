package utils

import com.zaxxer.hikari.HikariDataSource
import hu.bme.aut.ratings.database.DatabaseConnection
import io.ktor.application.*
import io.ktor.util.*

class HikariDbConfig(configuration: Configuration) {
    private val hikariDataSource = configuration.hikariDataSource

    class Configuration {
        lateinit var hikariDataSource: HikariDataSource
    }

    fun connect() {
        DatabaseConnection.connect(hikariDataSource)
    }

    companion object Feature : ApplicationFeature<ApplicationCallPipeline, Configuration, HikariDbConfig> {

        override val key = AttributeKey<HikariDbConfig>("HikariDbConfig")

        override fun install(pipeline: ApplicationCallPipeline, configure: Configuration.() -> Unit): HikariDbConfig {

            val configuration = Configuration().apply(configure)

            checkNotNull(configuration.hikariDataSource)
            configuration.hikariDataSource.validate()

            val feature = HikariDbConfig(configuration)
            feature.connect()

            return feature
        }
    }
}