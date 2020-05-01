package hu.bme.aut.ratings.dtos

import com.papsign.ktor.openapigen.annotations.Path
import com.papsign.ktor.openapigen.annotations.parameters.PathParam

@Path("/Series/{seriesId}/Season/{seasonId}/Episode/{episodeId}/Average")
data class GetAverageRatingForEpisodeParams (
    @PathParam("id of the series") val seriesId: String,
    @PathParam("id of the season") val seasonId: Int,
    @PathParam("id of the episode") val episodeId: Int
)