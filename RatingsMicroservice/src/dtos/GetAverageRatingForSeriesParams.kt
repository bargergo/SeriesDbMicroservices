package hu.bme.aut.ratings.dtos

import com.papsign.ktor.openapigen.annotations.Path
import com.papsign.ktor.openapigen.annotations.parameters.PathParam

@Path("/Series/{seriesId}/Average")
data class GetAverageRatingForSeriesParams(
    @PathParam("id of the series") val seriesId: String
)