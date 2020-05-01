package hu.bme.aut.ratings.dtos

import com.papsign.ktor.openapigen.annotations.Response

@Response
data class EpisodeRatingInfo(
    val id: Int,
    val userId: Int,
    val seriesId: String,
    val seasonId: Int,
    val episodeId: Int,
    val rating: Int,
    val opinion: String
)