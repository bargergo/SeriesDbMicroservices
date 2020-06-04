package hu.bme.aut.ratings.dtos

import com.papsign.ktor.openapigen.annotations.type.number.floating.clamp.FClamp

data class AverageOfRatingsResponse (
    @FClamp(0.0, 10.0) val average: Float,
    val count: Long
)