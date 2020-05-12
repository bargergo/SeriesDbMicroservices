package hu.bme.aut.ratings.dtos

data class SeriesRatingChangedEvent (
    val seriesId: String,
    val count: Long,
    val average: Float
)