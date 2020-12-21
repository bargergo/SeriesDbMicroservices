package hu.bme.aut.ratings

import com.fasterxml.jackson.databind.DeserializationFeature
import com.fasterxml.jackson.databind.SerializationFeature
import com.papsign.ktor.openapigen.OpenAPIGen
import com.papsign.ktor.openapigen.annotations.type.common.ConstraintViolation
import com.papsign.ktor.openapigen.openAPIGen
import com.papsign.ktor.openapigen.route.apiRouting
import com.papsign.ktor.openapigen.route.tag
import com.papsign.ktor.openapigen.schema.namer.DefaultSchemaNamer
import com.papsign.ktor.openapigen.schema.namer.SchemaNamer
import com.zaxxer.hikari.HikariConfig
import com.zaxxer.hikari.HikariDataSource
import hu.bme.aut.ratings.controllers.episodeRatings
import hu.bme.aut.ratings.controllers.seriesRatings
import hu.bme.aut.ratings.database.DatabaseConnection
import hu.bme.aut.ratings.models.NotAuthorizedException
import hu.bme.aut.ratings.services.EpisodeRatingService
import hu.bme.aut.ratings.services.MessageQueueConfig
import hu.bme.aut.ratings.services.RabbitService
import hu.bme.aut.ratings.services.SeriesRatingService
import hu.bme.aut.ratings.utils.getenvCheckNotNull
import io.ktor.application.*
import io.ktor.features.*
import io.ktor.http.*
import io.ktor.jackson.*
import io.ktor.request.*
import io.ktor.response.*
import io.ktor.routing.*
import org.slf4j.event.Level
import utils.HikariDbConfig
import utils.RabbitMQConfig
import utils.SwaggerTag
import kotlin.reflect.KType

fun main(args: Array<String>): Unit = io.ktor.server.netty.EngineMain.main(args)

@Suppress("unused") // Referenced in application.conf
@kotlin.jvm.JvmOverloads
fun Application.module(testing: Boolean = false) {

    install(DefaultHeaders)
    install(CallLogging) {
        level = Level.INFO
        filter { call -> call.request.path().startsWith("/") }
    }

    install(ContentNegotiation) {
        jackson {
            enable(SerializationFeature.INDENT_OUTPUT)
            disable(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES)
        }
    }

    install(StatusPages) {
        exception<NotFoundException> { e ->
            call.respond(HttpStatusCode.NotFound)
        }
        exception<ConstraintViolation> { e ->
            call.respondText(e.localizedMessage,
                ContentType.Text.Plain, HttpStatusCode.BadRequest)
        }
        exception<NotAuthorizedException> { e ->
            call.respond(HttpStatusCode.Unauthorized)
        }
        exception<Throwable> { e ->
            if (isDev) {
                call.respondText(e.localizedMessage,
                    ContentType.Text.Plain, HttpStatusCode.InternalServerError)
            } else {
                call.respondText("An unexpected server error occured",
                    ContentType.Text.Plain, HttpStatusCode.InternalServerError)
            }

        }
    }

    install(OpenAPIGen) {
        // basic info
        info {
            version = "v1"
            title = "Ratings API"
            description = "API for series and episode ratings"
        }
        //optional custom schema object namer
        replaceModule(DefaultSchemaNamer, object: SchemaNamer {
            val regex = Regex("[A-Za-z0-9_.]+")
            override fun get(type: KType): String {
                return type.toString().replace(regex) { it.value.split(".").last() }.replace(Regex(">|<|, "), "_")
            }
        })
    }

    install(HikariDbConfig) {
        hikariDataSource = hikariDatasource
    }

    install(RabbitMQConfig) {
        val config = messageQueueConfig
        hostName = config.hostName
        userName = config.userName
        password = config.password
        queues = listOf(
            RabbitMQConfig.QueueConfig("DummyQueue", "SeriesAndEpisodes.MessageQueue:IDummyMessage"),
            RabbitMQConfig.QueueConfig("SeriesRatingUpdateQueue", "SeriesAndEpisodes.MessageQueue:ISeriesRatingChangedEvent")
        )
    }

    apiRouting {
        tag(SwaggerTag("SeriesRatings")) {
            seriesRatings(SeriesRatingService())
        }
        tag(SwaggerTag("EpisodeRatings")) {
            episodeRatings(EpisodeRatingService())
        }
    }

    routing {
        get("/openapi.json") {
            call.respond(this@module.openAPIGen.api)
        }

        get("/") {
            call.respondRedirect("/swagger-ui/index.html?url=/openapi.json", true)
        }
    }
}

val Application.envKind get() = getenvCheckNotNull("KTOR_ENV")
val Application.isDev get() = envKind == "dev"
val Application.isProd get() = envKind != "dev"

val Application.hikariDatasource get(): HikariDataSource {
    val config = HikariConfig("/hikari.properties")
    config.jdbcUrl = getenvCheckNotNull("db__jdbcUrl")
    config.username = getenvCheckNotNull("db__username")
    config.password = getenvCheckNotNull("db__password")
    config.validate()
    return HikariDataSource(config)
}

val Application.messageQueueConfig get(): MessageQueueConfig {
    return MessageQueueConfig(
        getenvCheckNotNull("MessageQueueSettings__Hostname"),
        getenvCheckNotNull("MessageQueueSettings__Username"),
        getenvCheckNotNull("MessageQueueSettings__Password")
    )
}