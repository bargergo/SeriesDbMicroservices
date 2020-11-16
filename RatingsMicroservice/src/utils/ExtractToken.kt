package hu.bme.aut.ratings.utils

import com.fasterxml.jackson.module.kotlin.jacksonObjectMapper
import com.fasterxml.jackson.module.kotlin.readValue
import java.util.*

fun extractToken(token: String): Map<String,String> {
    val mapper = jacksonObjectMapper()
    var encodedString = token.substring(token.indexOf(".") + 1)
    encodedString = encodedString.substring(0, encodedString.indexOf("."))
    val decodedBytes = Base64.getDecoder().decode(encodedString)
    val decodedString = String(decodedBytes)
    val map: Map<String,String> = mapper.readValue(decodedString)
    return map
}