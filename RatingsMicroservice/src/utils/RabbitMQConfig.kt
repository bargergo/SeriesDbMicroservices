package utils

import hu.bme.aut.ratings.services.MessageQueueConfig
import hu.bme.aut.ratings.services.RabbitService
import io.ktor.application.*
import io.ktor.util.*

class RabbitMQConfig(configuration: Configuration) {
    private val hostName: String = configuration.hostName
    private val userName: String = configuration.userName
    private val password: String = configuration.password
    private val queues: List<QueueConfig> = configuration.queues

    class Configuration {
        lateinit var hostName: String
        lateinit var userName: String
        lateinit var password: String
        var queues: List<QueueConfig> = emptyList()
    }

    fun configureRabbitMQ() {
        RabbitService.configure(
            MessageQueueConfig(
                hostName,
                userName,
                password
            )
        ).tryToConnect()

        for (queueConfig in queues) {
            RabbitService
                .createExchangeAndQueue(queueConfig.queueName, queueConfig.exchangeName)
        }

        RabbitService.startListening()
    }

    data class QueueConfig(val queueName: String, val exchangeName: String)

    companion object Feature :
        ApplicationFeature<ApplicationCallPipeline, Configuration, RabbitMQConfig> {

        override val key = AttributeKey<RabbitMQConfig>("RabbitMQConfig")

        override fun install(pipeline: ApplicationCallPipeline, configure: Configuration.() -> Unit): RabbitMQConfig {

            val configuration = Configuration().apply(configure)

            checkNotNull(configuration.hostName)
            checkNotNull(configuration.userName)
            checkNotNull(configuration.password)

            val feature = RabbitMQConfig(configuration)
            feature.configureRabbitMQ()

            return feature
        }
    }
}