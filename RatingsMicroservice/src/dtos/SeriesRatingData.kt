package hu.bme.aut.ratings.dtos

import com.papsign.ktor.openapigen.annotations.type.number.integer.clamp.Clamp
import com.papsign.ktor.openapigen.annotations.type.string.length.Length
import com.papsign.ktor.openapigen.annotations.type.string.pattern.RegularExpression

data class SeriesRatingData (
        @Length(24, 24, "Wrong seriesId") @RegularExpression("^[a-zA-Z0-9]*$", "Wrong seriesId") val seriesId: String,
        @Clamp(1, 10, "Rating value must be between 1 and 10") val rating: Int,
        val opinion: String
)