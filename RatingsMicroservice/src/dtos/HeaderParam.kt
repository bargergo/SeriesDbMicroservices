package hu.bme.aut.ratings.dtos

import com.papsign.ktor.openapigen.annotations.Path
import com.papsign.ktor.openapigen.annotations.parameters.HeaderParam

data class HeaderParam (
    @HeaderParam("Authorization header") val Authorization: String? = null
)