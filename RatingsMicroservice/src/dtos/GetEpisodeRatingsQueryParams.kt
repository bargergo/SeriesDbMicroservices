package hu.bme.aut.ratings.dtos

import com.papsign.ktor.openapigen.annotations.parameters.QueryParam

data class GetEpisodeRatingsQueryParams (
    @QueryParam("id of the user") val userId: Int?,
    @QueryParam("seriesId") val seriesId: String?,
    @QueryParam("seasonId") val seasonId: Int?,
    @QueryParam("episodeId") val episodeId: Int?
)