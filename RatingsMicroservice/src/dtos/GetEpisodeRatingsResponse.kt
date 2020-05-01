package hu.bme.aut.ratings.dtos

import com.papsign.ktor.openapigen.annotations.Response

@Response
data class GetEpisodeRatingsResponse (
    val ratings: List<EpisodeRatingInfo>
)