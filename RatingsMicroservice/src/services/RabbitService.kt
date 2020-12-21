package hu.bme.aut.ratings.services

import com.rabbitmq.client.*
import hu.bme.aut.ratings.utils.getenvCheckNotNull
import java.net.ConnectException


object RabbitService {
    var connectionFactory = ConnectionFactory()
    lateinit var connection: Connection
    lateinit var host: String

    fun configure(config: MessageQueueConfig): RabbitService {
        host = config.hostName
        connectionFactory.apply {
            host = config.hostName
            port = 5672
            virtualHost = "/"
            username = config.userName
            password = config.password
        }
        return this
    }

    fun createExchangeAndQueue(queueName: String, exchangeName: String) {
        val channel = connection.createChannel()

        channel.exchangeDeclare(exchangeName, "fanout", true)
        val queueNameToUse = channel.queueDeclare(queueName, true, false, false, emptyMap()).queue
        channel.queueBind(queueNameToUse, exchangeName, "routingKey")

        channel.close()
    }

    fun tryToConnect(): RabbitService {
        while (true) {
            try {
                connection = connectionFactory.newConnection()
                break
            } catch (e: ConnectException) {
                println(e.localizedMessage)
                println("Waiting for message queue...")
                Thread.sleep(2000)
            }
        }
        return this
    }

    fun startListening() {
        val channel = connection.createChannel()
        val deliverCallback = DeliverCallback { consumerTag: String?, message: Delivery? ->
            println("Consuming message: ${String(message!!.body)}")
        }
        val cancelCallback = CancelCallback {consumerTag: String? ->
            println("Cancelled $consumerTag")
        }
        channel.basicConsume("DummyQueue",
        true,
        deliverCallback,
        cancelCallback)
    }
}

data class MessageQueueConfig(
    val hostName: String,
    val userName: String,
    val password: String
)