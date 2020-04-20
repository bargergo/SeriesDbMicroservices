package hu.bme.aut.ratings.dtos

class EpisodeRatingData(
    val userId: Int,
    val seriesId: String,
    val seasonId: Int,
    val episodeId: Int,
    val rating: Int,
    val opinion: String
)