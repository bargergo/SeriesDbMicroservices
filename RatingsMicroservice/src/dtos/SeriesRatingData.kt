package hu.bme.aut.ratings.dtos

import com.papsign.ktor.openapigen.annotations.type.number.integer.clamp.Clamp
import hu.bme.aut.ratings.validators.ExactLength

data class SeriesRatingData (
    val userId: Int,
    @ExactLength(24, "Series Id must be 24 characters long") val seriesId: String,
    @Clamp(1, 10) val rating: Int,
    val opinion: String
)