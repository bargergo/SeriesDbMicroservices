package hu.bme.aut.ratings

import com.fasterxml.jackson.annotation.JsonInclude
import com.fasterxml.jackson.databind.SerializationFeature
import com.papsign.ktor.openapigen.OpenAPIGen
import com.papsign.ktor.openapigen.openAPIGen
import com.papsign.ktor.openapigen.route.apiRouting
import hu.bme.aut.ratings.controllers.episodeRatings
import hu.bme.aut.ratings.controllers.seriesRatings
import hu.bme.aut.ratings.database.DatabaseFactory
import hu.bme.aut.ratings.services.EpisodeRatingService
import hu.bme.aut.ratings.services.SeriesRatingService
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
import io.ktor.response.respond
import io.ktor.response.respondRedirect
import io.ktor.response.respondText
import io.ktor.routing.Routing
import io.ktor.routing.get
import io.ktor.routing.routing
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

    install(OpenAPIGen) {
        // basic info
        info {
            version = "0.0.1"
            title = "Ratings API"
            description = "API for series and episode ratings"
        }
        //optional
        schemaNamer = {
            //rename DTOs from java type name to generator compatible form
            val regex = Regex("[A-Za-z0-9_.]+")
            it.toString().replace(regex) { it.value.split(".").last() }.replace(Regex(">|<|, "), "_")
        }
    }


    install(ContentNegotiation) {
        jackson {
            setSerializationInclusion(JsonInclude.Include.NON_NULL)
        }
    }

    DatabaseFactory.init()

    install(Routing) {
        routing {
            get("/openapi.json") {
                call.respond(this@module.openAPIGen.api)
            }

            get("/") {
                call.respondRedirect("/swagger-ui/index.html?url=/openapi.json", true)
            }
        }

        apiRouting {
            seriesRatings(SeriesRatingService())
            episodeRatings(EpisodeRatingService())
        }
    }
}

