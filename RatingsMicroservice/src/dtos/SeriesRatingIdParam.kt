package hu.bme.aut.ratings.dtos

import com.papsign.ktor.openapigen.annotations.Path
import com.papsign.ktor.openapigen.annotations.parameters.HeaderParam
import com.papsign.ktor.openapigen.annotations.parameters.PathParam

@Path("{id}")
data class SeriesRatingIdParam (
    @PathParam("id of the series rating") val id: Int,
    @HeaderParam("Authorization header") val Authorization: String? = null
)