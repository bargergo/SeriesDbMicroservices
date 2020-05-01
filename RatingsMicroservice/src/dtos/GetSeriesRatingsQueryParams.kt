package hu.bme.aut.ratings.dtos

import com.papsign.ktor.openapigen.annotations.parameters.QueryParam

data class GetSeriesRatingsQueryParams (
    @QueryParam("id of the user") val userId: Int?,
    @QueryParam("seriesId") val seriesId: String?
)