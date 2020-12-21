package hu.bme.aut.ratings.services

import com.rabbitmq.client.*
import hu.bme.aut.ratings.utils.getenvCheckNotNull
import java.net.ConnectException


object RabbitService {
    var connectionFactory = ConnectionFactory()
    lateinit var connection: Connection

    fun configure(): RabbitService {
        connectionFactory.apply {
            host = getenvCheckNotNull("MessageQueueSettings__Hostname")
            port = 5672
            virtualHost = "/"
            username = getenvCheckNotNull("MessageQueueSettings__Username")
            password = getenvCheckNotNull("MessageQueueSettings__Password")
        }
        return this
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

    fun dummyExchangeAndQueue(): RabbitService {
        val channel = connection.createChannel()

        channel.exchangeDeclare("SeriesAndEpisodes.MessageQueue:IDummyMessage", "fanout", true)
        val queueName = channel.queueDeclare("DummyQueue", true, false, false, emptyMap()).queue
        channel.queueBind(queueName, "SeriesAndEpisodes.MessageQueue:IDummyMessage", "routingKey")

        channel.close()
        return this
    }

    fun updateSeriesRatingExchangeAndQueue(): RabbitService {
        val channel = connection.createChannel()

        channel.exchangeDeclare("SeriesAndEpisodes.MessageQueue:ISeriesRatingChangedEvent", "fanout", true)
        val queueName = channel.queueDeclare("SeriesRatingUpdateQueue", true, false, false, emptyMap()).queue
        channel.queueBind(queueName, "SeriesAndEpisodes.MessageQueue:ISeriesRatingChangedEvent", "routingKey")

        channel.close()
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