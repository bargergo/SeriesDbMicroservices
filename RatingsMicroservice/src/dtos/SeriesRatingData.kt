package hu.bme.aut.ratings.dtos

import hu.bme.aut.ratings.validators.BetweenIntegers
import hu.bme.aut.ratings.validators.ExactLength

data class SeriesRatingData (
    val userId: Int,
    @ExactLength(24) val seriesId: String,
    @BetweenIntegers(1, 10) val rating: Int,
    val opinion: String
)