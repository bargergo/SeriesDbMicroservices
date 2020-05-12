package hu.bme.aut.ratings.services

import com.rabbitmq.client.CancelCallback
import com.rabbitmq.client.ConnectionFactory
import com.rabbitmq.client.DeliverCallback
import com.rabbitmq.client.Delivery

class RabbitService {
    val connectionFactory: ConnectionFactory
    init {
        connectionFactory = ConnectionFactory().apply {
            host = "message-queue"
            port = 5672
            virtualHost = "/"
            username = "guest"
            password = "guest"
        }
    }

    fun defaultExchangeAndQueue(): RabbitService {
        val connection = connectionFactory.newConnection()
        val channel = connection.createChannel()

        channel.exchangeDeclare("SeriesAndEpisodes.MessageQueue:IDummyMessage", "fanout", true)
        val queueName = channel.queueDeclare("DummyQueue", true, false, false, emptyMap()).queue
        channel.queueBind(queueName, "SeriesAndEpisodes.MessageQueue:IDummyMessage", "routingKey")

        channel.close()
        connection.close()
        return this
    }

    fun startListening() {
        val connection = connectionFactory.newConnection()
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