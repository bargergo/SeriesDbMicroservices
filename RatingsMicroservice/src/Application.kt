package hu.bme.aut.ratings

import com.fasterxml.jackson.annotation.JsonInclude
import com.papsign.ktor.openapigen.OpenAPIGen
import com.papsign.ktor.openapigen.annotations.type.common.ConstraintViolation
import com.papsign.ktor.openapigen.openAPIGen
import com.papsign.ktor.openapigen.route.apiRouting
import com.papsign.ktor.openapigen.route.tag
import com.papsign.ktor.openapigen.schema.namer.DefaultSchemaNamer
import com.papsign.ktor.openapigen.schema.namer.SchemaNamer
import hu.bme.aut.ratings.controllers.episodeRatings
import hu.bme.aut.ratings.controllers.seriesRatings
import hu.bme.aut.ratings.database.DatabaseFactory
import hu.bme.aut.ratings.services.EpisodeRatingService
import hu.bme.aut.ratings.services.RabbitService
import hu.bme.aut.ratings.services.SeriesRatingService
import io.ktor.application.Application
import io.ktor.application.call
import io.ktor.application.install
import io.ktor.features.*
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

    install(StatusPages) {
        exception<NotFoundException> { e ->
            call.respond(HttpStatusCode.NotFound)
        }
        exception<ConstraintViolation> { e ->
            call.respondText(e.localizedMessage,
                ContentType.Text.Plain, HttpStatusCode.BadRequest)
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


    install(ContentNegotiation) {
        jackson {
            setSerializationInclusion(JsonInclude.Include.NON_NULL)
        }
    }

    DatabaseFactory.init()
    RabbitService
        .dummyExchangeAndQueue()
        .updateSeriesRatingExchangeAndQueue()
        .startListening()

    install(Routing) {
        routing {
            get("/swagger/v1/swagger.json") {
                call.respond(this@module.openAPIGen.api)
            }

            get("/") {
                call.respondRedirect("/swagger-ui/index.html?url=/swagger/v1/swagger.json", true)
            }
        }

        apiRouting {
            tag(SwaggerTag("SeriesRatings")) {
                seriesRatings(SeriesRatingService())
            }
            tag(SwaggerTag("EpisodeRatings")) {
                episodeRatings(EpisodeRatingService())
            }
        }
    }
}

val Application.envKind get() = environment.config.property("ktor.environment").getString()
val Application.isDev get() = envKind == "dev"
val Application.isProd get() = envKind != "dev"
