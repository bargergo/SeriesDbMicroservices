package hu.bme.aut.ratings.dtos

class SeriesRatingData (
    val userId: Int,
    val seriesId: String,
    val rating: Int,
    val opinion: String
)