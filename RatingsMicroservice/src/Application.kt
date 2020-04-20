package hu.bme.aut

import com.fasterxml.jackson.databind.SerializationFeature
import hu.bme.aut.controllers.episodeRatings
import hu.bme.aut.controllers.seriesRatings
import hu.bme.aut.database.DatabaseFactory
import hu.bme.aut.services.EpisodeRatingService
import hu.bme.aut.services.SeriesRatingService
import io.ktor.application.Application
import io.ktor.application.call
import io.ktor.application.install
import io.ktor.features.CallLogging
import io.ktor.features.ContentNegotiation
import io.ktor.features.DefaultHeaders
import io.ktor.features.StatusPages
import io.ktor.http.ContentType
import io.ktor.http.HttpStatusCode
import io.ktor.jackson.jackson
import io.ktor.request.path
import io.ktor.response.respondText
import io.ktor.routing.Routing
import org.slf4j.event.Level

fun main(args: Array<String>): Unit = io.ktor.server.netty.EngineMain.main(args)

@Suppress("unused") // Referenced in application.conf
@kotlin.jvm.JvmOverloads
fun Application.module(testing: Boolean = false) {
    install(DefaultHeaders)
    install(CallLogging) {
        level = Level.INFO
        filter { call -> call.request.path().startsWith("/") }
    }

    install(StatusPages) {
        exception<Throwable> { e ->
            call.respondText(e.localizedMessage,
                ContentType.Text.Plain, HttpStatusCode.InternalServerError)
        }
    }

    install(ContentNegotiation) {
        jackson {
            enable(SerializationFeature.INDENT_OUTPUT)
        }
    }

    DatabaseFactory.init()

    install(Routing) {
        seriesRatings(SeriesRatingService())
        episodeRatings(EpisodeRatingService())
    }
}

