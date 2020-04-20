package hu.bme.aut.ratings.dtos

data class SeriesRatingInfo(
    val id: Int,
    val userId: Int,
    val seriesId: String,
    val rating: Int,
    val opinion: String
)