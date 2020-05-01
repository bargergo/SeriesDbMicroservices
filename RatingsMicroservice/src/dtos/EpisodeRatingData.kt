package hu.bme.aut.ratings.dtos

import hu.bme.aut.ratings.validators.ExactLength

data class EpisodeRatingData(
    val userId: Int,
    @ExactLength(24) val seriesId: String,
    val seasonId: Int,
    val episodeId: Int,
    val rating: Int,
    val opinion: String
)