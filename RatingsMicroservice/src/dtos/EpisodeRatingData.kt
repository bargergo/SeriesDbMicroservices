package hu.bme.aut.ratings.dtos

import com.papsign.ktor.openapigen.annotations.type.number.integer.clamp.Clamp
import hu.bme.aut.ratings.validators.ExactLength

data class EpisodeRatingData(
    val userId: Int,
    @ExactLength(24) val seriesId: String,
    val seasonId: Int,
    val episodeId: Int,
    @Clamp(1, 10) val rating: Int,
    val opinion: String
)