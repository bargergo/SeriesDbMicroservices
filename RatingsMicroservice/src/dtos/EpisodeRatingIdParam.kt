package hu.bme.aut.ratings.dtos

import com.papsign.ktor.openapigen.annotations.Path
import com.papsign.ktor.openapigen.annotations.parameters.HeaderParam
import com.papsign.ktor.openapigen.annotations.parameters.PathParam

@Path("{id}")
data class EpisodeRatingIdParam (
    @PathParam("id of the episode rating") val id: Int,
    @HeaderParam("Authorization header") val Authorization: String?
)